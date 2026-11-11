using Sunny.UI;
using System;
using System.Linq;
using XSao.Entity;
using XSao.Help;
using XSao.VO;

namespace XSao
{
    #region 提示：类级说明
    // [提示：]【类级说明】AddUser＝“新增用户 / 修改用户”共用的一个窗体（同一套控件，两种模式）。
    // [提示：]        模式判定：vo == null → 新增（走 SaveUser）；vo != null → 修改（走 UpdateUser）。
    // [提示：]【上下游】上游：UserFormCenter.But_AddUser_Click（新增）、But_EditUser_Click（修改，先 SetUser 回填）；
    // [提示：]            下游：保存成功后调 UpLoadData.Invoke() → 触发 UserFormCenter.LoadData 刷新表格。
    #endregion
    public partial class AddUser : UIForm
    {
        public event Action UpLoadData;
        // [提示：]【委托】UpLoadData 是“保存成功后请通知我”的回调，谁打开本窗体谁负责订阅。
        // [提示：]        订阅点：UserFormCenter.But_AddUser_Click / But_EditUser_Click 里的 addUser.UpLoadData += LoadData。
        // [提示：]【风险】某处 new AddUser() 后忘了订阅，UpLoadData 就是 null，91/184 行 Invoke() 会抛 NullReferenceException。

        public AddUser()                                              // 构造：只调 InitializeComponent
        {
            InitializeComponent();
        }
        // [提示：]【交互】取消按钮先弹确认框，只有点“确定”才关窗；点“取消”留在原地继续编辑。
        // [提示：]        UIMessageBox.Show(...OKCancel) 的返回值：true＝点了确定，false＝点了取消。
        /// <summary>
        /// 取消保存
        /// </summary>
        private void But_false_Click(object sender, EventArgs e)      // 取消按钮：确认后关窗、不保存
        {
            //UIMessageBox.Show("取消保存！","提示",UIStyle.Blue,UIMessageBoxButtons.OKCancel);
            //只有点击了确定，才关闭窗体
            bool result = UIMessageBox.Show("取消保存！", "提示", UIStyle.Blue, UIMessageBoxButtons.OKCancel);
            if (result)
            {
                this.Close();
            }
        }
        // [提示：]【分流】同一个“保存”按钮承担两种语义：vo==null 走新增，否则走修改。
        // [提示：]        vo 由 SetUser 灌入，这是模式开关，不是“数据是否为空”的判断，别改成判文本框。
        private void But_true_Click(object sender, EventArgs e)       // 保存按钮：vo 为空走新增，否则走修改
        {
            if (vo == null)
            {
                //新增用户
                SaveUser();
            }
            else
            {
                //修改用户
                UpdateUser();
            }

        }
        public UserVo vo;
        #region 提示：状态字段
        // [提示：]【状态字段】vo 既是“修改模式的数据来源”，也是“当前是不是修改模式”的标志。
        // [提示：]        public 是为了让 UserFormCenter 能间接检查；实际只由 SetUser 赋值。
        // [提示：]【职责】把选中行的 UserVo 回填到界面，并把 vo 存起来供 UpdateUser 使用。
        // [提示：]【关键】第 49 行 this.vo = userVo 不能漏：漏了 vo 仍为 null，点保存会走成“新增”，
        // [提示：]        于是同一个人被插两条记录——这就是源码注释里说的“雷点”。
        // [提示：]【业务】TB_ID（账号）被 Enabled=false 禁用：账号是唯一键，不允许在修改界面改，
        // [提示：]        否则会与登录校验、SaveUser 的判重逻辑冲突。
        #endregion
        public void SetUser(UserVo userVo)                            // 修改模式入口：把 UserVo 回填到表单
        {
            //雷点---忘记赋值，排查好久
            this.vo = userVo;
            //禁用账号---账号唯一性不能修改
            TB_ID.Enabled = false;
            TB_UserName.Text = userVo.UserName;
            TB_ID.Text = userVo.AccountNum;
            TB_PassWord.Text = userVo.PassWord;
            TB_AgainPW.Text = userVo.PassWord;
            Check_IO.Checked = userVo.IOManage;
            Check_camera.Checked = userVo.CameraManage;
            Check_Log.Checked = userVo.LogManage;
            Check_User.Checked = userVo.UserManage;
            Check_Output.Checked = userVo.CountManage;

        }

        #region 提示：流程
        // [提示：]【流程】校验两次密码 → 组 UserEntity（改用户表）→ 组 AuthorityEntity（改权限表）→ 两条 Update → 通知刷新 → 关窗。
        // [提示：]【缺陷 1】没有事务：两次 Updateable 各自独立提交，第二条失败时会出现“用户改了、权限没改”的半更新。
        // [提示：]【缺陷 2】没有给 UpdateTime 赋值：改完用户，表里的 UpdateTime 还停在创建时间。
        // [提示：]【注意】AccountNum 仍取 vo.AccountNum（正确，账号不允许变）；但 vo 为 null 时会直接崩，靠调用方保证。
        #endregion
        private void UpdateUser()                                     // 改已有用户：两条 Update，无事务
        {
            //此处可以加一个事务
            UserEntity userEntity = new UserEntity();
            userEntity.AccountNum = vo.AccountNum;
            userEntity.UserName = TB_UserName.Text;
            userEntity.PassWord = TB_PassWord.Text;
            userEntity.UId = vo.UId;
            if (TB_PassWord.Text != TB_AgainPW.Text)
            {
                UIMessageBox.ShowWarning("两次密码输入不一致！");
                return;
            }

            AuthorityEntity authorityEntity = new AuthorityEntity();
            authorityEntity.IOManage = Check_IO.Checked;
            authorityEntity.CameraManage = Check_camera.Checked;
            authorityEntity.LogManage = Check_Log.Checked;
            authorityEntity.UserManage = Check_User.Checked;
            authorityEntity.CountManage = Check_Output.Checked;
            authorityEntity.AId = vo.AId;

            SqlHelp.GetInstance().Updateable(userEntity).ExecuteCommand();
            SqlHelp.GetInstance().Updateable(authorityEntity).ExecuteCommand();

            UIMessageBox.Show("修改成功！");
            //刷新数据
            UpLoadData.Invoke();
            #region 问题：刷新界面的回调为什么用 Invoke 而不是 BeginInvoke？
            // 【答：Invoke 是同步的，BeginInvoke 是异步的，差别就在于“要不要等它执行完”：
            //      ①Invoke：把委托投递到 UI 线程，并在这里等委托执行完才返回 → 执行顺序有保证；
            //      ②BeginInvoke：投递完立刻返回，委托稍后才在 UI 线程执行 → 完成时间不确定。
            //      本处的因果链是：UpLoadData → UserFormCenter.LoadData（重绑 DataGridView.DataSource）→ 紧接着 this.Close() 关窗。
            //      若用 BeginInvoke，Close 可能先执行：要么用户看到旧列表闪一下，要么委托排队期间窗体已释放，
            //      回调里访问控件直接抛 ObjectDisposedException。所以“关窗前的收尾刷新”必须同步。
            //      影响范围：仅 91 行这一处（SaveUser 里 184 行同理）；两处本来就在 UI 线程（按钮 Click），
            //      Invoke 会同步直接执行、不开新线程，也不会死锁。】
            //关闭窗体
            #endregion
            this.Close();

        }
        #region 提示：业务
        // [提示：]【业务】一键全选/全不选：五个勾选框只要不是“全选中”，就全部选中；否则全部取消。
        // [提示：]        判定方式是用 && 串联五个 Checked：全为 true 才得到 true，取反后作为目标状态。
        // [提示：]【实现】遍历 GroupBox_czqx 里的所有 UICheckBox 统一赋值，比逐个写 5 行更耐改：
        // [提示：]        以后在分组里加第 6 个权限框，这里自动生效（但 Save/Update 里的取值得自己补一行）。
        // [提示：]【前提】权限勾选框必须放在 GroupBox_czqx 这个容器里，放外面就遍历不到。
        #endregion
        private void But_AllorNot_Click(object sender, EventArgs e)   // 五个权限框一键全选/全不选
        {
            //全选/全不选：一键切换五个权限勾选框
            //普通方法
            bool target = !(Check_IO.Checked && Check_camera.Checked && Check_Log.Checked && Check_User.Checked && Check_Output.Checked);
            //Check_IO.Checked = Check_camera.Checked = Check_Log.Checked = Check_User.Checked = Check_Output.Checked = target;
            //进阶写法
            foreach (var item in GroupBox_czqx.Controls.OfType<UICheckBox>())
            {
                item.Checked = target;
            }
        }
        #region 提示：流程
        // [提示：]【流程】非空校验 → 两次密码一致 → BeginTran → 账号判重 → 插用户(得 UId) → 插权限(得 AId) → 插中间表 → CommitTran。
        // [提示：]【为什么先判重】AccountNum 是登录账号且唯一；已存在就直接 return，避免插出两个同账号用户。
        // [提示：]【缺陷】144 行 return 与 209 行 catch 都没有 RollbackTran：事务一直挂着不提交，
        // [提示：]        后续操作可能报“已有未完成事务”，连接归还池时状态也不干净（见问题清单，高优先级）。
        // [提示：]【缺陷】catch 里把异常对象直接拼进提示（ex 的 ToString 很长），用户看不清真正原因。
        #endregion
        public void SaveUser()                                        // 新增用户：事务里插三张表
        {
            //保存用户信息
            //判断输入不能为空
            //TB_UserName，TB_ID，（TB_PassWord，TB_AgainPW）这两个必须一模一样
            //简单方法：输入框不能为空
            if (TB_UserName.Text == "" || TB_ID.Text == "" || TB_PassWord.Text == "" || TB_AgainPW.Text == "")
            {
                UIMessageBox.ShowWarning("请填写完整！");
                return;
            }
            //进阶方法:
            /*
            GroupBox_User.Controls.OfType<UITextBox>().ToList().ForEach(x =>
            {
                if (x.Text == "")
                {
                    UIMessageBox.ShowWarning("请填写完整:"+x.Tag.ToString());
                    return;
                }
            });
            */
            if (TB_PassWord.Text != TB_AgainPW.Text)
            {
                UIMessageBox.ShowWarning("两次密码输入不一致！");
                return;
            }

            //要么全部成功，要么全部失败
            SqlHelp.GetInstance().BeginTran();
            try
            {
                if (SqlHelp.GetInstance().Queryable<UserEntity>().Where(x => x.AccountNum == TB_ID.Text).ToList().Count > 0)
                {
                    UIMessageBox.ShowWarning("账号已存在！");
                    return;
                }
                //添加用户
                //Insertable是Sugar里面的方法
                int uid = SqlHelp.GetInstance().Insertable(new UserEntity
                {
                    AccountNum = TB_ID.Text,
                    UserName = TB_UserName.Text,
                    PassWord = TB_PassWord.Text,
                    CreateTime = DateTime.Now,
                    UpdateTime = DateTime.Now,
                }).ExecuteReturnIdentity();
                #region 问题：SqlSugar 的 ExecuteReturnIdentity 与 ExecuteReturnEntity 有什么区别，这里为什么用前者？
                // 【答：两者都执行 INSERT，区别只在“返回什么”：
                //      ①ExecuteReturnIdentity()：返回新行的自增主键值（这里就是 int uid）；
                //      ②ExecuteReturnEntity()：返回完整的新实体对象（主键 + 数据库端默认值回填后的字段）。
                //      本处要的是 uid / aid 两个数字去写中间表的 UId、AId，用 Identity 版最省；
                //      若还要读回 CreateTime 这类库端生成的值，才用 Entity 版。
                //      依据：SqlSugar 的 Insertable API；前提是表上真有自增列（hx_user.UId 已配 IsIdentity=true，满足）。
                //      影响范围：149 与 163 两处插入；实体版会多一次对象组装，性能上略贵但可忽略。】
                #endregion

                //添加权限
                int aid = SqlHelp.GetInstance().Insertable(new AuthorityEntity
                {
                    IOManage = Check_IO.Checked,
                    CameraManage = Check_camera.Checked,
                    LogManage = Check_Log.Checked,
                    UserManage = Check_User.Checked,
                    CountManage = Check_Output.Checked,

                }).ExecuteReturnIdentity();

                //判断是否添加成功
                if (SqlHelp.GetInstance().Insertable(new UserAndAuthorityEntity()
                {
                    UId = uid,
                    AId = aid
                }).ExecuteCommand() > 0)
                {
                    this.ShowWarningTip("成功！");
                    //提交事务
                    SqlHelp.GetInstance().CommitTran();
                    //成功后刷新数据---调用UserFormCenter内委托方法
                    UpLoadData.Invoke();
                    //清空输入框
                    TB_UserName.Text = "";
                    TB_ID.Text = "";
                    TB_PassWord.Text = "";
                    TB_AgainPW.Text = "";
                    Check_IO.Checked = false;
                    Check_camera.Checked = false;
                    Check_Log.Checked = false;
                    Check_User.Checked = false;
                    Check_Output.Checked = false;
                }
                else
                {
                    this.ShowWarningTip("保存失败！");
                    //回滚事务
                    SqlHelp.GetInstance().RollbackTran();
                }
                #region 问题：事务是怎么做到“三张表有一张没写成功就整体回滚”的？一句 Commit/Rollback 能同时控制三张表吗？
                // 【答：能，靠的是同一个连接上的同一个事务上下文，而不是那条语句本身有多强：
                //      ①139 行 BeginTran() 在 SqlHelp 的单例客户端上开启事务，此后该实例上的所有写操作都挂在这一个事务里；
                //      ②149/163/174 三次 Insertable 只是三步 DML，数据先进日志、不落盘、对外不可见；
                //      ③182 行 CommitTran() 才真正提交；200 行 RollbackTran() 则把三步一起撤销。
                //      所以“一句话”能控制三张表，是因为三张表共享同一个事务，而不是这句调用本身覆盖了三张表。
                //      依据：SQL Server 的事务是连接级上下文，SqlSugar 的 BeginTran/CommitTran/RollbackTran 直接映射它。
                //      【风险】本方法只在“三步都成功”的分支提交；账号已存在时 144 行是裸 return（没回滚），
                //      209 行 catch 里也只提示不回滚 → 事务悬空。验证方式：故意让第二张表插入失败，
                //      再执行一次任意查询，观察是否报“事务未结束”或数据是否残留。】
                #endregion
            }
            catch (Exception ex)
            {
                UIMessageBox.ShowWarning("保存失败！" + ex);
                return;
            }
        }
    }
}
