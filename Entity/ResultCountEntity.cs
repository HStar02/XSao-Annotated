using SqlSugar;
using System;

namespace XSao.Entity
{
    [SugarTable(TableName = "hx_ResultCount")]
    // [提示：]【表映射】hx_ResultCount＝识别结果流水：GLOSSED.ReportResult 每识别一次写一行（OK/NG）。
    // [提示：]        Output（产量统计）按 createTime 的小时/日期分组统计本表，得到柱状图与环形图数据。
    // [提示：]        internal + 字段名全小写：与数据库列名逐字对齐，SqlSugar 按属性名直接映射，不做大小写转换。
    internal class ResultCountEntity
    {
        [SugarColumn(IsPrimaryKey = true, IsIdentity = true)]//数据库是自增才配
        public int id { get; set; }
        public string productName { get; set; }
        public string result { get; set; }
        public DateTime createTime { get; set; }
    }
}
