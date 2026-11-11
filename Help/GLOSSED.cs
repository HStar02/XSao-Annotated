using HalconDotNet;
using System;
using System.Diagnostics;
using XSao.Entity;

namespace XSao.Help
{
    #region 提示：上下游
    // [提示：]【上下游】上游：CameraManagement.HalconOCR 调 num.TiaoMa(Himage, hv_CodeTypes, hv_WindowHandle, out count)；
    // [提示：]            另一个入口 GLO_OCR 目前没人调用（HalconOCR 里那两行被注释掉了），属备用能力。
    // [提示：]        下游：每次识别都经 ReportResult 往 hx_ResultCount 写一行，Output 界面按这张表统计产量。
    // [提示：]        无状态类：可以随意 new，不会持有图像或模型句柄（都在方法内 try/finally 里释放）。
    #endregion
    /// <summary>
    /// GLOSSED：视觉识别帮助类（OCR 字符识别 + 一维码识别）。
    ///
    /// 【结构说明】
    /// 原来同一套算法写了两个版本（带窗口显示 / 不带窗口显示），几百行 HALCON 流水线抄了两遍；
    /// 结果入库也散在各处，OCR 那边干脆没写。现在统一成三层：
    ///   ① 公开入口：GLO_OCR / TiaoMa        —— 只决定"要不要画到窗口上"，对外参数保持原样，调用方不用改
    ///   ② 私有核心：RunOcr / DecodeBarCode  —— 只管算法、不管画图，算法改一处两个入口同时生效
    ///   ③ 公共工具：ReportResult（入库）、DispOcrChars / DispBarCodeResult（绘制）
    /// 另外把 HObject / 条码模型的释放统一挪进 finally，正常、异常两条路都不会漏 Dispose。
    /// </summary>
    public class GLOSSED
    {
        // [优化：]字符识别的面积筛选区间由魔法数字提为具名常量（数值与原实现完全相同，仅便于统一调整）
        private const int OcrAreaMin = 5000;
        private const int OcrAreaMax = 20000;
        #region 常量：产品名与结果字面量
        /// <summary>写进数据库 hx_ResultCount 表的产品名；以后换产品只改这一处。</summary>
        private const string PRODUCT_NAME = "口红管";

        /// <summary>识别成功（OCR 至少识别到 1 个字符、一维码至少找到 1 个码）。</summary>
        private const string RESULT_OK = "OK";

        /// <summary>识别失败（抛异常、0 个结果都算失败）。</summary>
        private const string RESULT_NG = "NG";
        #endregion

        #region 公开入口：OCR 字符识别
        /// <summary>
        /// OCR 识别（只出结果，不画图）。
        /// 参数：ho_Image 待识别图像；hv_Class 输出，按字符位置排好序的识别结果（HTuple 字符串数组）。
        /// 说明：识别结果（OK/NG）由内部统一写数据库；出错时记 NG 后把异常继续抛给调用方。
        /// </summary>
        public void GLO_OCR(HObject ho_Image, out HTuple hv_Class)              // OCR识别：只出结果，不画图
        {
            hv_Class = RunOcr(ho_Image, new HTuple(), false);
        }

        /// <summary>
        /// OCR 识别（把每个字符的识别结果画在字符正下方）。
        /// 参数：hv_WindowHandle 往哪个 HALCON 窗口上画字；hv_Class 输出，识别结果。
        /// </summary>
        public void GLO_OCR(HObject ho_Image, HTuple hv_WindowHandle, out HTuple hv_Class)  // OCR识别：结果画到窗口上
        {
            hv_Class = RunOcr(ho_Image, hv_WindowHandle, true);
        }
        #endregion

        #region 公开入口：一维码识别
        /// <summary>
        /// 一维码识别（只要数量，不需要解码内容时的便捷调用）。
        /// 参数：ho_Image 待识别图像；hv_CodeTypes 码制（如 "EAN-13"）；hv_WindowHandle 显示窗口。
        /// 输出：hv_CodeCount 识别到的条码数量，0 表示没找到。
        /// </summary>
        public void TiaoMa(HObject ho_Image, HTuple hv_CodeTypes, HTuple hv_WindowHandle,
            out HTuple hv_CodeCount)                                            // 一维码识别：只取数量
        {
            HTuple hv_DecodedDataStrings = new HTuple();
            TiaoMa(ho_Image, hv_CodeTypes, hv_WindowHandle, out hv_DecodedDataStrings, out hv_CodeCount);
            // 解码内容这里不用，用完即释放，避免 HTuple 句柄堆积
            hv_DecodedDataStrings.Dispose();
        }

        /// <summary>
        /// 一维码识别（完整版：既要数量，也要解码内容）。
        /// 参数：ho_Image 待识别图像；hv_CodeTypes 码制；hv_WindowHandle 显示窗口。
        /// 输出：hv_DecodedDataStrings 解码出的字符串（与条码区域一一对应）；hv_CodeCount 条码数量。
        /// </summary>
        public void TiaoMa(HObject ho_Image, HTuple hv_CodeTypes, HTuple hv_WindowHandle,
            out HTuple hv_DecodedDataStrings, out HTuple hv_CodeCount)          // 一维码识别：数量 + 解码内容
        {
            // 条码区域由核心方法输出，这里负责用完释放
            HObject ho_SymbolRegions = null;
            hv_DecodedDataStrings = new HTuple();
            hv_CodeCount = new HTuple();
            try
            {
                // 第一步：只管找码和解码，算法细节都在 DecodeBarCode 里
                hv_DecodedDataStrings = DecodeBarCode(ho_Image, hv_CodeTypes, out ho_SymbolRegions);

                // 第二步：按"解码串的个数"判断有没有码，绝不能判断字符串是不是空串
                hv_CodeCount = new HTuple(hv_DecodedDataStrings.TupleLength());

                // 第三步：分成功 / 失败两条路——先画图，再入库（先画后记，画图出错时不会重复入库）
                if (hv_CodeCount.I == 0)
                {
                    HOperatorSet.DispText(hv_WindowHandle, "未识别到一维码", "window", 20, 20,
                        "red", "box", "true");
                    ReportResult(false);
                }
                else
                {
                    DispBarCodeResult(hv_WindowHandle, ho_SymbolRegions, hv_DecodedDataStrings, hv_CodeCount.I);
                    HOperatorSet.DispText(hv_WindowHandle,
                        "成功识别 " + hv_CodeCount.I + " 个一维码", "window", 20, 20,
                        "dark green", "box", "true");
                    ReportResult(true);
                }
            }
            catch (Exception ex)
            {
                // 异常路径同样要入库（记 NG），然后把原始异常继续往上抛，异常信息不要丢
                ReportResult(false);
                HOperatorSet.DispText(hv_WindowHandle, "识别发生异常：" + ex.Message, "window", 20, 20,
                    "red", "box", "true");
                throw;
            }
            finally
            {
                // 条码区域也要在 finally 里释放：正常结束、异常、提前 return 都会走到这里
                if (ho_SymbolRegions != null)
                {
                    ho_SymbolRegions.Dispose();
                }
            }
        }
        #endregion

        #region 私有核心：OCR 流水线
        #region 提示：算法链路
        // [提示：]【算法链路】反转 → 阈值(0~100) → 连通域 → 面积筛选(5000~20000) → 按字符排序 → 读字库 → 多字符识别。
        // [提示：]【参数敏感】面积区间 5000~20000 是按当前相机分辨率/工作距离调出来的经验值：
        // [提示：]            换镜头、改视野或改分辨率后字符面积会整体缩放，这段必须重调，否则一个字都选不出来（识别结果恒为空）。
        // [提示：]【异常约定】catch 里记 NG 后 throw，不在本层吞异常：调用方（界面按钮事件）必须自己 try/catch，
        // [提示：]            否则 HALCON 抛的异常会变成未处理异常，直接把程序崩掉（CameraManagement 目前没有接）。
        #endregion
        /// <summary>
        /// OCR 核心流水线：反转 → 阈值 → 连通域 → 面积过滤 → 字符排序 → 字库识别。
        /// 参数：ho_Image 待识别图像；hv_WindowHandle 显示窗口（dispChars 为 false 时不用）；
        ///      dispChars 是否把识别结果逐字符画到窗口上。
        /// 返回：按位置排好序的识别结果 HTuple。
        /// 说明：不管成功还是失败都会写一条结果到数据库（成功 OK、异常/零结果 NG）。
        /// </summary>
        private HTuple RunOcr(HObject ho_Image, HTuple hv_WindowHandle, bool dispChars)  // OCR流水线：反转→阈值→连通域→按字符识别
        {
            // 图像类中间变量：反转图 / 阈值区域 / 连通域 / 面积过滤后 / 排序后
            HObject ho_ImageInvert = null, ho_Regions = null, ho_ConnectedRegions = null;
            HObject ho_SelectedRegions = null, ho_SortedRegions = null;

            // 控制类变量：OCR 句柄 / 置信度 / 字符个数
            HTuple hv_OCRHandle = new HTuple(), hv_Confidence = new HTuple();
            HTuple hv_CharCount = new HTuple();
            HTuple hv_Class = new HTuple();
            try
            {
                // HALCON 转译铁律：HObject 先造"空对象"占位，之后才能安全 Dispose
                HOperatorSet.GenEmptyObj(out ho_ImageInvert);
                HOperatorSet.GenEmptyObj(out ho_Regions);
                HOperatorSet.GenEmptyObj(out ho_ConnectedRegions);
                HOperatorSet.GenEmptyObj(out ho_SelectedRegions);
                HOperatorSet.GenEmptyObj(out ho_SortedRegions);

                // 关键：原图黑字白底 → 反转成白字黑底，阈值才好用
                ho_ImageInvert.Dispose();
                HOperatorSet.InvertImage(ho_Image, out ho_ImageInvert);

                // 提取黑色像素点
                ho_Regions.Dispose();
                HOperatorSet.Threshold(ho_ImageInvert, out ho_Regions, 0, 100);

                // 识别原理——一个个识别（需要先分离）
                ho_ConnectedRegions.Dispose();
                HOperatorSet.Connection(ho_Regions, out ho_ConnectedRegions);

                // 去除不必要杂点——只识别字符（面积 5000~20000 才算字符）
                ho_SelectedRegions.Dispose();
                HOperatorSet.SelectShape(ho_ConnectedRegions, out ho_SelectedRegions, "area",
                    "and", OcrAreaMin, OcrAreaMax);

                // 字符排序：用 'character' 模式按从左到右排，识别结果才会按顺序对上号
                ho_SortedRegions.Dispose();
                HOperatorSet.SortRegion(ho_SelectedRegions, out ho_SortedRegions, "character",
                    "true", "row");

                // 重点——识别字符：1.加载字符训练库（HALCON 自带 0-9、A-Z、a-z 字库）
                hv_OCRHandle.Dispose();
                HOperatorSet.ReadOcrClassMlp("OCRB_A-Z+_NoRej.omc", out hv_OCRHandle);

                // 2.执行模型——输入排序好的字符区域和反转图
                hv_Class.Dispose(); hv_Confidence.Dispose();
                HOperatorSet.DoOcrMultiClassMlp(ho_SortedRegions, ho_ImageInvert, hv_OCRHandle,
                    out hv_Class, out hv_Confidence);

                // 3.画图（只有带窗口的入口才画），再统一入库
                if (dispChars)
                {
                    DispOcrChars(hv_WindowHandle, ho_SortedRegions, hv_Class);
                }

                // 一个字符都没识别出来同样算失败，记 NG
                hv_CharCount.Dispose();
                HOperatorSet.CountObj(ho_SortedRegions, out hv_CharCount);
                ReportResult(hv_CharCount.I > 0);

                return hv_Class;
            }
            catch (Exception ex)
            {
                // 异常路径记 NG，异常继续抛给调用方，不在这一层"吃掉"错误
                ReportResult(false);
                if (dispChars)
                {
                    HOperatorSet.DispText(hv_WindowHandle, "OCR 识别异常：" + ex.Message, "window",
                        20, 20, "red", "box", "true");
                }
                throw;
            }
            finally
            {
                // 正常、异常都从这里释放，避免 HALCON 内存泄漏
                if (ho_ImageInvert != null) { ho_ImageInvert.Dispose(); }
                if (ho_Regions != null) { ho_Regions.Dispose(); }
                if (ho_ConnectedRegions != null) { ho_ConnectedRegions.Dispose(); }
                if (ho_SelectedRegions != null) { ho_SelectedRegions.Dispose(); }
                if (ho_SortedRegions != null) { ho_SortedRegions.Dispose(); }
                hv_OCRHandle.Dispose();
                hv_Confidence.Dispose();
                hv_CharCount.Dispose();
            }
        }
        #endregion

        #region 私有核心：一维码解码
        #region 提示：关键参数
        // [提示：]【关键参数】element_size_min / element_size_max 是“单根条或空的宽度范围（像素）”，HALCON 默认只到 8，
        // [提示：]            且不随图像分辨率自适应。整幅大图上码的模块宽超过默认上限时，候选会在筛选阶段被静默剔除：
        // [提示：]            既不报错也不返回，看起来像“码印坏了”，实际是尺寸参数把大码筛掉了。
        // [提示：]【源码实际取值】这里传的是 1 和 100，即把可接受宽度整体放宽到 1~100 像素（不是用默认的 2~8），
        // [提示：]            所以高分辨率、远视野下的大码也能进候选。代价是候选明显变多：find_bar_code 更慢、误检概率上升；
        // [提示：]            现场码制与视野固定时，收窄这个上限既提速也更稳，改动前需拿实拍图对比识别率。
        #endregion
        /// <summary>
        /// 一维码核心：建模型 → 设参数 → find_bar_code 找码并解码。
        /// 参数：ho_Image 待识别图像；hv_CodeTypes 码制（这里是 EAN-13）。
        /// 输出：ho_SymbolRegions 找到的条码区域（可能有多个，由调用方释放）。
        /// 返回：解码出来的字符串，和区域一一对应。
        /// </summary>
        private HTuple DecodeBarCode(HObject ho_Image, HTuple hv_CodeTypes, out HObject ho_SymbolRegions)  // 条码流水线：找码→定单元尺寸→解码
        {
            HTuple hv_BarCodeHandle = new HTuple();
            HTuple hv_DecodedDataStrings = new HTuple();
            // 标记模型是否真的建出来了：没建出来就不能 Clear，否则会报无效句柄
            bool isModelCreated = false;
            ho_SymbolRegions = null;
            try
            {
                HOperatorSet.CreateBarCodeModel(new HTuple(), new HTuple(), out hv_BarCodeHandle);
                HOperatorSet.SetBarCodeParam(hv_BarCodeHandle, "element_size_min", 1);
                HOperatorSet.SetBarCodeParam(hv_BarCodeHandle, "element_size_max", 100);
                isModelCreated = true;

                // 'stop_after_result_num' 表示"找到多少个结果就停止搜索"：
                // 设 0 = 不限制数量，图里有几个码就全找出来；千万别设 1，否则找到第一个就停，后面全漏
                HOperatorSet.SetBarCodeParam(hv_BarCodeHandle, "stop_after_result_num", 0);

                // find_bar_code：核心算子。输入 Image / BarCodeHandle / CodeTypes，
                // 输出 SymbolRegions（码区域，可能有多个）和 DecodedDataStrings（解码串，与区域一一对应）
                HOperatorSet.FindBarCode(ho_Image, out ho_SymbolRegions, hv_BarCodeHandle,
                    hv_CodeTypes, out hv_DecodedDataStrings);

                return hv_DecodedDataStrings;
            }
            finally
            {
                // 正常执行完、发生异常都要释放条码模型——"用完就还"，否则模型句柄会一直涨
                if (isModelCreated)
                {
                    HOperatorSet.ClearBarCodeModel(hv_BarCodeHandle);
                }
                hv_BarCodeHandle.Dispose();
            }
        }
        #endregion

        #region 私有方法：绘制
        /// <summary>
        /// 把识别结果逐个画在字符正下方（图像坐标系，跟着图像一起缩放）。
        /// 参数：hv_WindowHandle 显示窗口；ho_SortedRegions 排好序的字符区域；hv_Class 识别结果。
        /// </summary>
        private void DispOcrChars(HTuple hv_WindowHandle, HObject ho_SortedRegions, HTuple hv_Class)  // 把每个识别字符画在图上
        {
            HObject ho_SingleCharRegion = null;
            HTuple hv_Area = new HTuple(), hv_Row = new HTuple(), hv_Column = new HTuple();
            HTuple hv_CharCount = new HTuple();
            try
            {
                HOperatorSet.GenEmptyObj(out ho_SingleCharRegion);
                HOperatorSet.CountObj(ho_SortedRegions, out hv_CharCount);
                int charCount = hv_CharCount.I;

                // HALCON 图像对象编号从 1 开始，所以循环 1..charCount
                for (int i = 1; i <= charCount; i++)
                {
                    ho_SingleCharRegion.Dispose();
                    HOperatorSet.SelectObj(ho_SortedRegions, out ho_SingleCharRegion, i);

                    // 取该字符区域的中心坐标：Row 是行（上下），Column 是列（左右）
                    HOperatorSet.AreaCenter(ho_SingleCharRegion, out hv_Area, out hv_Row, out hv_Column);

                    // 识别结果数组下标从 0 开始，图像编号从 1 开始，所以取 i-1；
                    // 行坐标 +20 让文字落在字符正下方
                    HOperatorSet.DispText(hv_WindowHandle, hv_Class.TupleSelect(i - 1), "image",
                        hv_Row + 20, hv_Column, "green", "box", "false");
                }
            }
            finally
            {
                if (ho_SingleCharRegion != null) { ho_SingleCharRegion.Dispose(); }
            }
        }

        /// <summary>
        /// 把条码画成绿框，并在每个码上方写"序号: 解码内容"。
        /// 参数：ho_SymbolRegions 条码区域集合；hv_DecodedDataStrings 解码串（与区域一一对应）；
        ///      codeCount 条码数量；hv_WindowHandle 显示窗口。
        /// </summary>
        private void DispBarCodeResult(HTuple hv_WindowHandle, HObject ho_SymbolRegions,
            HTuple hv_DecodedDataStrings, int codeCount)                        // 条码画绿框并标注解码内容
        {
            HObject ho_OneCodeRegion = null;
            HTuple hv_WindowRow = new HTuple(), hv_WindowColumn = new HTuple();
            HTuple hv_WindowWidth = new HTuple(), hv_WindowHeight = new HTuple();
            HTuple hv_Row1 = new HTuple(), hv_Column1 = new HTuple();
            HTuple hv_Row2 = new HTuple(), hv_Column2 = new HTuple();
            HTuple hv_CodeWindowRow = new HTuple(), hv_CodeWindowColumn = new HTuple();
            HTuple hv_Ascent = new HTuple(), hv_Descent = new HTuple();
            HTuple hv_TextWidth = new HTuple(), hv_TextHeight = new HTuple();
            HTuple hv_TextRow = new HTuple(), hv_TextColumn = new HTuple();
            HTuple hv_Label = new HTuple();
            try
            {
                HOperatorSet.GenEmptyObj(out ho_OneCodeRegion);

                // set_draw 设 'margin'：只画边缘轮廓、不填充内部，否则会把条码本身盖住
                HOperatorSet.SetDraw(hv_WindowHandle, "margin");
                HOperatorSet.SetColor(hv_WindowHandle, "green");
                HOperatorSet.SetLineWidth(hv_WindowHandle, 3);
                HOperatorSet.SetFont(hv_WindowHandle, "微软雅黑-Bold-22");

                // disp_obj：把所有条码区域用绿框画出来
                HOperatorSet.DispObj(ho_SymbolRegions, hv_WindowHandle);

                // 取窗口实际尺寸，不再假定窗口是 1000×733，兼容缩放和显示区域变化
                HOperatorSet.GetWindowExtents(hv_WindowHandle, out hv_WindowRow, out hv_WindowColumn,
                    out hv_WindowWidth, out hv_WindowHeight);

                for (int i = 0; i < codeCount; i++)
                {
                    // select_obj：取第 i+1 个区域（图像下标 1 开始，字符串下标 0 开始）
                    ho_OneCodeRegion.Dispose();
                    HOperatorSet.SelectObj(ho_SymbolRegions, out ho_OneCodeRegion, i + 1);

                    HOperatorSet.SmallestRectangle1(ho_OneCodeRegion, out hv_Row1, out hv_Column1,
                        out hv_Row2, out hv_Column2);

                    hv_Label.Dispose();
                    hv_Label = new HTuple((i + 1) + ": " + hv_DecodedDataStrings.TupleSelect(i).S);

                    // 图像坐标 → 窗口坐标，窗口缩放后文字才不会跑偏
                    HOperatorSet.ConvertCoordinatesImageToWindow(hv_WindowHandle, hv_Row1, hv_Column1,
                        out hv_CodeWindowRow, out hv_CodeWindowColumn);
                    HOperatorSet.GetStringExtents(hv_WindowHandle, hv_Label, out hv_Ascent, out hv_Descent,
                        out hv_TextWidth, out hv_TextHeight);

                    // 文字放条码上方，预留 8 像素间距，同时限制在窗口可见范围内
                    hv_TextRow.Dispose();
                    hv_TextRow = (new HTuple(10)).TupleMax2(
                        ((hv_CodeWindowRow - hv_TextHeight) - 8).TupleMin2((hv_WindowHeight - hv_TextHeight) - 10));
                    hv_TextColumn.Dispose();
                    hv_TextColumn = (new HTuple(10)).TupleMax2(
                        hv_CodeWindowColumn.TupleMin2((hv_WindowWidth - hv_TextWidth) - 10));

                    HOperatorSet.DispText(hv_WindowHandle, hv_Label, "window", hv_TextRow, hv_TextColumn,
                        "black", "box", "true");
                }
            }
            finally
            {
                if (ho_OneCodeRegion != null) { ho_OneCodeRegion.Dispose(); }
                // 注意：窗口相关 HTuple（hv_WindowHeight 等）不 Dispose，
                // 里面存的是窗口句柄数值，释放后后续绘制会找不到窗口（之前踩过这个坑）
            }
        }
        #endregion

        #region 私有方法：结果入库（数据库传输）
        #region 提示：写入策略
        // [提示：]【写入策略】一次识别＝一行记录：productName 固定“口红管”，result 记 OK/NG，createTime 记当前时间。
        // [提示：]【长期风险】这张表只增不删：产线全天运行会滚到百万行，而 Output 界面又是全量 ToList 拉取，
        // [提示：]            越跑越慢是必然结果；建议按月分表或定期归档（见问题清单）。
        // [提示：]【容错取舍】写库失败只 Debug.WriteLine，不上抛——识别流程不能被数据库拖死，这个取舍是对的；
        // [提示：]            代价是“库里没数据”时没人知道，现场只能靠 Debug 输出排查。
        // [提示：]【调用点】RunOcr 的成功/失败/异常三分支、TiaoMa 的成功/失败/异常三分支，共 6 处。
        #endregion
        /// <summary>
        /// 把一次识别结果写进数据库（hx_ResultCount 表）：成功记 OK，失败记 NG。
        /// 参数：isOk 本次识别是否成功——OCR（至少 1 个字符）、一维码（至少 1 个码）才算成功。
        /// 说明：数据库写失败（SQL Server 没开、网络断）不影响识别流程继续跑，只留调试痕迹。
        /// </summary>
        private static void ReportResult(bool isOk)                             // 把这次识别结果写一行 hx_ResultCount
        {
            try
            {
                SqlHelp.GetInstance().Insertable(new ResultCountEntity()
                {
                    productName = PRODUCT_NAME,
                    result = isOk ? RESULT_OK : RESULT_NG,
                    createTime = DateTime.Now
                }).ExecuteCommand();
            }
            catch (Exception ex)
            {
                // 关键：入库失败绝不能反过来把视觉检测带崩，更不能在异常分支里再抛一个异常
                // 把原始错误盖掉，所以这里只记录、不上抛
                Debug.WriteLine("[GLOSSED] 识别结果入库失败：" + ex.Message);
            }
        }
        #endregion
    }
}
