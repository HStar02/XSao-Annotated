using MvCameraControl;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Windows.Forms;

namespace XSao.Help
{
    #region 提示：类级说明
    // [提示：]【类级说明】HkServerHelp＝海康 MVS 相机 SDK 的一层薄封装：枚举 → 打开 → 取流 → 停流 → 释放。
    // [提示：]        它自己不开窗、不画图，只把每一帧通过 showImageCallback 交出去，由界面负责显示
    // [提示：]        （CameraManagement 构造函数里 help.showImageCallback = showImageCallBack 完成注册）。
    // [提示：]        生命周期：构造 SDKSystem.Initialize() → 析构 ~HkServerHelp() 里 SDKSystem.Finalize()。
    // [提示：]【使用约束】一个实例对应一台相机；多相机要多个实例，但 SDK 的 Initialize/Finalize 是进程级开关，
    // [提示：]            多个实例反复 Initialize/Finalize 会互相干扰（见问题清单）。
    #endregion
    public class HkServerHelp
    {
        #region 提示：字段分组
        //创建设备列表
        // [提示：]【字段分组】前 5 个字段描述一台相机的会话状态：
        // [提示：]            deviceInfoList＝枚举结果，只用于“挑设备、回填序列号”，不参与采集；
        // [提示：]            device＝真正打开的设备句柄，只有 Open 成功后才非空；
        // [提示：]            isGrabbing＝采集线程的开关标志，就是后台 while(isGrabbing) 的退出条件；
        // [提示：]            receiveThread＝取流线程；frameForSave＝最近一帧的克隆副本，供界面随时取图。
        #endregion
        List<IDeviceInfo> deviceInfoList = new List<IDeviceInfo>();
        //IDevice device是设备控制类
        public IDevice device;
        //isGrabbing是开始采集的状态，isGrabbing为true时，表示正在采集，为false时，表示未采集
        private bool isGrabbing;
        //定义开始采集的线程
        private Thread receiveThread;
        //保存的帧图片，也可以显示该帧图片
        public IFrameOut frameForSave;

        //定义保存图片的锁---应该是只读，不允许修改
        private readonly static object saveImageLock = new object();
        #region 问题：private readonly static object saveImageLock = new object(); 这一句到底是什么，为什么要这么写？
        // 【答：它是一个“只当锁用、不装任何业务数据”的空对象，作用是给线程划一条互斥边界：
        //      ①object 类型本身没有业务含义，new object() 只是拿它的“身份”当钥匙；
        //      ②lock(saveImageLock){...} 里的代码同一时刻只允许一个线程进入，这段叫临界区；
        //      ③static：锁属于类而不属于实例，保证即使 new 了多个 HkServerHelp 也共用同一把锁；
        //      ④readonly：引用不能再被换掉，防止有人中途 saveImageLock = new object() 把锁换走，互斥直接失效。
        //      本类里它保护的是 frameForSave 的赋值：后台取流线程写、界面线程读，不加锁可能读到“写了一半”的脏数据。
        //      依据：C# 的 lock 语句等价于 Monitor.Enter/Exit，锁对象必须是引用类型，且被所有相关线程共享同一个。
        //      影响范围：只覆盖 ReceiveThreadProcess 的写入段；读取方 CameraManagement.DisplayImage 读 frameForSave.Image 时没有加同一把锁，所以目前只保护了一半（见问题清单）。】
        #endregion

        // [提示：]【调用链】new HkServerHelp() → SDKSystem.Initialize()（加载 SDK 内部资源/驱动）→ 之后才能 EnumDevices。
        // [提示：]        构造里不做枚举，是为了“先建后用”：CameraManagement 构造时就 new 出对象，
        // [提示：]        但真正枚举设备发生在用户点“打开相机”的时候。
        public HkServerHelp()                                    // 构造：SDKSystem.Initialize()
        {
            SDKSystem.Initialize();
        }
        //析构函数
        ~HkServerHelp()
        {
            SDKSystem.Finalize();
        }
        #region 问题：SDKSystem.Initialize() 放在构造函数、SDKSystem.Finalize() 放在析构函数，各自起什么作用？
        // 【答：这对方法必须成对出现，效果相当于“开灯 / 关灯”：
        //      ①Initialize()：把相机 SDK 的运行时资源准备好（加载驱动、初始化内部句柄表）。没调它就去
        //         EnumDevices/Open，SDK 会直接返回错误码；所以放在构造函数里，保证“拿到对象就能用”；
        //      ②Finalize()：释放 SDK 占用的进程级资源，原则上与 Initialize 一一对应，放在析构函数 ~HkServerHelp() 里；
        //      ③“一个新建一个死亡”说的就是这对：Initialize 让 SDK 活起来，Finalize 让它退场。
        //      【风险】C# 析构函数的执行时机由 GC 决定、完全不可预期，而且跑在 GC 的终结器线程上：
        //        程序退出前若 GC 还没回收该对象，Finalize() 就不会执行，SDK 资源要等进程结束才释放；
        //        更稳妥的做法是在窗体 FormClosing 里显式调一次（本项目在 CameraManagement_FormClosing 只停了流，没调它）。
        //      依据：海康 MVS SDK 要求 Initialize 与 Finalize 成对调用；.NET 终结器不保证及时性和线程。】
        #endregion

        /// <summary>
        /// 显示图片的回调的函数
        /// </summary>
        public Action<IDevice, IFrameOut> showImageCallback;

        //定义只读集合
        readonly DeviceTLayerType enumTLayerType = DeviceTLayerType.MvGigEDevice
            | DeviceTLayerType.MvUsbDevice | DeviceTLayerType.MvGenTLGigEDevice
            | DeviceTLayerType.MvGenTLCXPDevice | DeviceTLayerType.MvGenTLCameraLinkDevice
            | DeviceTLayerType.MvGenTLXoFDevice;

        #region 提示：上下游
        //获取所有设备列表
        // [提示：]【上下游】上游：CameraManagement.But_OpenCam_Click 第一句就调它；
        // [提示：]            下游：OpenDevice 用这里返回的序列号去枚举列表里找设备。
        // [提示：]【边界】两种情况都返回 Error：①EnumDevices 返回非 MV_OK；②枚举成功但列表为空——
        // [提示：]            空列表不能当成“成功但没设备”，否则调用方拿到空 list 还要自己判，容易漏判。
        #endregion
        public APIResult<List<string>> GetDevice()               // 枚举相机，返回序列号列表
        {
            List<string> list = new List<string>();
            //去查询所有相机设备：enumTLayerType是相机类型，deviceInfoList是相机基本信息列表
            int nRet = DeviceEnumerator.EnumDevices(enumTLayerType, out deviceInfoList);
            if (nRet != MvError.MV_OK)
            {
                return APIResult<List<string>>.Error("获取设备列表失败！");
            }

            // ch:在窗体列表中显示设备名 | en:Display device name in the form list
            for (int i = 0; i < deviceInfoList.Count; i++)
            {
                IDeviceInfo deviceInfo = deviceInfoList[i];
                // ch:获取设备序列号 | en:Get device serial number
                //只添加设备序列号
                list.Add(deviceInfo.SerialNumber);
                #region 问题：deviceInfo.SerialNumber 里的 SerialNumber 是哪来的，为什么它就是序列号？
                // 【答：它不是本程序生成的、也不是 C# 自带的，而是海康 SDK 在枚举设备时从相机固件读出的出厂唯一编号：
                //      ①EnumDevices(enumTLayerType, out deviceInfoList) 会把每台设备的描述填进 IDeviceInfo 对象；
                //      ②IDeviceInfo 是 SDK 定义的接口，SerialNumber 是它的一个属性（相机里存好的字符串）；
                //      ③出厂唯一，所以用它挑设备比用 IP 或枚举序号更可靠：IP 会变、枚举顺序也会变。
                //      依据：MvCameraControl 的 IDeviceInfo 接口成员；DeviceFactory.CreateDevice(deviceInfo) 也要靠它定位相机。
                //      影响范围：GetDevice 只把序列号放进下拉列表，OpenDevice 再用序列号 Find 回设备信息，两处必须用同一个字段。】
                #endregion
            }

            // ch:选择第一项 | en:Select the first item
            if (list.Count == 0)
            {
                return APIResult<List<string>>.Error("没有发现设备！");
            }

            return APIResult<List<string>>.Success(list);
        }

        #region 提示：流程
        //获取完成设备列表后，开始打开设备
        // [提示：]【流程】序列号校验 → 在枚举列表里 Find 出设备信息 → CreateDevice → Open →
        // [提示：]            若是 GigE 相机再探测最佳包大小 → 最后设“连续采集 + 关触发”。
        // [提示：]【业务背景】“Continuous + TriggerMode=Off”是自由运行模式：相机自己按帧率出图，不等外部信号。
        // [提示：]            将来若要“光电开关到位才拍一张”，必须把 TriggerMode 改成 On 并配触发源，否则会一直连拍。
        #endregion
        public APIResult<bool> OpenDevice(string serialNumber)   // 按序列号打开相机
        {
            //先检测设备列表是否为空
            if (serialNumber == null)
            {
                return APIResult<bool>.Error("请先获取设备列表！");
            }
            if (deviceInfoList.Count == 0)
            {
                return APIResult<bool>.Error("没有发现设备，或者枚举设备失败！");
            }
            #region 问题：OpenDevice 里为什么先判 serialNumber == null，再判 deviceInfoList.Count == 0？
            // 【答：这是两个不同失败场景的防呆，分开判给出的提示更准确，排查更快：
            //      ①serialNumber == null：调用方压根没传序列号（例如没先调 GetDevice 就点了“打开”）→ 提示“请先获取设备列表”；
            //      ②deviceInfoList.Count == 0：传了序列号，但枚举结果为空（没插相机 / 被别的程序占用 / 驱动没装）→ 提示“没有发现设备”；
            //      ③两个条件都过了，才用 Find 去列表里找设备；否则 Find 会拿到 null，CreateDevice(null) 直接抛异常。
            //      依据：先校验参数、再校验状态、最后才动 SDK，是封装 SDK 时的通用顺序。
            //      影响范围：只影响错误提示文案；把两个条件合并会让“没传参数”和“没找到相机”混为一谈，现场排查变慢。】
            #endregion
            IDeviceInfo deviceInfo = deviceInfoList.Find(x => x.SerialNumber == serialNumber);
            try
            {
                // ch:打开设备 | en:Open device
                device = DeviceFactory.CreateDevice(deviceInfo);
            }
            catch (Exception)
            {
                return APIResult<bool>.Error("创建设备失败！");
            }

            int result = device.Open();
            if (result != MvError.MV_OK)
            {
                return APIResult<bool>.Error("打开设备失败！");
            }

            //ch: 判断是否为gige设备 | en: Determine whether it is a GigE device
            if (device is IGigEDevice)
            {
                //ch: 转换为gigE设备 | en: Convert to Gige device
                IGigEDevice gigEDevice = device as IGigEDevice;

                // ch:探测网络最佳包大小(只对GigE相机有效) | en:Detection network optimal package size(It only works for the GigE camera)
                int optionPacketSize;
                result = gigEDevice.GetOptimalPacketSize(out optionPacketSize);
                // [提示：]【缺陷提示】紧接其后的两处 APIResult<bool>.Error(...) 只“造了一个错误对象就丢掉”，
                // [提示：]            既没 return、也没赋给任何变量 → 包大小设置失败时界面完全无感知，属无效代码（见问题清单）。
                // [提示：]            GigE 包大小设不对的典型症状是：出图丢行、花屏、帧率上不去。
                if (result != MvError.MV_OK)
                {
                    APIResult<bool>.Error("警告：获取数据包大小失败！");
                }
                else
                {
                    result = device.Parameters.SetIntValue("GevSCPSPacketSize", (long)optionPacketSize);
                    if (result != MvError.MV_OK)
                    {
                        APIResult<bool>.Error("警告：获取数据包大小失败！");
                    }
                }
            }
            // ch:设置采集连续模式 | en:Set Continues Aquisition Mode
            device.Parameters.SetEnumValueByString("AcquisitionMode", "Continuous");
            device.Parameters.SetEnumValueByString("TriggerMode", "Off");

            return APIResult<bool>.Success("连接成功");
        }

        #region 提示：线程模型
        //打开设备后，开始采集数据
        // [提示：]【线程模型】先置 isGrabbing=true → 起 receiveThread 取流 → 再调 StartGrabbing 让相机开始吐帧。
        // [提示：]            顺序不能反：若先让相机开始出图再起线程，SDK 缓冲会被瞬间填满而丢帧。
        // [提示：]【异常路径】起线程失败时 MessageBox 之后 throw：异常会一路抛到按钮事件，最终成为未处理异常（见清单）。
        #endregion
        public APIResult<bool> StartGrabbing()                   // 起取流线程并让相机开始吐帧
        {
            if (!device.IsConnected)
            {
                return APIResult<bool>.Error("请先连接相机！");
            }
            try
            {
                // ch:标志位置位true | en:Set position bit true
                isGrabbing = true;

                receiveThread = new Thread(ReceiveThreadProcess);
                receiveThread.Start();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Start thread failed!, " + ex.Message);
                // [提示：]【文案缺陷】这行的引号位置写错了：逗号被包进了字符串里，
                // [提示：]            实际弹出来是  Start thread failed!, <异常信息>（逗号贴着感叹号）。只影响显示，不影响逻辑。
                throw;
            }

            // ch:开始采集 | en:Start Grabbing
            int result = device.StreamGrabber.StartGrabbing();
            if (result != MvError.MV_OK)
            {
                isGrabbing = false;
                receiveThread.Join();
                return APIResult<bool>.Error("开始抓取失败！");
            }
            return APIResult<bool>.Success();
        }

        #region 提示：核心循环
        //线程中实时采集图像数据
        // [提示：]【核心循环】取流线程主体：while(isGrabbing) 反复 GetImageBuffer(超时 1000ms)。
        // [提示：]            取到帧 → 加锁克隆一份给界面 → 回调通知界面显示 → FreeImageBuffer 把缓冲还给 SDK。
        // [提示：]【关键点】必须 FreeImageBuffer：SDK 的帧缓冲数量有限（通常几个），不归还几帧后就会彻底卡死。
        // [提示：]【关键点】保存的是 Clone 而不是 frameOut 本身：frameOut 归还后内存会被 SDK 复用，
        // [提示：]            只存引用的话，界面读到的会变成下一帧的内容（画面“跳帧/错位”的经典原因）。
        // [提示：]【运行位置】本方法跑在 receiveThread 上，不在 UI 线程，所以里面绝不能直接碰控件。
        #endregion
        private void ReceiveThreadProcess()                      // 取流线程：反复取帧交回调
        {
            int nRet;

            while (isGrabbing)
            {
                IFrameOut frameOut;

                nRet = device.StreamGrabber.GetImageBuffer(1000, out frameOut);
                if (MvError.MV_OK == nRet)
                {

                    lock (saveImageLock)
                    {
                        try
                        {
                            frameForSave = frameOut.Clone() as IFrameOut;
                            // [提示：]【内存】每帧 Clone 一次都要新分配内存，而旧的 frameForSave 从未 Dispose → 持续泄漏（见问题清单）。
                            // [提示：]        正确做法：赋值前先 if (frameForSave != null) frameForSave.Dispose()，或用双缓冲轮换。
                        }
                        catch (Exception e)
                        {
                            MessageBox.Show("IFrameOut.Clone failed, " + e.Message);
                            return;
                        }
                    }

                    showImageCallback(device, frameOut);
                    #region 提示：风险
                    // [提示：]【风险】这里没判 null 就直接调：若界面没注册回调（或已被置 null），抛 NullReferenceException，
                    // [提示：]        而异常发生在后台线程，WinForms 拦不住，整个进程直接退出。
                    // [提示：]【参数】第一个参数给的是 device（界面要用它调 ImageRender 渲染），
                    // [提示：]            第二个是本次真实帧；界面里再自己决定是否显示，属于“数据+上下文一起给”的回调设计。
                    // 【问：为什么在这个位置（取流线程里）调用 showImageCallback，而不是让界面自己去取图？】
                    // 【答：这是回调（观察者）模式的典型用法，用来解开“数据从哪来”和“数据怎么显示”的耦合：
                    //      ①帧是 SDK 在后台线程一帧帧吐出来的，只有这里才知道“新图到了”，界面无从轮询；
                    //      ②本类故意不持有任何控件，只把 (device, frameOut) 交给注册进来的方法；
                    //      ③界面在构造时 help.showImageCallback = showImageCallBack 注册，显示逻辑就留在 CameraManagement；
                    //         将来换界面只需重新注册，SDK 封装层一行都不用改。
                    //      【风险】此处未判 null：忘记注册就在后台线程抛异常，进程会直接崩（见问题清单）。
                    //      依据：Action<IDevice, IFrameOut> 是委托字段，未注册时默认值为 null。】
                    //        好处是SDK类不依赖具体界面，换个界面重新注册回调即可。
                    #endregion

                    //device.ImageRender.DisplayOneFrame(pictureBox1.Handle, frameOut.Image);
                    //释放帧图像
                    device.StreamGrabber.FreeImageBuffer(frameOut);
                }
                else
                {
                    Thread.Sleep(5);
                }
            }
        }

        #region 优化：新增
        // [优化：]【新增】给业务层取"当前最新一帧"，和写方用同一把锁，避免读到只写了一半的状态。
        // [优化：]【为什么不 Clone 一份再返回】Clone 要新分配一整块图像内存，触发频繁时开销大且容易漏 Dispose。
        // [优化：]        这里直接返回引用：旧帧被新帧替换时不会被释放（本类从不 Dispose 旧帧），
        // [优化：]        所以调用方拿到引用后立即转 HALCON 图像是安全的（GenImage1 是像素拷贝，拷完就不再依赖原帧）。
        // [优化：]【约定】调用方只读、不要 Dispose——帧的生命周期归本类管。
        // [优化：]【调用点】CameraManagement.OnPlcTrigger（PLC 触发拍照时取图）。
        #endregion
        public bool TryGetLatestFrame(out IFrameOut frame)       // 取最近一帧（与写方同一把锁）
        {
            lock (saveImageLock)
            {
                frame = frameForSave;
            }
            return frame != null;
        }

        #region 提示：收尾流程
        //停止采集
        // [提示：]【收尾流程】判连接 → 判是否在采集 → StopGrabbing → Close → Dispose。
        // [提示：]【严重缺陷】本方法没有把 isGrabbing 置回 false：后台线程 while(isGrabbing) 仍为真，
        // [提示：]            会在已停止、甚至已 Dispose 的设备上无限 GetImageBuffer（返回错误 → Sleep 5ms → 再试），
        // [提示：]            结果是线程空转、CPU 白烧，设备释放后还可能抛 ObjectDisposedException（见问题清单，高优先级）。
        // [提示：]【边界】device == null 时，本方法第一行 device.IsConnected 就会抛 NullReferenceException，
        // [提示：]            所以 240 行的 if (device != null) 实际上永远轮不到 device 为 null 的情况。
        // [提示：]【调用点】CameraManagement.But_CloseGet_Click（用户点关闭）与 CameraManagement_FormClosing（关窗兜底）。
        #endregion
        public APIResult<bool> StopCamera()                      // 停流→关闭→释放设备
        {
            if (!device.IsConnected)
            {
                return APIResult<bool>.Error("相机未连接,请先打开设备！");
            }
            if (!isGrabbing)
            {
                return APIResult<bool>.Error("相机未开始采集！");
            }

            int result = device.StreamGrabber.StopGrabbing();

            if (result != MvError.MV_OK)
            // 【问：MvError.MV_OK 是哪来的，是 C# 自带的还是 SDK 的？】
            // 【答：是海康 SDK 自带的枚举常量，不是 .NET 的东西：
            //      ①它定义在 MvCameraControl 命名空间（本文件第 1 行 using），MV_OK = 0 表示操作成功；
            //      ②SDK 绝大多数方法（EnumDevices / Open / StartGrabbing / StopGrabbing / GetImageBuffer / SetIntValue）
            //         都返回 int 错误码，统一用它与 MV_OK 比较来判断成败；
            //      ③非 0 即错误码，可查 SDK 手册定位（常见：无权限、设备被占用、超时）。
            //      依据：本文件 59、111、125、132、168、187、233 行全是这种“int 返回值 vs MV_OK”的写法。
            //      影响范围：判断方式写错（例如用 > 0 判成功）会把一部分错误码当成成功而静默放行。】
            //        所有SDK方法都返回它来判断成败。不是C#自带的，是SDK提供的。
            {
                return APIResult<bool>.Error("停止采集失败！");
            }
            if (device != null)
            {
                device.Close();
                device.Dispose();
                #region 问题：为什么先 Close() 再 Dispose()？能不能只调 Dispose()？
                // 【答：两者管的不是同一件事，顺序反了会留下“占着硬件、却没有对象可控”的状态：
                //      ①Close()：断开与相机的连接、结束底层会话，相当于硬件层面“放手”；
                //      ②Dispose()：销毁托管对象、释放其内存与内部句柄，相当于对象层面“销毁”；
                //      ③只 Dispose 不 Close：SDK 的连接资源可能仍被占用，下次 Open 同一台相机会报“设备被占用”；
                //      ④先 Close 再 Dispose，才是“先停止工作、再销毁对象”的标准收尾顺序。
                //      依据：IDevice 的生命周期约定 Close → Dispose；本处是相机唯一的释放点（窗体关闭时也会走到）。
                //      影响范围：顺序写错会导致重开相机失败，必须重启程序才能恢复。】
                //        先关后销是标准顺序：只Dispose不Close，硬件可能还占着连接资源。
                #endregion
            }
            return APIResult<bool>.Success();
        }
    }
}
