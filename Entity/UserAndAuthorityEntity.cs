using SqlSugar;

namespace XSao.Entity
{
    [SugarTable(TableName = "hx_userAndAuthority")]
    // [提示：]【表映射】hx_userAndAuthority＝用户与权限之间的桥表（多对多关系的落地点）。
    // [提示：]        删除一个用户必须三张表一起删：hx_user → 本表 → hx_authority；
    // [提示：]        漏删任何一张都会留下孤立权限行，下次按权限联表查询就查不到该用户（UserFormCenter 正在做这件事）。
    ///<summary>
    /// 用户，权限---中间表
    /// </summary>
    public class UserAndAuthorityEntity
    {
        [SugarColumn(IsPrimaryKey = true, IsIdentity = true)]//数据库是自增才配
        public int MId { get; set; }
        public int UId { get; set; }
        public int AId { get; set; }

    }
}
