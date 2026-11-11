using Sunny.UI;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using XSao.Entity;
using XSao.Help;

namespace XSao
{
    // [提示：]【类级说明】产量统计界面：一张柱状图 + 一张环形图，数据源是 hx_ResultCount。
    // [提示：]【统计口径】起止是同一天 → 按“小时”分组（只取 9~18 点）；跨天 → 按“日期”分组。
    public partial class Output : UIForm
    {
        // [优化：]统计口径的“班次时段”与 Y 轴参考线数值提为具名常量（数值与原实现完全相同，改一处即可全局生效）
        private const int ShiftStartHour = 9;
        private const int ShiftEndHour = 18;
        private const double OutputUpperLine = 560;
        private const double OutputLowerLine = 460;

        public Output()                                               // 构造：初始化界面
        {
            InitializeComponent();
        }

        // [提示：]【数据来源】一次性把整张 hx_ResultCount 拉进内存，再在内存里按日期过滤、分组。
        // [提示：]【性能缺陷】表越大启动越慢（全表 ToList），应把时间范围下推到 SQL（见问题清单）。
        public void Output_Load(object sender, EventArgs e)           // 加载时拉全表并画两张图
        {
            List<ResultCountEntity> list = SqlHelp.GetInstance().Queryable<ResultCountEntity>().ToList();
            LoadData(list);
        }
        // [提示：]【结构】LoadData 同时喂两张图，但柱状图与环形图各自重复了一遍“取数 + 分组 + 填轴”的代码，
        // [提示：]        同一份逻辑写两遍，是本文件最明显的维护负担：改统计口径必须改两处（见问题清单）。
        // [提示：]【时间基准】StartDay / EndDay 都取 .Date（当天 00:00:00），后面所有比较都基于这两个值。
        private void LoadData(List<ResultCountEntity> list)           // 按日期/小时分组，喂柱状图与环形图
        {
            DateTime StartDay = Count_Start_time.Value.Date;
            DateTime EndDay = Count_End_time.Value.Date;
            if (StartDay > EndDay) { Count_Start_time.Value = EndDay; }
            // [提示：]【缺陷】这里只把控件值改成了 EndDay，局部变量 StartDay / EndDay 没有跟着更新：
            // [提示：]        后面所有过滤仍用“开始 > 结束”的旧区间，查出来恒为空（见问题清单）。
            UIBarOption option = new UIBarOption();
            #region 柱状图

            option.Title = new UITitle();
            option.Title.Text = "产量统计";
            option.Title.SubText = "产量/小时";

            //设置Legend
            option.Legend = new UILegend();
            option.Legend.Orient = UIOrient.Horizontal;
            option.Legend.Top = UITopAlignment.Top;
            option.Legend.Left = UILeftAlignment.Left;
            option.Legend.AddData($"{DateTime.Now}产量");//option.Legend.AddData是图列的名字
            //设置series---列
            var series = new UIBarSeries();
            series.Name = $"{DateTime.Now:yyyy-MM-dd}产量_series";//series.Name是鼠标悬浮提示的名字

            //根据时间分组
            if (StartDay == EndDay)
            {
                var result = list
                    .Where(x => x.createTime.Date == StartDay)                    // ★ 先限定"这一天"
                    .Where(x => x.createTime.Hour >= ShiftStartHour && x.createTime.Hour <= ShiftEndHour)   // ← 只加这一行显示9-18点数据
                    .GroupBy(x => x.createTime.Hour).Select(x => new
                    {
                        Hour = x.Key,
                        OK = x.Count(y => y.result == "OK"),
                        NG = x.Count(y => y.result == "NG"),
                        Count = x.Count()

                    }).OrderBy(x => x.Hour).ToList();

                TextBox_output.Text = result.Sum(x => x.Count).ToString();
                OK_products.Text = result.Sum(x => x.OK).ToString();
                NG_products.Text = result.Sum(x => x.NG).ToString();


                foreach (var item in result)
                {
                    //根据数据进行分组
                    option.XAxis.Data.Add(item.Hour.ToString() + ":00");
                    series.AddData(item.Count);
                }
            }
            else
            {
                //根据日期分组，横坐标为日期，纵坐标为数量
                var result = list
                    .Where(x => x.createTime >= StartDay && x.createTime <= EndDay)
                    .GroupBy(x => x.createTime.Date).Select(x => new
                    {
                        Date = x.Key,
                        OK = x.Count(y => y.result == "OK"),
                        NG = x.Count(y => y.result == "NG"),
                        Count = x.Count()

                    }).OrderBy(x => x.Date).ToList();
                TextBox_output.Text = result.Sum(x => x.Count).ToString();
                OK_products.Text = result.Sum(x => x.OK).ToString();
                NG_products.Text = result.Sum(x => x.NG).ToString();
                foreach (var item in result)
                {
                    //根据数据进行分组
                    option.XAxis.Data.Add(item.Date.ToString("yyyy-MM-dd"));
                    series.AddData(item.Count);
                }
            }
            //设置X轴---标题
            //option.XAxis.Data.Add("Mon");
            //AddData和Data.Add是一一对应
            //设置Y轴---数据
            //series.AddData(10);


            option.Series.Add(series);//option.Series.Add(series);是显示？
            option.ToolTip.Visible = true;//option.ToolTip.Visible是鼠标悬浮提示是否可见
            option.YAxis.Scale = true;//option.YAxis.Scale是Y轴是否自动缩放
            //设置X轴和Y轴的名字
            option.XAxis.Name = "日期";
            //option.XAxis.AxisLabel.Angle = 60;控制X轴标签的旋转角度
            option.YAxis.Name = "数值";

            option.YAxisScaleLines.Add(new UIScaleLine() { Color = Color.Red, Name = "上限", Value = OutputUpperLine });
            // [提示：]【缺陷】Y 轴上下限写死 560 / 460：换产品、换节拍后这两条参考线毫无意义，
            // [提示：]        应从数据里算（如最大值/平均值）或做成可配置项。
            // [提示：]【业务含义】这两条线是“产能上/下限”提示：超过上限说明节拍被拉高，低于下限说明有停机或空转。
            option.YAxisScaleLines.Add(new UIScaleLine() { Color = Color.Gold, Name = "下限", Value = OutputLowerLine });

            uiBarChart1.SetOption(option);
            #endregion
            #region 环形图
            var option1 = new UIDoughnutOption();

            //设置Title
            option1.Title = new UITitle();
            option1.Title.Text = "产量统计";
            option1.Title.SubText = "产量/小时";
            option1.Title.Left = UILeftAlignment.Center;

            //设置ToolTip
            option1.ToolTip.Visible = true;

            //设置Legend
            option1.Legend = new UILegend();
            option1.Legend.Orient = UIOrient.Vertical;
            option1.Legend.Top = UITopAlignment.Top;
            option1.Legend.Left = UILeftAlignment.Left;

            //设置Series
            var series1 = new UIDoughnutSeries();
            series1.Name = $"{DateTime.Now:yyyy-MM-dd}产量_series";
            series1.Radius.Inner = 40;
            series1.Radius.Outer = 70;
            series1.Label.Show = true;
            series1.Label.Position = UIPieSeriesLabelPosition.Center;

            if (StartDay == EndDay)
            {
                var result = list
                    .Where(x => x.createTime.Date == StartDay)                    // ★ 先限定"这一天"
                    .Where(x => x.createTime.Hour >= ShiftStartHour && x.createTime.Hour <= ShiftEndHour)   // ← 只加这一行显示9-18点数据
                    .GroupBy(x => x.createTime.Hour).Select(x => new
                    {
                        Hour = x.Key,
                        OK = x.Count(y => y.result == "OK"),
                        NG = x.Count(y => y.result == "NG"),
                        Count = x.Count()

                    }).OrderBy(x => x.Hour).ToList();

                TextBox_output.Text = result.Sum(x => x.Count).ToString();
                OK_products.Text = result.Sum(x => x.OK).ToString();
                NG_products.Text = result.Sum(x => x.NG).ToString();


                foreach (var item in result)
                {
                    //根据数据进行分组
                    option1.Legend.AddData(item.Hour.ToString() + ":00");
                    series1.AddData(item.Hour.ToString() + ":00", item.Count);
                }
            }
            else
            {
                //根据日期分组，横坐标为日期，纵坐标为数量
                var result = list
                    .Where(x => x.createTime >= StartDay && x.createTime <= EndDay)
                    .GroupBy(x => x.createTime.Date).Select(x => new
                    {
                        Date = x.Key,
                        OK = x.Count(y => y.result == "OK"),
                        NG = x.Count(y => y.result == "NG"),
                        Count = x.Count()

                    }).OrderBy(x => x.Date).ToList();
                TextBox_output.Text = result.Sum(x => x.Count).ToString();
                OK_products.Text = result.Sum(x => x.OK).ToString();
                NG_products.Text = result.Sum(x => x.NG).ToString();
                foreach (var item in result)
                {
                    //根据数据进行分组
                    //根据数据进行分组
                    option1.Legend.AddData(item.Date.ToString("yyyy-MM-dd"));
                    series1.AddData(item.Date.ToString("yyyy-MM-dd"), item.Count);
                }
            }

            //增加Series
            option1.Series.Add(series1);

            //设置Option
            uiDoughnutChart1.SetOption(option1);
            #endregion

        }

        // [提示：]【刷新】查询按钮重新拉全表并重算两张图，没有防抖也没有缓存，连点就是重复劳动。
        private void uiBut_Select_Click(object sender, EventArgs e)   // 查询按钮：重新拉数并重画
        {
            //刷新
            LoadData(SqlHelp.GetInstance().Queryable<ResultCountEntity>().ToList());
        }


    }
}
