using Sunny.UI;
using System;
using System.Threading;
using XSao.Entity;
using XSao.Help;

namespace XSao
{
    #region 提示：类级说明
    // [提示：]【类级说明】登录窗体直接继承 SunnyUI 的 UILoginForm 模板：标题、输入框、按钮、样式全部由基类绘制。
    // [提示：]        本类只做两件事：①把“怎么校验账号密码”挂到基类的 OnLogin 事件；②校验通过后把账号密码写进 ini。
    // [提示：]【关键契约】OnLogin 是事件、不是可重写方法，委托签名是 bool(string userName, string password)：
    // [提示：]            返回 true → 基类置 IsLogin=true 并自己 Close()；返回 false → 留在登录界面。
    // [提示：]            所以 Program.cs 里 if (loginForm.IsLogin) 的依据，就是本类方法的返回值。
    #endregion
    public partial class LoginForm : UILoginForm
    {
        public LoginForm()                                                 // 构造：初始化登录界面
        {
            InitializeComponent();
            this.OnLogin += LoginForm_OnLogin;
        }
        //登录
        // [提示：]【业务】登录失败计数器，初值 1，配合下方 3-count 的“剩余次数”文案。
        // [提示：]【缺陷】count 只增不减：一旦超过 3，再输入错误就会算出负数（“剩余 -1 次”），见问题清单。
        private int count = 1;
        // [提示：]【路径】userInfo.ini 放在 exe 同目录（AppDomain.CurrentDomain.BaseDirectory）。
        // [提示：]【下游】New_HomePage 构造函数读同一个文件，把账号密码回填到 User 对象，用于后续权限查询。
        // [提示：]【风险】密码明文写进 ini，任何能访问该目录的人都能直接看到（安全问题，见清单）。
        public static string userInfoFile = AppDomain.CurrentDomain.BaseDirectory + "userInfo.ini";

        #region 提示：执行线程
        // [提示：]【执行线程】本方法在 UI 线程执行（由基类按钮点击触发），所以可以直接弹提示、改控件状态。
        // [提示：]【失败处理】返回 false 不关窗；连续失败超过 3 次后禁用整个窗体 + 弹框 + 睡 3 秒再恢复。
        // [提示：]【缺陷】Thread.Sleep(3000) 跑在 UI 线程上：界面会真的冻结 3 秒（连“取消”都点不动）；
        // [提示：]        若用户点“确定”已经 Close()，Sleep 之后还会对已关闭的窗体设置 Enabled=true。
        #endregion
        private bool LoginForm_OnLogin(string userName, string password)   // 校验账号密码，成功写 ini、失败计数锁定
        {
            //失败超过3次失败，不可输入
            //判断账号密码是否为空
            if (string.IsNullOrWhiteSpace(userName) || string.IsNullOrWhiteSpace(password))
            {
                UIMessageTip.ShowError("账号或密码不能为空！");
                return false;
            }
            bool ok = SqlHelp.GetInstance()
                .Queryable<UserEntity>()
                .Where(x => x.AccountNum == userName && x.PassWord == password)
                .Count() > 0;
            #region 提示：校验字段
            // [提示：]【校验字段】用的是 AccountNum（账号）+ PassWord（明文），不是 UserName（姓名）：
            // [提示：]            换成 UserName 会让“同名不同账号”的用户互相串登录。
            // [提示：]【安全】密码明文比对、无哈希，属明文存储体系（见问题清单）。
            // [提示：]【异常】数据库连不上时这里直接抛异常，登录界面会崩（没有 try/catch）。
            #endregion
            if (!ok)
            {
                UIMessageTip.ShowError($"账号或密码错误！剩余：{3 - count}次机会！");
                count++;
                // [提示：]【锁定逻辑】第 3 次失败时 count 已变 4 → 进入锁定：禁用窗体、弹框、睡 3 秒后恢复输入。
                // [提示：]【缺陷】恢复 Enabled=true 之后没有把 count 复位（例如置回 1），
                // [提示：]            下一次再输错会立刻再次锁定并显示“剩余 -1 次”，3 次阈值实际失效（见清单）。
                if (count > 3)
                {
                    //UIMessageBox.ShowError("登录失败次数过多，请稍后再试！");
                    this.Enabled = false;
                    //出现小弹窗提示,点击确定后，关闭窗体，否则等待3s后 this.Enabled =true
                    bool closeNow = UIMessageBox.Show(
                        "登录失败次数过多，请稍后再试！\r\n\r\n【确定】关闭程序",
                        "提示", UIStyle.Red, UIMessageBoxButtons.OKCancel);
                    if (closeNow) this.Close();
                    Thread.Sleep(3000);
                    this.Enabled = true;

                }
                return false;   // 返回 false：不关窗，留在登录界面重输
            }

            #region 提示：为什么写 ini
            //ini文件
            // [提示：]【为什么写 ini】程序重启后要能“记住上次登录的账号”，主界面用它去查权限。
            // [提示：]【风险】存的是明文密码（不是密文），读回后还直接用于 SQL 条件比对。
            // [提示：]【改进方向】只存账号不存密码（改为存 token），或退出时删除该文件（56 行源码注释已有此设想）。
            #endregion
            IniFile file = new IniFile(userInfoFile);
            file.Write("UserInfo", "UesrName", userName);
            file.Write("UserInfo", "Password", password);
            //优化点：关闭程序后删除ini文件

            return true;        // 返回 true：基类自己 Close()

        }

    }
}
