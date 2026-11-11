using SqlSugar;
using System;

namespace XSao.Entity
{
    [SugarTable(TableName = "hx_user")]
    // [提示：]【表映射】hx_user＝用户主表。两个字段最容易混，务必分清：
    // [提示：]        AccountNum＝登录账号（唯一，AddUser 里用它判重）；UserName＝姓名（可重名，只用于显示）。
    // [提示：]        登录校验走 AccountNum + PassWord（见 LoginForm），不是 UserName。
    // [提示：]【风险】PassWord 目前明文存库、明文比对，无盐无哈希；属安全问题，见问题清单。
    ///<summary>
    /// 用户表
    /// </summary>
    public class UserEntity
    {
        //id，自增列
        [SugarColumn(IsPrimaryKey = true, IsIdentity = true)]
        public int UId { get; set; }
        public string UserName { get; set; }
        public string AccountNum { get; set; }
        public string PassWord { get; set; }
        public DateTime CreateTime { get; set; }
        public DateTime UpdateTime { get; set; }

    }
}
