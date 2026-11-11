using Sunny.UI;
using System;
using System.Windows.Forms;
using XSao.Entity;
using XSao.Help;
using XSao.VO;

namespace XSao
{
    // [提示：]【类级说明】用户管理界面：列表展示（三表联查）+ 新增/修改（复用 AddUser）+ 批量删除。
    // [提示：]【上下游】上游：新主页 GoPage(nameof(UserFormCenter))；下游：AddUser（新增/修改）、SqlHelp（三表增删改）。
    public partial class UserFormCenter : UIForm
    {
        //把刷新加载DataGridView_User，做成一个方法委托


        public UserFormCenter()                                       // 构造：关自动列并先加载一次数据
        {
            InitializeComponent();
            //在初始化界面时加载一遍数据
            //AutoGenerateColumns= false作用：设置该属性为false，DataGridView_User不会自动生成列
            DataGridView_User.AutoGenerateColumns = false;
            LoadData();
        }

        // [提示：]【委托绑定】addUser.UpLoadData += LoadData：AddUser 保存成功后回调本界面的 LoadData 刷新表格。
        // [提示：]【模式】这里没有 SetUser，new 出来的 vo 就是 null → AddUser 天然走“新增”分支。
        private void But_AddUser_Click(object sender, EventArgs e)    // 新增：开 AddUser，绑刷新委托
        {

            //跳转到添加用户界面
            AddUser addUser = new AddUser();
            //绑定Action委托
            addUser.UpLoadData += LoadData;
            addUser.ShowDialog();
        }
        // [提示：]【联表】三表 InnerJoin 后投影成匿名对象，直接绑到 DataGridView。
        // [提示：]        匿名对象的属性名必须与 Designer 里各列的 DataPropertyName 完全一致，否则那一列显示空白。
        // [提示：]【风险】投影里带了 PassWord：密码会明文显示在表格里（见问题清单）。
        public void LoadData()                                        // 三表联查刷新用户列表
        {
            //【点拨·重点】连表查询（InnerJoin 内连接）：
            //用户表 + 中间表 + 权限表 按主外键拼成一张大表再取需要的列，
            //类似 Excel 的 VLOOKUP（按 UId 把用户和权限拼到同一行）。
            var result = SqlHelp.GetInstance().Queryable<UserEntity>().InnerJoin<UserAndAuthorityEntity>((u, ua) => u.UId == ua.UId)
                .InnerJoin<AuthorityEntity>((u, ua, a) => ua.AId == a.AId).Select((u, ua, a) => new
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
            //接下来赋值给表格DataGridView_User
            DataGridView_User.DataSource = null;
            //先清空一下，避免数据重复
            DataGridView_User.DataSource = result;
        }
        //刷新按钮点击事件
        private void Refresh_Click(object sender, EventArgs e)        // 刷新按钮
        {
            //刷新数据
            LoadData();
        }

        #region 提示：流程
        // [提示：]【流程】统计勾选行数 → 二次确认 → 逐行删三张表 → 刷新 → 提示条数。
        // [提示：]【缺陷 1】三次 Deleteable 没有事务：中途失败会留下“用户删了、权限还在”的脏数据。
        // [提示：]【缺陷 2】没有 try/catch：数据库异常会直接抛到按钮事件，成为未处理异常。
        // [提示：]【缺陷 3】用户点“取消”确认框后，代码仍会执行 LoadData + ShowInfo，弹出一个“成功删除 0 条”的提示。
        #endregion
        private void But_DelUser_Click(object sender, EventArgs e)    // 删除勾选用户（逐行删三张表）
        {
            if (SelectRows() == 0)
            {
                UIMessageBox.ShowWarning("请选择要删除的用户！");
                return;
            }
            //删除DataGridView_User中Choices为true的行And从数据库中删除
            if (UIMessageBox.Show("确定删除选中的 " + SelectRows() + " 个用户吗？", "提示", UIStyle.Red, UIMessageBoxButtons.OKCancel))
            //if (UIMessageBox.ShowAsk("确定删除选中的 " + SelectRows() + " 个用户吗？", false, UIMessageDialogButtons.Ok))
            {
                foreach (DataGridViewRow row in DataGridView_User.Rows)
                {
                    if (Convert.ToBoolean(row.Cells["Choices"].Value))
                    {
                        SqlHelp.GetInstance().Deleteable<UserEntity>().Where(u => u.UId == Convert.ToInt32(row.Cells["UId"].Value)).ExecuteCommand();
                        SqlHelp.GetInstance().Deleteable<UserAndAuthorityEntity>().Where(u => u.UId == Convert.ToInt32(row.Cells["UId"].Value)).ExecuteCommand();
                        SqlHelp.GetInstance().Deleteable<AuthorityEntity>().Where(u => u.AId == Convert.ToInt32(row.Cells["AId"].Value)).ExecuteCommand();
                    }
                }
            }
            //刷新数据
            LoadData();
            UIMessageBox.ShowInfo("成功删除：" + SelectRows() + "条数据！");
            // [提示：]【缺陷】这里的 SelectRows() 是在 LoadData() 刷新表格之后重新统计的：
            // [提示：]        此时勾选已被清空 → 提示恒为“成功删除：0 条数据”，与实际删除行数不符（见清单）。
            // [提示：]        正确做法：删除前把行数存进局部变量，提示时用那个变量。
        }
        // [提示：]【实现】遍历所有行，数 Choices 列为 true 的行数。
        // [提示：]        Convert.ToBoolean(null) 得到 false，所以“没勾过”的单元格不会抛异常，这点是安全的。
        private int SelectRows()                                      // 统计勾选了多少行
        {
            int rows = 0;
            foreach (DataGridViewRow row in DataGridView_User.Rows)
            {
                if (Convert.ToBoolean(row.Cells["Choices"].Value))
                {
                    rows++;
                }
            }
            return rows;
        }

        // [提示：]【业务约束】只能选一个：SelectRows() != 1 就提示并返回，避免“多选后改错人”。
        // [提示：]【流程】取选中行 → new AddUser → 绑委托 → 改标题为“修改用户” → SetUser 回填 → ShowDialog。
        // [提示：]【关键】SetUser 内部会把 vo 存起来，AddUser 才知道该走修改而不是新增（漏了就变成插重复用户）。
        private void But_EditUser_Click(object sender, EventArgs e)   // 修改：只选一行，回填后开 AddUser
        {
            //修改用户信息，只能选一个用户
            if (SelectRows() != 1)
            {
                UIMessageBox.ShowWarning("请选择一个用户进行修改！");
                return;
            }
            UserVo result = SelectRowResult();
            //跳转到修改用户界面
            AddUser addUser = new AddUser();
            //绑定Action委托
            addUser.UpLoadData += LoadData;
            //将“添加用户”修改为“修改用户”
            addUser.Text = "修改用户";
            //窗口数据绑定---调用SetUser方法
            addUser.SetUser(result);
            #region 点拨：委托绑定在 112 行已完成
            //【点拨】委托绑定在 112 行已完成（addUser.UpLoadData += LoadData）。
            //作用：AddUser 保存/修改成功后调 UpLoadData.Invoke() → 自动刷新这里的表格。
            //若不绑定，UpLoadData 是 null，AddUser 里 Invoke() 会报"未将对象引用设置到对象的实例"。
            //然后打卡窗口
            #endregion
            addUser.ShowDialog();
        }
        private UserVo SelectRowResult()                              // 把勾选那一行取成 UserVo
        {
            foreach (DataGridViewRow row in DataGridView_User.Rows)
            {
                if (Convert.ToBoolean(row.Cells["Choices"].Value))
                {
                    return new UserVo()
                    {
                        UserName = row.Cells["UserName"].Value.ToString(),
                        AccountNum = row.Cells["AccountNum"].Value.ToString(),
                        PassWord = row.Cells["PassWord"].Value.ToString(),
                        IOManage = Convert.ToBoolean(row.Cells["IOManage"].Value),
                        CameraManage = Convert.ToBoolean(row.Cells["CameraManage"].Value),
                        LogManage = Convert.ToBoolean(row.Cells["LogManage"].Value),
                        UserManage = Convert.ToBoolean(row.Cells["UserManage"].Value),
                        CountManage = Convert.ToBoolean(row.Cells["CountManage"].Value),
                        AId = Convert.ToInt32(row.Cells["AId"].Value),
                        UId = Convert.ToInt32(row.Cells["UId"].Value),
                    };
                }
            }
            return null;
        }
    }
}
