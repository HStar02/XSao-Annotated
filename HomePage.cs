using Sunny.UI;
namespace XSao
{
    // [提示：]【现状】这是旧版首页：五个按钮各自 new 一个窗体并 ShowDialog，互相独立、没有导航栏，也没有权限校验。
    // [提示：]        Program.cs 启动的是 New_HomePage，本类没有任何实例化点 → 已被取代的死代码。
    // [提示：]        保留通常是“怕以后要用”；确认无用后建议从 csproj 移除，减少编译时间与理解成本（见问题清单）。
    public partial class HomePage : UIForm
    {
        public HomePage()                                                // 构造：初始化旧版首页（死代码）
        {
            InitializeComponent();
        }

        private void H_Log_Click(object sender, System.EventArgs e)      // 旧版：弹日志窗
        {
            //跳转日志管理
            DateLog log = new DateLog();
            log.ShowDialog();
        }

        private void H_IO_Click(object sender, System.EventArgs e)       // 旧版：空实现，没接页面
        {

        }

        private void H_Camare_Click(object sender, System.EventArgs e)   // 旧版：弹相机窗
        {
            //跳转相机管理
            CameraManagement cameraManagement = new CameraManagement();
            cameraManagement.ShowDialog();
        }

        private void H_Output_Click(object sender, System.EventArgs e)   // 旧版：弹产量窗
        {
            //跳转输出管理
            Output output = new Output();
            output.ShowDialog();
        }

        private void H_User_Click(object sender, System.EventArgs e)     // 旧版：弹用户管理窗
        {
            //跳转用户管理
            UserFormCenter userFormCenter = new UserFormCenter();
            userFormCenter.ShowDialog();
        }
    }
}
