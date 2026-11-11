using Sunny.UI;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using XSao.Entity;
using XSao.Help;
using XSao.VO;

namespace XSao
{
    // [提示：]【类级说明】新版主界面：顶部导航（UIHeaderButton）+ 中间容器 Panel_HomePage，所有业务页都塞进这个容器里。
    // [提示：]        与旧版 HomePage 的区别：旧版每个页面都用 ShowDialog 弹独立窗口，新版是“单容器多页面”切换。
    public partial class New_HomePage : UIForm
    {
        public UserEntity User = new UserEntity();

        //页面缓存
        // [提示：]【作用】页面缓存：同一个页面只 new 一次，切回来直接复用，避免重复查库、重复建相机对象。
        // [提示：]【失效条件】页面被 Dispose（例如用户关过它）时缓存不可用 → 靠 page.IsDisposed 判断后重建。
        public Dictionary<string, UIForm> pageDict = new Dictionary<string, UIForm>();

        // [提示：]【新增·布局适配】记录每个页面在“设计器里的原始尺寸”：子窗体一旦 Dock=Fill，Size 就被面板改写，
        // [提示：]        所以必须在改 Dock 之前把原始尺寸存下来，后面才能判断“面板够不够大、要不要压缩页面”。
        private readonly Dictionary<string, Size> pageDesignSize = new Dictionary<string, Size>();

        // [提示：]【新增·布局适配】滚动宿主：Panel_HomePage（SunnyUI 的 UIPanel）的 AutoScroll 实测不生效，
        // [提示：]        所以在其内垫一层标准 Panel 承载页面，页面比视口大时由它出滚动条。
        // [提示：]        注意：标准 Panel 一定会有滚动条（已实测 VerticalScroll.Visible=True）。
        private Panel pageScroller;


        //当前页面名称
        private string currentPageName = "";



        // [提示：]【启动即查库】构造里读 ini 拿账号密码，然后立刻 GoPage(Output)，而 GoPage 内部会联表查权限：
        // [提示：]            数据库没开/连不上时，主界面在构造阶段就抛异常 → 程序起不来（见问题清单）。
        public New_HomePage()                                                 // 构造：读 ini 账号、默认进产量页
        {
            InitializeComponent();
            // [提示：]【新增·布局适配】窗口放大/还原会改变面板尺寸；不重新算一遍，页面会一直沿用切页当时的判断结果。
            Panel_HomePage.SizeChanged += Panel_HomePage_SizeChanged;
            //查询保存的用户信息
            IniFile ini = new IniFile(LoginForm.userInfoFile);
            User.UserName = ini.Read("UserInfo", "UesrName", "");
            User.PassWord = ini.Read("UserInfo", "Password", "");
            //跳转产量界面
            GoPage(nameof(Output));
        }

        // [提示：]【主流程】页面名白名单校验 → 判断是否已在当前页 → 查用户权限 → 校验该页权限 → 取/建页面 → 挂到容器。
        // [提示：]【为什么白名单】pageName 来自按钮 Tag，属外部输入；不白名单过滤就等于允许任意字符串进 switch。
        // [提示：]【体验缺陷】checkAuthority 返回 false 时静默 return：用户点了没反应，也没有任何提示。
        private void GoPage(string pageName)                                  // 切页：白名单→权限→取页面→挂容器
        {

            if (string.IsNullOrEmpty(pageName))
            {
                UIMessageTip.ShowWarning("未配置跳转页面！");
            }
            switch (pageName)
            {
                case nameof(Output):
                case nameof(DateLog):
                case nameof(UserFormCenter):
                case nameof(CameraManagement):
                    break;
                default:
                    UIMessageTip.ShowWarning("未配置跳转页面！");
                    return;

            }
            #region 提示：缺陷
            //跳转页面
            // [提示：]【缺陷】上面 if (string.IsNullOrEmpty(pageName)) 分支只弹了提示、没有 return：
            // [提示：]            流程会继续往下走 switch，再弹一次“未配置跳转页面”，出现连续两个提示框。
            //判断当前页面是否为空
            #endregion
            if (currentPageName == pageName)
            {
                return;
            }

            UIForm page;

            try
            {
                UserVo vo = GetUserAndAuthority();
                if (vo == null)
                {
                    this.ShowWarningTip("权限异常！");
                    return;
                }
                if (!checkAuthority(vo, pageName))
                {
                    return;
                }
                //页面跳转
                page = GetOrCreatePage(pageName);
            }
            catch (Exception)
            {
                UIMessageTip.ShowWarning("未找到页面！");
                return;
            }
            //暂停容器布局
            Panel_HomePage.SuspendLayout();
            try
            {
                //移除容器内的所有控件
                Panel_HomePage.Controls.Clear();
                // [提示：]【新增·布局适配】Clear 会把旧的滚动宿主一并移除，所以这里重建（并清空引用后重新挂）。
                pageScroller = null;
                EnsureScroller();
                //然后添加新的控件
                pageScroller.Controls.Add(page);
                page.Show();
                currentPageName = pageName;
                // [提示：]【新增·布局适配】缓存页面可能是按旧面板尺寸摆的，挂上来后按当前面板再算一次，避免沿用旧判断。
                FitPageToPanel(pageName, page);

            }
            catch (Exception)
            {
                throw;
            }
            finally { Panel_HomePage.ResumeLayout(true); }
        }

        #region 提示：职责
        // [提示：]【职责】按页面名取实例：缓存里有且没被释放 → 直接用；否则 new 一个并做“子窗体化”设置。
        // [提示：]【子窗体化四连】TopLevel=false 是能塞进 Panel 的前提；Dock=Fill 让它铺满容器；
        // [提示：]            ShowTitle/AllowShowTitle=false + Padding=0 + ZoomScaleDisabled=true 用来去掉 SunnyUI 自带标题栏的占位，
        // [提示：]            少任何一条都会出现“页面没铺满、上方留白或形状异常”（源码 137 行注释就是踩过这个坑）。
        #endregion
        private UIForm GetOrCreatePage(string pageName)                       // 取缓存页，没有就 new 并子窗体化
        {
            if (!pageDict.TryGetValue(pageName, out UIForm page) || page.IsDisposed)
            // 【问：为什么写成 pageDict.TryGetValue(...) || page.IsDisposed 会报异常，加上 ! 就正常？】
            // 【答：关键在 || 的短路求值，以及 ! 把真假值翻了过来：
            //      ①不加 !：左操作数是 TryGetValue 的返回值。字典里没有该页时它返回 false，
            //        于是 || 必须继续算右半边 page.IsDisposed；而这时 out page 已被赋成 null
            //        → null.IsDisposed 抛 NullReferenceException，这就是你看到的报错。
            //      ②加了 !：没有该页时 !false = true，true || 任意直接短路，右半边根本不执行 → 不会碰 null。
            //      语义也正好对：字典里没有(true) 或 页面已被释放(true)，两种情况都该重新 new 一个页面。
            //      依据：C# 逻辑或在左操作数为 true 时不再求值右操作数。
            //      验证方式：先点一个从未打开过的页面（字典为空）→ 不报错；关掉该页再点 → 走 IsDisposed 分支重建。】
            {
                switch (pageName)
                {
                    case nameof(Output):
                        page = new Output();
                        break;
                    case nameof(DateLog):
                        page = new DateLog();
                        break;
                    case nameof(UserFormCenter):
                        page = new UserFormCenter();
                        break;
                    case nameof(CameraManagement):
                        page = new CameraManagement();
                        break;
                    default:
                        throw new Exception("此页面暂未开发！");
                }
                //只有禁用状态的Top页面才能被添加到panel中
                // [提示：]【新增·布局适配】此时页面还是设计器尺寸（如 DateLog 的 1411×922），先记下来再改 Dock。
                pageDesignSize[pageName] = page.Size;
                page.TopLevel = false;

                //新增
                page.FormBorderStyle = FormBorderStyle.None;    // 新增：子窗体不画边框
                page.ShowTitle = false;                         // 新增：关掉 UIForm 自带标题栏
                page.AllowShowTitle = false;                    // 新增：连标题区高度都不占
                page.Padding = new Padding(0);                  // 新增：清掉为标题栏预留的 (2,36,2,2) 内边距
                page.ZoomScaleDisabled = true;                  // 新增：避免 SunnyUI 缩放二次拉伸
                //让页面充满panel
                // [提示：]【缺陷】无条件 Dock=Fill 就是“被压缩”的根源：面板只有 1420×700，而 DateLog/UserFormCenter/
                // [提示：]        CameraManagement 的设计尺寸是 1411×922，Dock=Fill 会把它们强行压到 700 高，
                // [提示：]        页面里的控件是绝对定位（没有 Anchor/TableLayout），一压就重叠或被裁，看起来“信息不全”。
                // [提示：]【改法】面板够大 → 继续 Fill 铺满；面板不够大 → 保持设计尺寸不改，交给面板滚动条显示（见 FitPageToPanel）。
                FitPageToPanel(pageName, page);
                //但是页面仍然没有充满整个Panel_HomePage，反而奇形怪状！
                //添加页面
                pageDict[pageName] = page;
            }
            //缓存命中，获取缓存的页面
            return page;
        }

        #region 提示：新增·布局适配
        // [提示：]【新增·布局适配】子窗体的两种放法，按“滚动宿主的可视区够不够大”二选一：
        // [提示：]  ①可视区 ≥ 页面设计尺寸：Dock=Fill 铺满（变大只是留白，不会裁内容，与原观感一致）；
        // [提示：]  ②可视区 < 页面设计尺寸：Dock=None + 恢复设计尺寸 + 宿主出滚动条，页面一个像素都不缩。
        // [提示：]  为什么不直接缩放页面（如按比例 Zoom）：页面内部控件全是绝对定位、字体会跟着缩，
        // [提示：]        缩完字号变小、边距错位，比滚动更难读，所以只滚动不缩放。
        #endregion
        private void FitPageToPanel(string pageName, UIForm page)             // 按可视区决定铺满还是出滚动条
        {
            Size design = pageDesignSize.ContainsKey(pageName) ? pageDesignSize[pageName] : page.Size;
            Size view = (pageScroller != null && !pageScroller.IsDisposed) ? pageScroller.ClientSize : Panel_HomePage.ClientSize;

            if (view.Width >= design.Width && view.Height >= design.Height)
            {
                // 装得下：铺满，行为与原来一致
                if (pageScroller != null)
                {
                    pageScroller.AutoScroll = false;
                }
                page.Dock = DockStyle.Fill;
            }
            else
            {
                // 装不下：保持设计尺寸，靠滚动条，绝不压缩页面内部控件
                if (pageScroller != null)
                {
                    pageScroller.AutoScroll = true;
                }
                page.Dock = DockStyle.None;
                page.Size = design;
                page.Location = new Point(0, 0);
            }
        }

        // [提示：]【新增·布局适配】懒创建滚动宿主：Dock=Fill 撑满 Panel_HomePage，尺寸随主窗口变化，
        // [提示：]        同时订阅它自己的 SizeChanged，窗口缩放时按新视口重新决定“铺满还是滚动”。
        private Panel EnsureScroller()                                        // 懒创建滚动宿主 Panel
        {
            if (pageScroller == null || pageScroller.IsDisposed)
            {
                pageScroller = new Panel();
                pageScroller.Name = "PageScroller";
                pageScroller.BackColor = Color.Transparent;
                pageScroller.Dock = DockStyle.Fill;
                pageScroller.AutoScroll = true;
                pageScroller.SizeChanged += Panel_HomePage_SizeChanged;
                Panel_HomePage.Controls.Add(pageScroller);
            }
            return pageScroller;
        }

        // [提示：]【新增·布局适配】主窗口尺寸变化时，只重算“当前正在显示的那一页”。
        // [提示：]        其余缓存页面等切回来时由 GoPage 里的那次 FitPageToPanel 校正，避免一次拖动触发多次布局。
        private void Panel_HomePage_SizeChanged(object sender, EventArgs e)   // 主窗口尺寸变了，重排当前页
        {
            if (string.IsNullOrEmpty(currentPageName))
            {
                return;
            }
            UIForm page = pageDict.ContainsKey(currentPageName) ? pageDict[currentPageName] : null;
            if (page != null && !page.IsDisposed)
            {
                FitPageToPanel(currentPageName, page);
            }
        }


        // [提示：]【权限映射】按页面名读 UserVo 里对应的 bool，这是“进不进得去”的唯一关卡。
        // [提示：]【缺陷】Output（产量统计）判的是 vo.IOManage（IO 权限），而 CountManage（产量权限）在整个项目里从未被使用：
        // [提示：]            结果是没有 IO 权限的人看不到产量，有产量权限的人反而进不去（见问题清单）。
        private bool checkAuthority(UserVo vo, string pageName)               // 按页面名查 UserVo 里的权限位
        {
            switch (pageName)
            {
                case nameof(Output):
                    return vo.IOManage;
                case nameof(DateLog):
                    return vo.LogManage;
                case nameof(UserFormCenter):
                    return vo.UserManage;
                case nameof(CameraManagement):
                    return vo.CameraManage;
                default:
                    UIMessageTip.ShowWarning("未配置跳转页面！");
                    return false;

            }
        }

        #region 提示：联表
        // [提示：]【联表】用户表 InnerJoin 桥表 InnerJoin 权限表：按 UId、AId 串起来，投影成 UserVo。
        // [提示：]【注意】条件写的是 u.AccountNum == User.UserName：User.UserName 里装的其实是“账号”
        // [提示：]        （LoginForm 写进 ini 的 userName 就是输入框里的账号），字段名与内容不符，容易误读。
        // [提示：]【性能】每次切页都查一次库，页面切换频繁时是纯浪费；可在登录后查一次缓存起来。
        #endregion
        private UserVo GetUserAndAuthority()                                  // 三表联查得到 UserVo
        {
            List<UserVo> result = SqlHelp.GetInstance().Queryable<UserEntity>()
                .InnerJoin<UserAndAuthorityEntity>((u, ua) => u.UId == ua.UId)
                .InnerJoin<AuthorityEntity>((u, ua, a) => ua.AId == a.AId)
                .Where(u => u.AccountNum == User.UserName && u.PassWord == User.PassWord)
                .Select((u, ua, a) => new UserVo
                {
                    UserName = u.UserName,
                    AccountNum = u.AccountNum,
                    PassWord = u.PassWord,
                    IOManage = a.IOManage,
                    CameraManage = a.CameraManage,
                    LogManage = a.LogManage,
                    UserManage = a.UserManage,
                    CountManage = a.CountManage,
                    MId = ua.MId,
                    AId = a.AId,
                    UId = u.UId
                }).ToList();

            return result.FirstOrDefault();
        }

        //页面跳转
        // [提示：]【入口】所有导航按钮共用这一个事件：从按钮的 Tag 里取页面名，交给 GoPage。
        private void Go_Page(object sender, EventArgs e)                      // 导航按钮共用事件：取 Tag 交给 GoPage
        {
            UIHeaderButton btn = sender as UIHeaderButton;
            GoPage(btn.Tag?.ToString());
            #region 问题：btn.Tag?.ToString() 这里为什么要加那个问号？
            // 【答：它是 C# 的“空条件运算符”（?.），作用是“左边为 null 就整个表达式直接返回 null，不再往下调用”：
            //      ①sender 被 as 转成 UIHeaderButton，若不是这个类型 btn 就是 null；
            //      ②控件 Tag 没赋值时也是 null；
            //      ③不加 ?. 就会变成 null.ToString() → 抛 NullReferenceException；加了则安静地得到 null，
            //        再由 GoPage 里的 string.IsNullOrEmpty 分支给出“未配置跳转页面”的提示。
            //      依据：?. 的语义是“短路求值并传播 null”。影响范围：所有导航按钮的 Tag 都必须配成页面名（Output/DateLog/…）。
            //      对比：btn.Tag.ToString() 只在“确定 Tag 一定有值”时才可用。】
            #endregion
        }
    }
}
