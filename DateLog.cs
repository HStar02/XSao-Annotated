using SqlSugar;
using SqlSugar.Extensions;
using Sunny.UI;
using System;
using System.Collections.Generic;
using XSao.Entity;
using XSao.Help;

namespace XSao
{
    // [提示：]【类级说明】日志查询界面：三条件（用户名 / 时间段 / 级别）动态拼装 + 分页 DataGridView。
    // [提示：]【上下游】上游：新主页 GoPage(nameof(DateLog))；下游：SqlHelp 查 hx_log——本工程没有任何地方写这张表。
    public partial class DateLog : UIForm
    {
        // [提示：]【构造三件事】关自动列 → 加载数据 → 绑定级别下拉。
        // [提示：]【顺序】LoadData 放在 LoadLogLevelg 之前：此时 CB_LogLevel 还没有数据源、SelectedIndex=-1，
        // [提示：]        所以首屏查询不带级别条件，能查到全部记录（这个顺序是对的，别调换）。
        public DateLog()                                              // 构造：关自动列、载数据、填级别下拉
        {
            InitializeComponent();
            //禁止生成列---AutoGenerateColumns
            DataGridView_Log.AutoGenerateColumns = false;
            //加载数据库
            LoadData();
            LoadLogLevelg();
        }
        //创建日志级别List，1为信息，2为警告，3为错误
        public List<LogLevels> listLevel = new List<LogLevels>();
        //页码字段
        private int pageIndex = 1;
        private int pageSize = 20;
        private int totalCount;
        private int totalPage;

        // [提示：]【数据源】LogLevels 是界面专用的轻量类：DisplayMember 决定显示什么，ValueMember 决定取值是什么。
        // [提示：]        界面上看到“信息/警告/错误”，代码里 SelectedValue 拿到的是 1/2/3。
        private void LoadLogLevelg()                                  // 给级别下拉填 信息/警告/错误
        {
            listLevel.Add(new LogLevels() { LogLevel = 1, LogLevelName = "信息" });
            listLevel.Add(new LogLevels() { LogLevel = 2, LogLevelName = "警告" });
            listLevel.Add(new LogLevels() { LogLevel = 3, LogLevelName = "错误" });
            //添加到ComboBox
            CB_LogLevel.DataSource = listLevel;
            //设置显示字段
            CB_LogLevel.DisplayMember = "LogLevelName";
            //指定对应值字段值 需要显示的字段
            CB_LogLevel.ValueMember = "LogLevel";

            CB_LogLevel.SelectedIndex = -1;
            #region 问题：为什么要把 ComboBox 的 SelectedIndex 设成 -1？
            // 【答：SelectedIndex 默认是 0——数据源一绑上，第一项“信息”就被自动选中。
            //      而 72 行的查询条件是 if (CB_LogLevel.SelectedIndex > -1) 才拼“级别”过滤：
            //      保持 0 就等于“一打开界面就默认只看信息级”，警告/错误日志全被过滤掉，用户会以为数据丢了。
            //      设 -1 = 显式回到“未选择”，三个条件都不填时才能查出全部记录，也才能让 113 行的“重置”回到同一初始态。
            //      依据：ComboBox 绑定 DataSource 后 SelectedIndex 会被自动置 0，这是 WinForms 的既定行为。
            //      影响范围：本界面每次打开、以及点“重置”之后；顺序也重要——必须在设置 DataSource 之后设 -1。】
            #endregion

        }

        public class LogLevels
        {
            public int LogLevel { get; set; }
            public string LogLevelName { get; set; }
        }

        #region 提示：查询构造
        // [提示：]【查询构造】Expressionable 是 SqlSugar 的条件积木：三个 if 各自决定要不要 And 一个条件，
        // [提示：]        最终 ToExpression() 拼成表达式树交给 Where，等价于“没填的条件不参与过滤”。
        // [提示：]【投影】Select 里把 LogLevel 的 1/2/3 翻成中文，表格直接显示人话，不必再写单元格格式化。
        // [提示：]【分页】ToPageList 只取当前页 20 行，总条数/总页数用 ref 带出。
        // [提示：]【缺陷】翻页后没有重置 pageIndex：先翻到第 5 页再改查询条件，很可能查出 0 行（总页数变小了）。
        // [提示：]【缺陷】时间条件用 Convert.ToDateTime(文本框)，用户输入非法日期时抛 FormatException，本方法没有捕获。
        #endregion
        public void LoadData()                                        // 按三条件拼查询并分页取数
        {
            #region 点拨：下面这行查询结果没赋值给任何变量
            //【点拨】下面这行查询结果没赋值给任何变量，是冗余行（白查了），可删
            //SqlHelp.GetInstance().Queryable<LogEntity>().Where(x => x.LogId > 0).ToList();
            //【点拨·重点】Expressionable = 动态拼查询条件的"积木盒"：
            //3 个条件（用户名/时间/级别）用户可能只填几个，用它"有就加、没有就跳过"
            #endregion
            var expable = Expressionable.Create<LogEntity>();
            if (!string.IsNullOrEmpty(TextBox_UserID.Text))
            {
                //模糊查询，只要匹配到用户名就返回
                expable.And(x => x.UserName.Contains(TextBox_UserID.Text));
            }
            //按时间匹配---根据开始和结束时间
            if (!string.IsNullOrEmpty(Start_time.Text) && !string.IsNullOrEmpty(End_time.Text))
            {
                expable.And(x => x.CreateTime >= Convert.ToDateTime(Start_time.Text) && x.CreateTime <= Convert.ToDateTime(End_time.Text));
            }
            //根据日志级别，根据日志级别
            if (CB_LogLevel.SelectedIndex > -1)
            {
                expable.And(x => x.LogLevel == CB_LogLevel.SelectedValue.ObjToInt());
            }
            //将查询到的数据保存在DataGridView中
            var list = SqlHelp.GetInstance().Queryable<LogEntity>().Where(expable.ToExpression()).Select(a => new
            {
                LogId = a.LogId,
                UserName = a.UserName,
                SourceDevice = a.SourceDevice,
                LogLevel = a.LogLevel == 1 ? "信息" : a.LogLevel == 2 ? "警告" : a.LogLevel == 3 ? "错误" : "未知",
                LogContent = a.LogContent,
                CreateTime = a.CreateTime

            }).
            ToPageList(pageIndex, pageSize, ref totalCount, ref totalPage);
            #region 问题：ToList() 与 ToPageList() 有什么区别，这里为什么用 ToPageList？
            //ToList();
            // 【答：两者的差别是“取多少行、要不要顺带算总数”：
            //      ①ToList()：把满足条件的全部行一次性拉进内存，数据越多越慢、越吃内存（十万行日志会明显卡顿）；
            //      ②ToPageList(页码, 每页数, ref 总条数, ref 总页数)：只返回当前页那 20 行，
            //        另外用 ref 把总条数/总页数带回来。
            //      日志属于“越攒越多”的数据，必须分页；本界面 pageSize=20、pageIndex 从 1 开始。
            //      【注意】totalCount/totalPage 是 ref 传出的，首次查询前 totalPage 还是 0，
            //      此时点“下一页”会被判成“已是最后一页”，属初始化边界问题（见问题清单）。
            //      验证方式：断点看 list.Count 恒等于 20（最后一页除外），而 totalCount 是全量行数。】
            #endregion
            Label_Page.Text = "第" + pageIndex + "页/共" + totalPage + "页";
            //Label_Page.Text = $"第{pageIndex}页/共{totalPage}页";
            //先清空一下，避免数据重复
            DataGridView_Log.DataSource = null;
            DataGridView_Log.DataSource = list;
        }

        // [提示：]【缺陷】“查询”按钮没有把 pageIndex 复位成 1：停在第 3 页时改条件，第一页的数据会被直接跳过。
        // [提示：]        分页界面的通用规则是“查询条件变了就回第一页”。
        private void But_Select_Click(object sender, EventArgs e)     // 查询按钮：按当前条件重查
        {
            //刷新
            LoadData();
        }

        // [提示：]【重置】把三个条件全部清空、级别回到未选择，再查一次——不是“清空表格”，而是“回到无过滤状态”。
        private void But_CZ_Click(object sender, EventArgs e)         // 重置：清空条件后重查
        {
            //DataGridView_Log重置操作
            //所有输入查询都重置为空
            TextBox_UserID.Clear();
            Start_time.Text = null;
            End_time.Text = null;
            CB_LogLevel.SelectedIndex = -1;
            //刷新
            LoadData();
        }

        // [提示：]【边界】pageIndex<=1 时只提示不查询，避免 pageIndex=0 传给 ToPageList 造成非法参数。
        private void But_UpPage_Click(object sender, EventArgs e)     // 上一页（已到首页只提示）
        {
            //上一页
            if (pageIndex <= 1)
            {
                UIMessageTip.ShowWarning("已经是第一页了！");
                return;
            }
            else
            {
                pageIndex--;
                LoadData();
            }
        }

        // [提示：]【边界】pageIndex>=totalPage 时提示“最后一页”。注意 totalPage 首次为 0：
        // [提示：]        若首屏恰好 0 条数据，第 1 页就会被判成最后一页（提示语不准，属边界缺陷）。
        private void But_NextPage_Click(object sender, EventArgs e)   // 下一页（已到末页只提示）
        {
            //下一页
            if (pageIndex >= totalPage)
            {
                UIMessageTip.ShowWarning("已经是最后一页了！");
                return;
            }
            else
            {
                pageIndex++;
                LoadData();
            }
        }

        // [提示：]【校验】先 int.TryParse 判“是不是数字”，再判“在不在 1..totalPage 之间”，两道都过才改页码。
        // [提示：]        用 TryParse 而不是 Convert.ToInt32：后者遇到空串/字母会直接抛异常。
        private void But_JumpPage_Click(object sender, EventArgs e)   // 跳页：先校验数字与页码范围
        {
            //跳转页面
            //判断页码是否合法
            if (!int.TryParse(TextBox_Page.Text, out int pagenum))
            {
                UIMessageTip.ShowWarning("请输入正确的页码！");
                return;
            }
            //判断页码是否超出范围
            if (pagenum < 1 || pagenum > totalPage)
            {
                UIMessageTip.ShowWarning("页码超出范围！");
                return;
            }
            pageIndex = pagenum;
            LoadData();
        }
    }
}
