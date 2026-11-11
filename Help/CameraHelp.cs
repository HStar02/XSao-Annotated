using System;

namespace XSao.Help
{
    // [提示：]【类级说明】CameraManagement 日志表格的行模型：一条操作记录＝一行（时间 + 文本）。
    // [提示：]        只服务界面展示，不落库；DataGridView 的两列靠 DataPropertyName 绑到 MakeTime / MakeLog。
    // [提示：]        internal：仅本程序集可见，不对外暴露成公共契约。
    internal class CameraHelp
    {
        public DateTime MakeTime { get; set; }
        public string MakeLog { get; set; }
    }
}
