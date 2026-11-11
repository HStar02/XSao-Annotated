using System;
using System.Windows.Forms;

namespace XSao
{
    // [提示：]【类级说明】Program 是“入口容器”类：C# 要求 Main 必须挂在某个类型上，所以建了这个 internal static 空壳。
    // [提示：]        它定义全程序的启动骨架：Main → LoginForm（登录）→ New_HomePage（主界面），
    // [提示：]        Main 里这两次 Application.Run 少了任何一次，对应窗体都不会出现。
    internal static class Program
    {
        #region 提示：特性含义
        // [提示：]【特性含义】STAThread＝单线程单元（Single-Threaded Apartment）。
        // [提示：]        WinForms 依赖一批 COM 组件（剪贴板、文件对话框、拖放、部分输入法），
        // [提示：]        它们只能在 STA 线程上创建；漏掉本特性，典型症状是“复制粘贴或打开文件框偶发卡死”。
        // [提示：]        它只对入口方法生效，必须写在 Main 签名上方，位置不能挪。
        #endregion
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        // [提示：]【上下游】上游：操作系统加载程序集后自动调用；下游：两次 Application.Run。
        // [提示：]        Main 内不放业务逻辑，只做三件事：全局 UI 设置、起登录窗、按登录结果起主窗体。
        static void Main()   // 程序入口：UI设置→登录→按结果起主窗体
        {
            #region 提示：顺序约束
            // [提示：]【顺序约束】下面两行必须在任何窗体/控件句柄创建之前执行，否则不生效：
            // [提示：]            EnableVisualStyles：让控件使用系统视觉样式（否则按钮是 Win95 灰方框）；
            // [提示：]            SetCompatibleTextRenderingDefault(false)：文字用 GDI 排版而非 GDI+，更贴近系统字宽。
            // [提示：]            两者都是全进程一次性设置，只能在 Main 里设一次。
            #endregion
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            //打开CameraManagement
            Application.Run(new CameraManagement());

            /*
            LoginForm loginForm = new LoginForm();
            #region 提示：关键流程
            // [提示：]【关键流程】Application.Run(窗体) 会在这里阻塞、跑 Windows 消息循环，直到该窗体关闭才返回。
            // [提示：]            UILoginForm 基类的登录按钮在 OnLogin 返回 true 后会自己 Close()（见 LoginForm.cs），
            // [提示：]            所以这一行返回，就代表“登录交互已结束”，才有下一行判断结果的机会。
            // [提示：]            【常见坑】只写这一句、不写第二段 Run：登录成功窗一关，消息循环结束 → 进程退出，主界面永远出不来。
            #endregion
            Application.Run(loginForm);
            #region 提示：因果
            //这里需要判断登录成功
            // [提示：]【因果】IsLogin 是 SunnyUI UILoginForm 基类公开的 bool 属性：基类拿到 OnLogin 的返回值后写它，再关窗。
            // [提示：]        所以此处判断依据，实际来源于 LoginForm.LoginForm_OnLogin 的 return（本文件看不到，见 LoginForm.cs）。
            // [提示：]        为 false（账号密码错、点取消、3 次失败被锁）时什么都不做 → Main 结束 → 进程退出，且没有任何提示。
            #endregion
            if (loginForm.IsLogin)
            {
                // [提示：]【风险】同一线程先后两次 Application.Run，是 WinForms 官方支持的“两段式启动”；
                // [提示：]        但第一次 Run 退出时消息泵已结束过一轮，主窗体里再调 Application.Exit / DoEvents，语义指向的是第二段循环。
                // [提示：]        这里 new 出来的主窗体没有保存引用：主窗体关闭 = 第二段 Run 返回 = 程序退出（这正是期望行为）。
                Application.Run(new New_HomePage());
            }
            */
        }
    }
}
