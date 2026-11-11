using SqlSugar;

namespace XSao.Entity
{
    [SugarTable(TableName = "hx_authority")]
    // [提示：]【表映射】hx_authority＝权限表，一行代表某个用户的一套功能开关。
    // [提示：]        与 hx_user 通过中间表 hx_userAndAuthority(UId, AId) 关联；
    // [提示：]        五个 bool 分别控制 IO / 日志 / 用户 / 相机 / 产量 五个界面能否进入（判断点在新主页的 checkAuthority）。
    // [提示：]        权限跟着权限行走：一个用户对应一行，改权限＝改这一行，不是改用户表。
    /// <summary>
    /// 权限表
    /// </summary>
    public class AuthorityEntity
    {
        //自增
        [SugarColumn(IsPrimaryKey = true, IsIdentity = true)]//数据库是自增才配
        public int AId { get; set; }
        public bool IOManage { get; set; }
        public bool LogManage { get; set; }
        public bool UserManage { get; set; }
        public bool CameraManage { get; set; }
        public bool CountManage { get; set; }
    }
}
