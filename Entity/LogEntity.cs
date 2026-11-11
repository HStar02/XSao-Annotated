using SqlSugar;
using System;

namespace XSao.Entity
{
    [SugarTable(TableName = "hx_log")]

    // [提示：]【表映射】hx_log＝操作日志表；DateLog 界面按 用户名/时间段/级别 三个条件分页查询它。
    // [提示：]【现状】本工程内只有查询，没有任何 Insert 这张表的代码——日志须由外部或后续补的写日志逻辑写入。
    // [提示：]        所以新装环境里日志界面永远是空的：不是查询写错，是压根没人写（见问题清单）。
    // [提示：]【字段】LogLevel 存 1/2/3，只在界面层翻译成 信息/警告/错误，库里不存中文。
    public class LogEntity
    {
        [SugarColumn(IsPrimaryKey = true, IsIdentity = true)]//数据库是自增才配
        public int LogId { get; set; }
        public string UserName { get; set; }
        public string SourceDevice { get; set; }
        public int LogLevel { get; set; }
        public string LogContent { get; set; }
        public DateTime CreateTime { get; set; }

    }
}
