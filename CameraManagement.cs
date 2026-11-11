using HalconDotNet;
using MvCameraControl;
using Sunny.UI;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Windows.Forms;
using XSao.Help;

namespace XSao
{
    #region 提示：类级说明
    // [提示：]【类级说明】相机管理界面：左边实时预览（pictureBox1，由 SDK 直接渲染），右边 Halcon 处理窗口（pictureBox2）。
    // [提示：]        为什么要两块画布：SDK 的 ImageRender 只能原样显示，而识别和绘图要用 HALCON 的窗口句柄，
    // [提示：]        两套绘图体系互不相通，各占一块最省事。
    // [提示：]【上下游】上游：新主页 GoPage(nameof(CameraManagement)) 创建；下游：HkServerHelp（相机）、GLOSSED（识别）。
    #endregion
    public partial class CameraManagement : UIForm
    {
        // [提示：]【字段】Loglist＝界面日志缓存（只增不减，见问题清单）；hv_CodeTypes＝一维码码制，固定 EAN-13。
        // [提示：]        改码制只改这一处，别在调用处另写字符串，否则同一界面会出现两种码制混用。
        List<CameraHelp> Loglist = new List<CameraHelp>();
        HTuple hv_CodeTypes = "EAN-13";
        #region 优化：解决什么问题
        // [优化：]【解决什么问题】原来 PLC 相关的地址、IP、"16"这个魔法数全写死在 PLC_Monitor 方法体里，
        // [优化：]        而且 PLC 对象声明在类中间（244 行），离使用处很远，看不出这几件事是一套配置。
        // [优化：]【优化点】把这套参数统一提成常量放字段区，现场换 PLC 只改这几行，不用读方法体。
        // [优化：]【依据：现场 PLC 导出程序】MBUS_SERVER 的 HoldStart=&VB2000 → 40001 就是 VW2000。
        // [优化：]        该程序里对 VW2000 只有 MOVW +0（清零），从没写过 16 → 原来"等 16"的判据永远不成立。
        // [优化：]        PLC 真正给出来的状态是 VW2000 的 bit12（V2001.4，值 4096）：转动那 3 秒写 1，转完写 0。
        #endregion
        private const string PlcIp = "192.168.0.10";        // 现场 IP 未定，先用占位值，接线前必须核对
        private const int PlcPort = 502;                   // Modbus TCP 默认端口
        private const byte PlcSlaveId = 1;                 // PLC 从站号，西门子侧一般 1
        private const ushort PlcStatusAddress = 0;         // 40001 = VW2000：PLC 在这个字里回写"转动中"位
        private const ushort PlcBitRotating = 0x0010;      // 16 = bit4 = V2001.4（VW2000 低字节是 VB2001，所以 V2001.4 落在整字 bit4）
        private const ushort PlcBitSoftStart = 0x0100;     // 256 = V2000.0，需在 PLC 的 Network 8 里并联 `O V2000.0`
        private const ushort PlcTriggerModel = 16;         // 触发来源码（沿用原约定，只用于日志；值与 PlcBitRotating 相同）
        private const int ShotStartDelayMs = 1500;           // 开始转动后先跳过这一段（躲开加速阶段的糊帧）
        public int ShotIntervalMs = 500;                    // 连拍间隔：想调就改这个数（100~300ms 之间试）
        private const int ShotTimeoutMs = 5000;             // 这一轮连拍最多持续多久（兜底，防止一直拍）
        private const int SoftStartHoldMs = 300;           // 软启动位保持时长：按住 300ms 再撤，等于"按下—松手"
        private PlcHelp plcHelp = new PlcHelp(PlcIp, PlcPort, PlcSlaveId);
        private Thread plcThread;                          // PLC 轮询线程
        private volatile bool plcRunning;                  // 轮询线程退出标志（volatile：跨线程读写可见）
        private volatile bool softStartRequest;            // UI 线程置真 → 轮询线程去写软启动位（Modbus 只在轮询线程里用）
        private bool softStartPending;                     // 软启动位已写 1、还没撤销
        private int softStartTick;                         // 软启动位写下那一刻的 TickCount
        private bool lastRotating;                         // 上一拍的"转动中"状态，用来判上升沿（0→1 = 开始转）
        private int rotateStartTick;                       // 开始转动那一刻的 TickCount，本轮连拍的计时起点
        private int nextShotTick;                          // 下一张该拍的时刻（TickCount）
        private bool shotLoop;                             // 这一轮"连拍到成功"还在进行中
        private volatile bool shotOk;                      // 识别完成后回填：这一张已经识别出一维码了
        private int shotCount;                             // 本轮已拍张数（只用于日志）
        private bool halconBusy;                           // 识别防重入：上一张还没识别完就不接新触发
        public CameraManagement()                                               // 构造：建 help、挂取流回调、绑 HALCON 窗口
        {
            InitializeComponent();
            help = new HkServerHelp();
            #region 问题：为什么把 help（海康相机 SDK 封装对象）声明成窗体字段，并且放在构造函数里创建？
            // 【答：因为它是“整个窗体共用一份”的长生命周期对象，不是某一次点击的临时变量：
            //      ①按钮事件、DisplayImage、HalconOCR、CameraManagement_FormClosing 全都要用同一个 help（12 行声明）；
            //         只有做成字段才能跨方法共享，局部变量做不到。
            //      ②放构造函数里，能保证“窗口一出现 help 就已经可用”：SDK 初始化、showImageCallback 的注册（18 行）都在这里完成，
            //         之后无论先点哪个按钮，都不会出现 help 为 null 或回调没挂上。
            //      ③反例：若在按钮里 new，每点一次就多一个 SDK 实例（Initialize 调多次、设备句柄多份），
            //         关窗时也没法统一 StopCamera，属于典型资源失控。
            //      代价：构造函数里做 SDK 初始化，一旦失败窗口直接打不开（构造抛异常）；更稳的写法是构造只声明字段、
            //         在 Load 或首次点“打开相机”时初始化并 try/catch 提示。
            //      依据：WinForms 中窗体字段的初始化时机就是构造方法；下方【点拨】讲的是同一件事的“好处”一面。】
            //【点拨】help=海康相机SDK封装类；放构造函数=窗口一打开就创建好相机工具，
            //       后面点按钮直接用；也方便在 new 时就把回调方法注册好（showImageCallback）
            #endregion
            help.showImageCallback = showImageCallBack;
            //【点拨】关掉"自动生成列"：默认 DataGridView 会把数据源所有属性都建成列；
            //       设 false 后只用 Designer 手工配好的两列，靠 DataPropertyName 对上 MakeTime/MakeLog。
            Camera_Log.AutoGenerateColumns = false;
            //绑定Halcon的句柄
            BandHalconHandler();
            #region 优化：解决什么问题
            // [优化：]【解决什么问题】构造时容器还没跑完布局，pictureBox2 只是 Designer 里记的设计尺寸（565×274），
            // [优化：]        等页面被 New_HomePage 挂进 1420×700 的 Panel_HomePage（Padding 清零、Dock=Fill 重排）后，
            // [优化：]        pictureBox2 实际会变成 569×284 左右，而 HALCON 窗口仍停在 564×273 → 右下角露出
            // [优化：]        pictureBox2 自己的灰色背景（肉眼就是"红色没铺满画布"）。
            // [优化：]【优化点】挂 SizeChanged：控件尺寸一变，就把 HALCON 窗口外框同步成控件尺寸，并重画当前图。
            // [优化：]【为什么不改功能】只补"窗口与控件同尺寸"，OpenWindow 的参数、背景色、句柄用途、业务方法一律不动；
            // [优化：]        没图时不重画，有图时按原有 SetPart+DispObj 的同一写法重画，显示结果与原逻辑一致。
            // [优化：]【怎么验证】把 pictureBox2.BackColor 临时改成 Lime，红框四周无绿边即通过；
            // [优化：]        或打印 GetWindowExtents 的 width/height 与 pictureBox2.Width/Height 对比，应只差 1 像素。
            #endregion
            pictureBox2.SizeChanged += PictureBox2_SizeChanged;
        }
        // [提示：]【职责】把 HALCON 绘图窗口绑定到 pictureBox2 上，之后所有 DispObj/DispText 都往这个句柄输出。
        // [提示：]【时序】必须在控件句柄创建之后调用；这里放在构造函数末尾，pictureBox2.Handle 会强制创建句柄。
        // [提示：]【全局栈】HDevWindowStack.Push：HALCON 的“当前窗口”是线程级全局状态，push 后部分算子可不显式传句柄。
        private void BandHalconHandler()                                        // 把 HALCON 窗口句柄绑到 pictureBox2
        {
            //绑定Halcon的句柄
            HOperatorSet.SetWindowAttr("background_color", "red");
            HOperatorSet.OpenWindow(0, 0, pictureBox2.Width - 1, pictureBox2.Height - 1, pictureBox2.Handle, "visible", "", out hv_WindowHandle);
            #region 问题：OpenWindow 的宽和高为什么要减 1？第 5 个参数原本是 0，为什么改成 pictureBox2.Handle？
            // 【答：①减 1：窗口尺寸是“像素个数”，而控件宽 560 的有效坐标是 0~559。传 560 会让 Halcon 认为
            //          窗口比画布多 1 像素，右/下边缘被裁掉 1 像素，画面轻微错位；所以用 Width-1、Height-1。
            //      ②第 5 个参数是 FatherWindow（父窗口句柄）：0 表示自己开一个独立顶层窗口；
            //          传 pictureBox2.Handle 表示把 Halcon 窗口嵌进这个控件的客户区，图像才会显示在窗体里。
            //      ③顺序：SetWindowAttr 设背景色（这里 red）→ OpenWindow 建窗 → Push 进栈供后续算子取用。
            //      依据：HALCON open_window(Row, Column, Width, Height, FatherWindow, Mode, Machine) 的参数定义。
            //      影响范围：本窗体所有 Halcon 绘制与识别都向这个句柄输出，改错会导致“看不见图/图跑到另一个窗口”。】
            #endregion
            HDevWindowStack.Push(hv_WindowHandle);
        }

        // [优化：]【新增】pictureBox2 尺寸变化时，让 HALCON 窗口跟上控件尺寸。
        // [优化：]【上游】用户拖动/最大化窗体、切页导致 Dock=Fill 重排，都会触发 SizeChanged。
        // [优化：]【影响】只动窗口外框与重画，不碰任何识别/相机逻辑。
        private void PictureBox2_SizeChanged(object sender, EventArgs e)        // pictureBox2 尺寸变了就同步 HALCON 窗口
        {
            SyncHalconWindow();
        }

        // [优化：]【新增】把 HALCON 窗口外框改成 pictureBox2 当前尺寸；旧窗口失效时重开一个。
        // [优化：]【为什么还要判 0】构造期/页面隐藏时控件可能是 0 宽高，此时改窗口会让 HALCON 报尺寸非法。
        private void SyncHalconWindow()                                         // 按控件当前尺寸改 HALCON 窗口
        {
            if (pictureBox2.Width <= 1 || pictureBox2.Height <= 1)
            {
                return;
            }
            if (hv_WindowHandle == null || hv_WindowHandle.Length == 0)
            {
                // [优化：]句柄还没建（正常流程不会走到这里），补建一次再继续同步
                BandHalconHandler();
            }
            try
            {
                HOperatorSet.SetWindowExtents(hv_WindowHandle, 0, 0, pictureBox2.Width - 1, pictureBox2.Height - 1);
                // [优化：]窗口变大后新露出来的区域不会自动重刷，先清一次窗口让它填满背景色（red），
                // [优化：]再按 Himage 重画；没有图时就是纯红底，不会出现一块块的旧像素残留。
                HOperatorSet.ClearWindow(hv_WindowHandle);
            }
            catch (Exception)
            {
                // [优化：]切页时 New_HomePage 会 Controls.Clear() 再 Add，pictureBox2 的 HWND 会被重建，
                // [优化：]旧 HALCON 窗口的父窗口已经没了 → SetWindowExtents 报错 → 关掉重开一个。
                RebindHalconWindow();
            }
            RedrawCurrentImage();
        }

        // [优化：]【新增】关闭旧 HALCON 窗口并按当前控件尺寸重开。
        // [优化：]【为什么连栈一起清】HDevWindowStack 是线程级全局栈，每次 OpenWindow 都会 Push；
        // [优化：]        重开前先 Pop 掉旧的，栈里才不会越积越多、GetActive 取到已关闭的句柄。
        private void RebindHalconWindow()                                       // 关旧窗口、按新尺寸重开（并清窗口栈）
        {
            try
            {
                if (hv_WindowHandle != null && hv_WindowHandle.Length > 0)
                {
                    HOperatorSet.CloseWindow(hv_WindowHandle);
                }
            }
            catch (Exception)
            {
                // [优化：]旧窗口已随父控件销毁，关不掉属正常情况，忽略
            }
            try
            {
                if (HDevWindowStack.IsOpen())
                {
                    HDevWindowStack.Pop();
                }
            }
            catch (Exception)
            {
            }
            BandHalconHandler();
        }

        // [优化：]【新增】窗口尺寸变化后，之前画上去的画面会被裁掉或留空，按当前 Himage 重画一次。
        // [优化：]【写法来源】与 DisplayImage / HalconOCR 里的 SetPart + DispObj 完全一致，只是抽出来复用。
        // [优化：]【注意】重画的是"图像"，识别框/字符标注属于一次性叠加绘制，尺寸变化后需要重新点识别才会回来。
        private void RedrawCurrentImage()                                       // 尺寸变化后按当前图像重画
        {
            if (Himage == null)
            {
                return;
            }
            HOperatorSet.GetImageSize(Himage, out HTuple hv_Width, out HTuple hv_Height);
            HOperatorSet.SetPart(hv_WindowHandle, 0, 0, hv_Height, hv_Width);
            HOperatorSet.DispObj(Himage, hv_WindowHandle);
        }

        // [提示：]【运行线程】本方法由取流线程调用，不是 UI 线程——所以下面必须先判 InvokeRequired。
        // [提示：]【分工】这里只负责把原始帧刷到 pictureBox1；Halcon 那边要等用户点“获取图片”才转图。
        // [提示：]【参数】@out 是 C# 关键字 out 的同名标识符，前面加 @ 只是让它当普通变量名用，没有其它含义。
        private void showImageCallBack(IDevice device, IFrameOut @out)          // 取流回调：把原始帧刷到 pictureBox1
        {
            #region 问题：showImageCallBack 里为什么要先判断 pictureBox1.InvokeRequired？
            // 【答：因为这个回调不是 UI 线程调用的——它由 HkServerHelp.ReceiveThreadProcess 的后台取流线程触发。
            //      WinForms 控件有线程亲和性：非创建线程直接改控件会抛“跨线程访问控件”的 InvalidOperationException。
            //      InvokeRequired=true → 控件属于别的线程，必须用 Invoke 把显示动作丢回创建它的 UI 线程执行；
            //      InvokeRequired=false → 当前已在 UI 线程（例如同步调用），直接显示，省一次调度。
            //      依据：Control.InvokeRequired / Control.Invoke 的标准用法。
            //      影响范围：相机每次出图都走这里；写错就是“每来一帧崩一次”，且崩在后台线程、堆栈难读。】
            #endregion
            if (pictureBox1.InvokeRequired)
            {
                pictureBox1.Invoke(new Action(() =>
                {
                    device.ImageRender.DisplayOneFrame(pictureBox1.Handle, @out.Image);
                }));
            }
            else
            {
                device.ImageRender.DisplayOneFrame(pictureBox1.Handle, @out.Image);
            }
        }

        public HkServerHelp help;

        #region 提示：流程
        // [提示：]【流程】枚举设备 → 判空 → 打开第一台 → 开始取流 → 提示并记日志。
        // [提示：]【简化】固定取 result.data[0]：多相机时不提供选择，属于当前版本的业务限制。
        // [提示：]【缺陷】OpenDevice 成功但 StartGrabbing 失败时，相机已被打开却没停流/关闭，
        // [提示：]        资源停在“开着但没采集”的中间态，下次再点打开可能报设备被占用（见问题清单）。
        #endregion
        private void But_OpenCam_Click(object sender, EventArgs e)              // 打开相机：枚举→打开第一台→开始取流
        {
            //先查询设备
            APIResult<List<string>> result = help.GetDevice();
            if (!result.IsSuccess)
            {
                this.ShowWarningTip(result.Message);
                Camera_Logs(result.Message);
                return;
            }
            if (result.data.Count == 0)
            {
                this.ShowWarningTip("没有可用的相机");
                Camera_Logs("连接相机失败：没有可用的相机！");
                return;
            }
            //打开设备
            APIResult<bool> openResult = help.OpenDevice(result.data[0]);
            //判断相机是否打开成功
            if (!openResult.IsSuccess)
            {
                this.ShowWarningTip("相机打开失败！");
                Camera_Logs("连接相机失败：相机打开失败！");
                return;
            }
            if (!help.StartGrabbing().IsSuccess)
            {
                this.ShowWarningTip("开始抓图失败！");
                return;
            }
            this.ShowSuccessTip("相机打开成功！");
            Camera_Logs("相机打开成功！");
            #region 优化：原代码的问题
            //通过plc去监听
            // [优化：]【原代码的问题】源码在这里是 `PLC_Monitor(); DisplayImage(); HalconOCR();` 三行连写，
            // [优化：]        而 PLC_Monitor 本身是一个跑在 UI 线程上的 while 死循环（无 Sleep/无 Invoke/无退出条件），
            // [优化：]        所以点"打开相机"后界面会冻死，后面两行永远执行不到，等于自动取图+识别从来没跑过。
            // [优化：]【优化点】改成只调 StartPlcMonitor()：轮询搬到后台线程，UI 线程立刻返回；
            // [优化：]        拍照+识别交给"PLC 触发"这条链（见下方 OnPlcTrigger / ShowAndRecognize）。
            // [优化：]【为什么不改功能】"打开相机 → 等 PLC 说拍就拍 → 识别 → 显示结果"的意图完全保留，
            // [优化：]        只是把"等 PLC"从 UI 线程挪到后台，且不再重复开流。
            // [优化：]【怎么验证】点"打开相机"后界面不卡（能拖动窗体、能关窗），日志出现"PLC监控已启动"；
            // [优化：]        随后当 PLC 的 40001 由非 16 变成 16 时，右侧 HALCON 窗口出图并打印识别结果。
            #endregion
            StartPlcMonitor();
        }

        // [优化：]【新增】启动 PLC 轮询线程，替代原来跑在 UI 线程里的 PLC_Monitor 死循环。
        // [优化：]【线程】IsBackground=true：窗体关闭/进程退出时不会被这条线程拖住
        // [优化：]        （原来的死循环连"关窗"都关不掉，只能杀进程）。
        private void StartPlcMonitor()                                          // 启动PLC轮询线程（后台）
        {
            if (plcRunning)
            {
                // [优化：]防连点：连点两次"打开相机"不会起两条轮询线程
                Camera_Logs("PLC监控已在运行，忽略重复启动");
                return;
            }
            plcRunning = true;
            plcThread = new Thread(PlcLoop);
            plcThread.IsBackground = true;
            plcThread.Start();
            Camera_Logs("PLC监控已启动，等待PLC的『转动中』位变 1（开始转）");
        }

        // [优化：]【新增】停止轮询并断开 PLC。关窗/切页必须调：
        // [优化：]        否则后台线程会一直读 PLC，还会往已经销毁的控件写日志（跨线程异常）。
        private void StopPlcMonitor()                                           // 停止轮询并断开PLC
        {
            plcRunning = false;
            if (plcThread != null && plcThread.IsAlive)
            {
                // [优化：]最多等 1 秒：线程内部有 Sleep(10~2000ms)，醒来看到标志位就自己退出了
                plcThread.Join(1000);
            }
            plcThread = null;
            plcHelp.DisConnectPlc();
        }

        // [优化：]【新增】轮询主体：不在循环条件里重连，只做"读一次、判一次、睡一会"。
        private void PlcLoop()                                                  // 轮询主体：读触发寄存器、判上升沿
        {
            while (plcRunning)
            {
                // [优化：]【连接策略】没连上才连；连不上退避 2 秒再试。
                // [优化：]        原代码 while (plcHelp.ConnectPlc()) 是每一圈都 new Plc + Open，
                // [优化：]        既慢又会把 PLC 的有限连接名额占满。
                if (!plcHelp.IsPlc_Connect)
                {
                    if (!plcHelp.ConnectPlc())
                    {
                        Thread.Sleep(2000);
                        continue;
                    }
                    Camera_LogsSafe("PLC已连接");
                }

                ushort value;
                if (!plcHelp.TryReadUShort(PlcStatusAddress, out value))
                {
                    // [优化：]TryReadUShort 失败时已把 IsPlc_Connect 置 false，下一圈自然走上面的重连分支
                    Thread.Sleep(200);
                    continue;
                }

                #region 软启动：软件模拟"按一下绿钮"
                // [优化：]【前提】PLC 的 Network 8 里并联一个触点 `O V2000.0`（配合说明见交付时给的改法），
                // [优化：]        此后往 40001 写 256 就等于"按下绿钮"，写 0 等于"松手"。
                // [优化：]【为什么 300ms 后要写 0】M0.0 是自锁的（`LD I0.0 / O M0.0`），按下瞬间锁住就够了；
                // [优化：]        若一直保持 256，一旦本轮因 M5.3 报警没跑起来，这一位会残留，下次一上电又自己启动。
                // [优化：]【为什么在轮询线程里写、不在按钮里写】PlcHelp 只有一套 master，两个线程同时读写会互相踩。
                #endregion
                if (softStartRequest)
                {
                    softStartRequest = false;
                    if (plcHelp.TryWriteUShort(PlcStatusAddress, PlcBitSoftStart))
                    {
                        softStartTick = Environment.TickCount;
                        softStartPending = true;
                        Camera_LogsSafe("软启动：已向 PLC 下发一次「按下绿钮」");
                    }
                    else
                    {
                        Camera_LogsSafe("软启动失败：写 40001 失败（连接可能已断）");
                    }
                }
                if (softStartPending && Environment.TickCount - softStartTick >= SoftStartHoldMs)
                {
                    softStartPending = false;
                    if (plcHelp.TryWriteUShort(PlcStatusAddress, 0))
                    {
                        Camera_LogsSafe("软启动：已撤销（等价于松手）");
                    }
                }

                #region 触发判据：开始转以后"连拍"，拍到第一张能识别的就收手
                // [优化：]【这个 16 到底是什么】40001 = VW2000，它的低字节是 VB2001、高字节是 VB2000。
                // [优化：]        PLC 在 Network 10 里写的是 `= V2001.4`，落在整个字里就是 bit4 = 16。
                // [优化：]        所以原代码"读到 16"本身是对的 —— 之前误判成"PLC 从没写过 16"，是字位换算搞错了。
                // [优化：]【为什么不再"卡一个固定时刻"】滚筒起步加速段和收尾段转速不稳，行频跟着不匹配，
                // [优化：]        两头拍出来都是糊的；中间那段清楚，但具体在第几百毫秒谁也说不准。
                // [优化：]        固定延时等于赌一个数字，换个转速、换根物料就得重调。
                // [优化：]【现在的做法】把"猜时刻"换成"多试几张"：
                // [优化：]        ① 看到 16 由 0→1 = 开始转，先等 ShotStartDelayMs（躲开加速的糊帧）；
                // [优化：]        ② 之后每 ShotIntervalMs 取一帧识别一次，成功就停；
                // [优化：]        ③ 兜底：超过 ShotTimeoutMs 还没识别到就停，防止一直拍。
                // [优化：]【怎么调】间隔改 ShotIntervalMs：太小会跟识别抢时间（halconBusy 会丢帧，等于白拍），
                // [优化：]        太大会跳过码面 → 一般 100~300ms。起步还是糊就把 ShotStartDelayMs 加大。
                // [优化：]【读之外的写】本方法自己只读；唯一的写是"软启动脉冲"（见上面的 softStart 分支），
                // [优化：]        而且写完 300ms 就撤销，不跟 PLC 每周期回写的 16 抢位。
                #endregion
                bool rotating = (value & PlcBitRotating) != 0;
                if (rotating && !lastRotating)
                {
                    // 0→1：刚开始转。起一轮"连拍到成功"的会话
                    rotateStartTick = Environment.TickCount;
                    nextShotTick = rotateStartTick + ShotStartDelayMs;
                    shotLoop = true;
                    shotOk = false;
                    shotCount = 0;
                    Camera_LogsSafe("检测到开始转动：先等 " + ShotStartDelayMs + "ms 躲糊帧，之后每 "
                                    + ShotIntervalMs + "ms 拍一张，识别成功即停");
                }
                if (shotLoop)
                {
                    if (shotOk)
                    {
                        shotLoop = false;
                        Camera_LogsSafe("已识别到一维码，停止连拍（本次共拍 " + shotCount + " 张）");
                    }
                    else if (Environment.TickCount - rotateStartTick >= ShotTimeoutMs)
                    {
                        shotLoop = false;
                        Camera_LogsSafe("连拍超时（" + ShotTimeoutMs + "ms）仍未识别到，停止（共拍 " + shotCount + " 张）");
                    }
                    else if (Environment.TickCount - nextShotTick >= 0)
                    {
                        nextShotTick = Environment.TickCount + ShotIntervalMs;
                        shotCount++;
                        OnTakeTimeUp();
                    }
                }
                lastRotating = rotating;

                // [优化：]10ms 轮询：够快，又不会把 PLC 通信打满
                Thread.Sleep(10);
            }
        }

        #region 优化：连拍里的一次取图
        // [优化：]【这一层为什么存在】连拍循环只负责"什么时候拍"，真正取帧、转图、交 UI 还是走 OnPlcTrigger，
        // [优化：]        这样物理绿钮那条路和软件连拍这条路共用同一套取图流程，不会出现两套逻辑各自跑偏。
        // [优化：]【要不要在这里加 Sleep】不用。线扫相机是连续出帧，取"当前最新一帧"就是此刻的画面，
        // [优化：]        在这里再等几十毫秒只会让循环变慢、白丢机会。
        #endregion
        private void OnTakeTimeUp()                                             // 连拍中的一张：交给统一的取图流程
        {
            OnPlcTrigger(PlcTriggerModel);
        }

        #region 优化：新增
        // [优化：]【新增】一次拍照请求：取最新一帧 → 转 HALCON 图像 → 交回 UI 线程显示并识别。
        // [优化：]【为什么不在这里 StartGrabbing】相机在 But_OpenCam_Click 里已经开好了连续采集，
        // [优化：]        再调一次 StartGrabbing 会多起一条采集线程抢同一台相机的帧（原代码每触发一次就加一条）。
        // [优化：]        线扫相机连续出图，直接取"当前最新一帧"就是触发时刻的图像，帧间隔只有几毫秒。
        #endregion
        private void OnPlcTrigger(ushort model)                                 // 取最新帧→转图→交UI识别（PLC转完/手动补拍共用）
        {
            if (help.device == null)
            {
                // [优化：]只判设备是否打开：isGrabbing 是 HkServerHelp 的私有字段，业务层不该直接碰它；
                // [优化：]设备为空说明相机没打开，触发也没有帧可取。相机中途掉线由 TryGetLatestFrame 返回 false 兜住。
                Camera_LogsSafe("相机未打开，忽略本次拍照请求");
                return;
            }
            if (halconBusy)
            {
                // [优化：]防重入：上一张还在识别（或还在 UI 队列里），这一拍直接丢掉，避免堆积和内存暴涨
                Camera_LogsSafe("上一拍还在识别，本次触发被跳过（来源码" + model + "）");
                return;
            }
            halconBusy = true;

            IFrameOut frame;
            if (!help.TryGetLatestFrame(out frame))
            {
                halconBusy = false;
                Camera_LogsSafe("触发拍照失败：相机还没有可用帧");
                return;
            }

            HObject image;
            if (!HalconHelp.Instance.TryCreateHalconImage(frame.Image, out image))
            {
                halconBusy = false;
                Camera_LogsSafe("触发拍照失败：帧转 HALCON 图像失败");
                return;
            }

            // [优化：]【为什么转换留后台、显示和识别回 UI】TiaoMa 会把框和文字直接画在 hv_WindowHandle 上，
            // [优化：]        而 HALCON 窗口挂在 pictureBox2 的句柄上（有线程亲和性），后台线程直接画容易崩。
            // [优化：]        GenImage1 是像素拷贝（不是引用外部内存），所以在后台转换是安全的。
            if (IsDisposed || !IsHandleCreated)
            {
                image.Dispose();
                halconBusy = false;
                return;
            }
            BeginInvoke(new Action(() => ShowAndRecognize(image, model)));
        }

        // [优化：]【新增】UI 线程里执行：换图 → 显示 → 识别 → 记日志；无论成败都释放 busy 标志。
        private void ShowAndRecognize(HObject image, ushort model)              // UI线程里：换图→显示→识别→记日志
        {
            try
            {
                if (Himage != null)
                {
                    // [优化：]旧图先释放，否则每触发一次泄漏一个 HALCON 图像句柄
                    Himage.Dispose();
                }
                Himage = image;

                HOperatorSet.GetImageSize(Himage, out HTuple hv_Width, out HTuple hv_Height);
                HOperatorSet.SetPart(hv_WindowHandle, 0, 0, hv_Height, hv_Width);
                HOperatorSet.DispObj(Himage, hv_WindowHandle);

                Camera_Logs("触发取图：来源码" + model + "，图像已显示");
                // [优化：]复用原有识别流程（HalconOCR 内部做条码识别 + 画框 + 写日志），不另写一套。
                // [优化：]拿它的返回值当"这一张是否可用"：>0 表示认出码了，连拍循环据此收手。
                int codeCount = HalconOCR();
                shotOk = codeCount > 0;
            }
            catch (Exception ex)
            {
                // [优化：]识别异常只记日志，不让它把 UI 线程打崩（原代码 PLC_Monitor 里没有任何 try/catch）
                Camera_Logs("PLC触发拍照异常：" + ex.Message);
            }
            finally
            {
                halconBusy = false;
            }
        }

        // [优化：]【新增】线程安全的日志入口：后台线程不能直接改 DataGridView 的数据源，必须切回 UI 线程。
        private void Camera_LogsSafe(string log)                                // 线程安全写日志（自动 Invoke 回UI）
        {
            if (Camera_Log == null || IsDisposed)
            {
                return;
            }
            if (Camera_Log.InvokeRequired)
            {
                Camera_Log.BeginInvoke(new Action(() => Camera_Logs(log)));
            }
            else
            {
                Camera_Logs(log);
            }
        }

        // [提示：]【流程】文件对话框选图 → ReadImage 读成 Himage → 取尺寸 → SetPart → DispObj 显示。
        // [提示：]【缺陷】第 104 行直接覆盖 Himage，没有先 Dispose 旧图：每选一次图泄漏一个 HALCON 图像句柄。
        private void But_ChoicePhoton_Click(object sender, EventArgs e)         // 选本地图片并显示到 HALCON 窗口
        {
            //在文件内选择照片
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Filter = GetImageFilter();
            dialog.Title = "选择图像";
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                HOperatorSet.ReadImage(out Himage, dialog.FileName);
                // [提示：]【内存】这里直接覆盖 Himage，上一张图的 HALCON 句柄没释放 → 反复选图内存持续上涨。
                // [提示：]        正确做法：先 if (Himage != null) Himage.Dispose()，再 ReadImage（DisplayImage 里就是这么处理的）。
                HOperatorSet.GetImageSize(Himage, out HTuple hv_Width, out HTuple hv_Height);
                HOperatorSet.SetPart(hv_WindowHandle, 0, 0, hv_Height, hv_Width);
                HOperatorSet.DispObj(Himage, hv_WindowHandle);
                Camera_Logs("选择图片成功！" + dialog.FileName);
            }
        }
        private static string GetImageFilter()                                  // 拼图片对话框的文件过滤串
        {
            return "所有图像|*.bmp;*.jpg;*.jpeg;*.png;*.tif;*.tiff;*.gif;*.jp2;*.pcx;*.pgm;*.ppm;*.pbm;*.xwd;*.ima"
                 + "|BMP|*.bmp"
                 + "|JPEG|*.jpg;*.jpeg"
                 + "|PNG|*.png"
                 + "|TIFF|*.tif;*.tiff"
                 + "|所有文件|*.*";
        }

        public HObject Himage;
        private HTuple hv_WindowHandle;

        // [提示：]【流程】判相机是否打开 → DisplayImage（把当前帧转成 Halcon 图并显示）→ HalconOCR（直接识别）。
        // [提示：]【缺陷】help.frameForSave 可能为 null（相机刚打开、还没出第一帧），DisplayImage 里会抛 NullReferenceException。
        private void But_GetPhoto_Click(object sender, EventArgs e)             // 获取图片：取当前帧→显示→识别
        {
            //判断是否打开相机
            if (help.device == null)
            {
                this.ShowWarningTip("相机未打开！");
                Camera_Logs("获取图片失败：相机未打开！");
                return;
            }
            DisplayImage();
            //运行一下But_HalconRun
            HalconOCR();
            #region 问题：HalconOCR() 调用时为什么不需要传参数？
            // 【答：因为它读的是类字段，不是形参——图像 Himage（121 行）和窗口句柄 hv_WindowHandle（122 行）
            //      都是本窗体的成员，BandHalconHandler / DisplayImage 已经把它们赋好了，任何方法都能直接访问。
            //      这是 WinForms 里“用字段当共享状态”的典型写法：调用点很干净，代价是隐式依赖——
            //      若 Himage 为 null（没取图/没选图），方法内 GetImageSize 会抛异常，调用前必须先判空。
            //      影响范围：133 行先 DisplayImage() 把帧转成 Himage，紧接着才识别，顺序不能颠倒；
            //      But_HalconRun_Click 里有判空保护，而本处没有（见问题清单）。】
            #endregion

        }
        #region 提示：流程
        //显示照片
        // [提示：]【流程】释放旧图 → 用 HalconHelp 把相机帧转成 HObject → 取尺寸 → SetPart → 显示。
        // [提示：]【为什么先 Dispose】HALCON 图像是托管句柄包着非托管内存，不 Dispose 就是内存泄漏。
        // [提示：]【缺陷】help.frameForSave 未判空：相机还没出帧时它是 null，这里直接取 .Image 会崩。
        // [提示：]【缺陷】读 frameForSave 时没有加与写入端同一把锁（saveImageLock），极端情况下会读到正在被替换的帧。
        #endregion
        public void DisplayImage()                                              // 相机帧转 HALCON 图并显示
        {
            //先判断是否有照片
            if (Himage != null)
            {
                Himage.Dispose();
                Camera_Logs("显示照片：不为空！");

            }
            if (!HalconHelp.Instance.TryCreateHalconImage(help.frameForSave.Image, out Himage))
            {
                this.ShowWarningTip("转换图片失败！");
                Camera_Logs("转换图片失败！");
                return;
            }
            HOperatorSet.GetImageSize(Himage, out HTuple hv_Width, out HTuple hv_Height);
            HOperatorSet.SetPart(hv_WindowHandle, 0, 0, hv_Height, hv_Width);
            #region 问题：GetImageSize 得到的是 (宽, 高)，为什么 SetPart 里写成 (0, 0, 高, 宽)？
            // 【答：因为两者参数语义不同：
            //      GetImageSize 返回 Width, Height（列数、行数）；
            //      SetPart(Window, Row1, Column1, Row2, Column2) 要的是“显示区域左上角与右下角的坐标”，
            //      是（行, 列）顺序，右下角 = (Height, Width) → 所以第 3、4 个实参要写成 高、宽。
            //      写成 (0, 0, 宽, 高) 不会报错，但图像长宽比被拉反，画面被压扁/拉长。
            //      影响范围：本文件 106、155、223 三处 SetPart 都是同一写法，属固定约定，改一处就要三处一起改。】
            #endregion
            HOperatorSet.DispObj(Himage, hv_WindowHandle);
        }

        // [提示：]【业务】“识别”按钮只对已加载的图像生效：Himage 为空说明既没选图也没取图，先提示再返回。
        private void But_HalconRun_Click(object sender, EventArgs e)            // 识别按钮：对已加载的图像再识别
        {

            if (Himage == null)
            {
                this.ShowWarningTip("请先选择图片！");
                Camera_Logs("HalconRun失败！未选择图片");
                return;
            }
            HalconOCR();
        }

        // [提示：]【流程】判相机 → StopCamera → 按成功/失败分别提示并记日志。
        // [提示：]【缺陷】StopCamera 内部不会把 isGrabbing 置 false，取流线程会继续空转（详见 HkServerHelp.StopCamera）。
        private void But_CloseGet_Click(object sender, EventArgs e)             // 关闭相机：StopCamera 并按结果提示
        {
            //关闭相机
            if (help.device == null)
            {
                this.ShowWarningTip("相机未打开！");
                Camera_Logs("关闭相机失败：相机未打开！");
                return;
            }
            APIResult<bool> result = help.StopCamera();
            if (result.IsSuccess)
            {
                this.ShowSuccessTip("相机已关闭！");
                Camera_Logs("相机已关闭！");
            }
            else
            {
                this.ShowWarningTip(result.Message);
                Camera_Logs("关闭相机失败：" + result.Message);
            }

        }

        // [提示：]【时机】窗体关闭是清理相机资源的最后机会：不停流、不 Close，相机会一直被占用，下次打开就失败。
        // [提示：]【缺陷】这里只调了 StopCamera，没有调 SDKSystem.Finalize()，也没有 Dispose help 对象：
        // [提示：]        SDK 的进程级资源要等终结器或进程退出才释放。
        private void CameraManagement_FormClosing(object sender, FormClosingEventArgs e)  // 关窗收尾：先停轮询，再关相机
        {
            #region 点拨：窗体关闭是清理资源的最后时机
            //释放相机资源
            //关闭相机
            //【点拨】窗体关闭是清理资源的最后时机：不停采集/不Close，相机连接会一直被占着，
            //       下次打开可能失败，后台抓图线程也会一直空转。这一招补上了资源泄漏隐患。
            // [优化：]【清理顺序】先停 PLC 轮询线程（它会读 PLC、还会往界面写日志），再停相机采集、最后断 PLC。
            // [优化：]        原代码只停相机：PLC 那条线程即使不在 UI 线程里也根本没退出机制，进程一直留着连接。
            // [优化：]【怎么验证】关掉相机页后用 netstat -ano 看该进程已不再持有 502 端口连接；任务管理器里进程能正常退出。
            #endregion
            StopPlcMonitor();
            if (help.device == null)
            {
                return;
            }
            help.StopCamera();
        }

        #region 点拨：刷新表格必须重新赋一次 DataSource
        //日志输出Camera_Log
        //【点拨】刷新表格必须重新赋一次 DataSource：只往 List 里 Add 界面不会自己刷新；
        //       先 =null 再 =Loglist 是标准的"强制重绑"套路（断开旧绑定再挂新数据）。
        // [提示：]【刷新套路】List 内容变了 DataGridView 不会自动感知，必须重新赋一次 DataSource：
        // [提示：]        先置 null 断开旧绑定，再赋同一个 List → 触发重新绑定（WinForms 的常规做法）。
        // [提示：]【缺陷】Loglist 只 Add 不清，长时间运行界面会越用越卡、内存越占越多（见问题清单）。
        // [提示：]【调用点】打开/关闭相机、选图、识别结果、各类失败提示，共十余处。
        #endregion
        public void Camera_Logs(string log)                                     // 往界面日志表格加一行
        {
            Loglist.Add(new CameraHelp() { MakeTime = DateTime.Now, MakeLog = log });
            Camera_Log.DataSource = null;
            Camera_Log.DataSource = Loglist;
        }
        #region 提示：职责
        //Halcon识别方法
        // [提示：]【职责】把“显示 + 识别 + 记日志”打包成一个方法，取图按钮与识别按钮共用，避免两处逻辑分叉。
        // [提示：]【缺陷】GLOSSED.TiaoMa 内部异常会继续上抛（它 catch 后 throw），本方法没有 try/catch，
        // [提示：]        异常会一路抛到按钮 Click，成为未处理异常 → 程序崩溃。
        // [提示：]【缺陷】Himage 为 null 时 GetImageSize 直接抛异常（But_GetPhoto_Click 那边没有判空）。
        // [提示：]【遗留】226~232 行是注释掉的历史调用，含旧版 GLO_OCR 与参数极多的旧 TiaoMa：留着当对照，不影响编译。
        #endregion
        private int HalconOCR()                                                 // 识别当前图：返回认出的一维码个数（0=这张不能用）
        {
            // 获取图片宽高
            HOperatorSet.GetImageSize(Himage, out HTuple hv_Width, out HTuple hv_Height);
            HOperatorSet.SetPart(hv_WindowHandle, 0, 0, hv_Height, hv_Width);
            HOperatorSet.DispObj(Himage, hv_WindowHandle);

            //GLOSSED num = new GLOSSED();
            //num.GLO_OCR(Himage, out HTuple hv_Class);
            //HOperatorSet.DispText(hv_WindowHandle, "识别结果: " + hv_Class,"window", 12, 12, "red", "box", "false");//旧版

            GLOSSED num = new GLOSSED();
            //num.GLO_OCR(Himage, hv_WindowHandle, out HTuple hv_Class);
            //num.TiaoMa(Himage, out HObject ho_SymbolRegions, out HObject ho_OneCodeRegion,hv_WindowHandle, hv_CodeTypes, out HTuple hv_DecodedDataStrings,out HTuple hv_BarCodeHandle, out HTuple hv_Exception, out HTuple hv_CodeCount,out HTuple hv_WindowRow, out HTuple hv_WindowColumn, out HTuple hv_WindowWidth,out HTuple hv_WindowHeight, out HTuple hv_Index, out HTuple hv_Row1, out HTuple hv_Column1,out HTuple hv_Row2, out HTuple hv_Column2, out HTuple hv_Label, out HTuple hv_CodeWindowRow,out HTuple hv_CodeWindowColumn, out HTuple hv_Ascent, out HTuple hv_Descent,out HTuple hv_TextWidth, out HTuple hv_TextHeight, out HTuple hv_TextRow, out HTuple hv_TextColumn);
            num.TiaoMa(Himage, hv_CodeTypes, hv_WindowHandle, out HTuple hv_CodeCount);
            Camera_Logs("识别结果：" + hv_CodeCount.ToString() + "个一维码");
            // [优化：]【为什么要有返回值】连拍循环靠它判断"这一张成了没有"；原来这个方法只写日志、不给信号，
            // [优化：]        所以外面只能靠猜时刻。hv_CodeCount 是 int 元组，取 .I 就是个数。
            return hv_CodeCount.I;
        }

        #region 说明：软件"按一下绿钮"要做哪两件事
        // 【问：点一下软件按钮，能不能等于按一次物理绿钮，让伺服转一圈？】
        // 【答：能，但必须两边各做一步，缺一不可：
        //      ① PLC 端（一次性，改完下载才生效）：在 Network 8 的 `LD I0.0` 下面并联一行 `O V2000.0`，
        //         让"软件位"和物理按钮一样能锁住 M0.0。为什么必须动 PLC：M0.0 只有 I0.0 和它自己两个来源，
        //         而 I0.0 是输入映像区，Modbus 只能读（1xxxx）写不进去；走"使能+启动"那条路又会被
        //         `AN V20.1`（使能一亮就不许再按）和 Network 12（使能一亮就 MOVW +0 清整字）清掉。
        //      ② C# 端：向 40001 写 256（V2000.0=1）→ 300ms 后写 0（松手）。M0.0 会自己锁住，之后照常走完
        //         0.3s 起步、3s 转动、T104 收尾这一整套，和我们按物理钮得到的效果一致。
        // 【为什么选 V2000.0 这个位】VW2000 的低字节 VB2001 已经被 V2001.0~V2001.7 占满，
        //      高字节 VB2000 在导出的程序里没有任何一处引用，正好空着；而且 Network 11 收尾时
        //      `MOVW +0, VW2000` 会顺手把它清掉，等于自带"按一次只跑一轮"的保护。】
        #endregion
        private void Run_Take_Click(object sender, EventArgs e)                  // 软件模拟按一次绿钮：下发 300ms 脉冲给 PLC
        {
            if (!plcHelp.IsPlc_Connect)
            {
                this.ShowWarningTip("PLC未连接！");
                Camera_Logs("软启动失败：PLC未连接");
                return;
            }
            softStartRequest = true;
            Camera_Logs("软启动：已请求按下绿钮，等待轮询线程下发");
        }
    }
}
