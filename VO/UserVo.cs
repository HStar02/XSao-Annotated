namespace XSao.VO
{
    #region 提示：类级说明
    // [提示：]【类级说明】UserVo＝界面层使用的“用户 + 权限”合并视图对象，不是数据库实体，不映射任何表。
    // [提示：]        产生：UserFormCenter.LoadData / New_HomePage.GetUserAndAuthority 的三表 InnerJoin 投影；
    // [提示：]        去向：DataGridView 选中行 → 传给 AddUser.SetUser 回填表单；或用于界面权限判断。
    // [提示：]        因此它同时携带 用户字段(UId/UserName/AccountNum/PassWord)、权限字段(AId/五个bool)、桥表字段(MId)。
    #endregion
    public class UserVo
    {
        public int AId { get; set; }
        public bool IOManage { get; set; }
        public bool LogManage { get; set; }
        public bool UserManage { get; set; }
        public bool CameraManage { get; set; }
        public bool CountManage { get; set; }
        public int UId { get; set; }
        public string UserName { get; set; }
        public string AccountNum { get; set; }
        public string PassWord { get; set; }
        public int MId { get; set; }
    }
}
