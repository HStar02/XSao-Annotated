using HalconDotNet;
using MvCameraControl;
using System;

namespace XSao.Help
{
    public class HalconHelp
    {
        private HalconHelp()                                                    // 私有构造：单例，只能走 Instance
        {

        }
        public static HalconHelp Instance { get; private set; } = new HalconHelp();
        #region 问题：私有的构造方法 HalconHelp() 与 Instance 属性这两行合起来实现了什么？
        // 【答：这是饿汉式单例三件套，目的是保证全项目只有一个 HalconHelp 对象：
        //       ①private HalconHelp() {}：构造方法设为私有，外部 new 不出来（编译期就拦住），从根上杜绝多实例；
        //       ②public static HalconHelp Instance { get; private set; } = new HalconHelp()：类加载时立刻建好唯一实例；
        //       ③private set：外部只能读 Instance，不能把它改成 null 或换成一个新对象。
        //       使用方式：HalconHelp.Instance.TryCreateHalconImage(...)（见 CameraManagement.DisplayImage）。
        //       为什么要单例：这个类没有状态，只提供“相机图 → Halcon 图”的转换，重复 new 纯属浪费。
        //       与 SqlHelp 的差别：SqlHelp 是懒汉式 + 双重检查锁（第一次用时才建），本类是饿汉式（静态字段直接初始化）。
        //       影响范围：全程序共享同一个转换器；它没有成员变量，所以也不存在并发共享状态的风险。】
        #endregion

        #region 提示：上下游
        // [提示：]【上下游】上游：CameraManagement.DisplayImage 传入相机回调里克隆的帧（help.frameForSave.Image）。
        // [提示：]            下游：输出的 HObject 交给 HalconOCR → GLOSSED.TiaoMa 做识别与绘制，用完由调用方 Dispose。
        // [提示：]【关键约束】GenImage1 / GenImageInterleaved 只是把一块内存指针包成 Halcon 图像，不复制像素：
        // [提示：]            所以返回的图像必须在源 IImage 存活期间使用；相机缓冲一被 SDK 回收，画面立刻变花甚至崩溃。
        // [提示：]            它属于借用视图，不能跨帧长期缓存，也不能在后台线程释放。
        #endregion
        /// <summary>
        /// 尝试将 MvCameraControl.IImage 转换为 Halcon image 图像
        /// </summary>
        /// <param name="image"></param>
        /// <param name="halconImage"></param>
        /// <returns></returns>
        public bool TryCreateHalconImage(IImage image, out HObject halconImage)  // 相机帧转 HObject（按像素格式选接口）
        {

            // 初始化输出的 Halcon 图像为 null
            halconImage = null;

            // 校验：图像对象为空 或 像素数据指针为空 → 无法转换，直接返回失败
            if (image == null || image.PixelDataPtr == IntPtr.Zero)
            {
                return false;
            }

            // 获取图像的宽和高（强制转换为 int 类型）
            int width = (int)image.Width;
            int height = (int)image.Height;

            #region 提示：业务判断
            // [提示：]【业务判断】按像素格式选建图算子，是这张图能不能识别出的第一道关：
            // [提示：]            8 位灰度/Bayer → GenImage1(“byte”)；10~16 位 → GenImage1(“uint2”)；
            // [提示：]            RGB/BGR 打包 → GenImageInterleaved（需额外告知通道交错方式）。
            // [提示：]【风险】Bayer 数据被当单通道灰度直接建图，画面会呈马赛克/伪彩，阈值分割的灰度也随之不稳；
            // [提示：]            若要稳定识别彩色相机图，应先做 Bayer→RGB 转换再进算法（当前实现不做，属已知简化）。
            // [提示：]【兜底】default 返回 false：遇到未支持的格式不会静默出错，调用方会收到“转换图片失败”。
            #endregion
            switch (image.PixelType)
            {
                case MvGvspPixelType.PixelType_Gvsp_Mono8:
                case MvGvspPixelType.PixelType_Gvsp_BayerGR8:
                case MvGvspPixelType.PixelType_Gvsp_BayerRG8:
                case MvGvspPixelType.PixelType_Gvsp_BayerGB8:
                case MvGvspPixelType.PixelType_Gvsp_BayerBG8:
                case MvGvspPixelType.PixelType_Gvsp_BayerRBGG8:
                    /*
                         2. 每个参数到底什么意思？
                        ① out HObject halconImage
                        输出参数
                        函数执行成功后，会自动创建一个 Halcon 图像，并赋值给这个变量
                        你后面才能用它显示、处理、保存
                        ② "byte" → 最重要！
                        表示每个像素的数据类型：
                        byte = 8 位无符号整数（0~255）
                        适用于：
                        Mono8 黑白相机
                        BayerGR8 / RG8 / GB8 / BG8 彩色相机
                        8 位灰度图
                        如果是 16 位图像，就写 "uint2"。
                     */
                    HOperatorSet.GenImage1(
                        out halconImage,    // 输出：创建好的 Halcon 图像
                        "byte",             // 图像类型（每个像素占 8 位）
                        width,              // 图像宽度
                        height,             // 图像高度
                        image.PixelDataPtr  // 像素数据的内存指针
                    );
                    //HOperatorSet.GenImage1(out halconImage, "byte", width, height, image.PixelDataPtr);
                    return true;

                case MvGvspPixelType.PixelType_Gvsp_Mono10:
                case MvGvspPixelType.PixelType_Gvsp_Mono12:
                case MvGvspPixelType.PixelType_Gvsp_Mono14:
                case MvGvspPixelType.PixelType_Gvsp_Mono16:
                case MvGvspPixelType.PixelType_Gvsp_BayerGR10:
                case MvGvspPixelType.PixelType_Gvsp_BayerRG10:
                case MvGvspPixelType.PixelType_Gvsp_BayerGB10:
                case MvGvspPixelType.PixelType_Gvsp_BayerBG10:
                case MvGvspPixelType.PixelType_Gvsp_BayerGR12:
                case MvGvspPixelType.PixelType_Gvsp_BayerRG12:
                case MvGvspPixelType.PixelType_Gvsp_BayerGB12:
                case MvGvspPixelType.PixelType_Gvsp_BayerBG12:
                case MvGvspPixelType.PixelType_Gvsp_BayerGR16:
                case MvGvspPixelType.PixelType_Gvsp_BayerRG16:
                case MvGvspPixelType.PixelType_Gvsp_BayerGB16:
                case MvGvspPixelType.PixelType_Gvsp_BayerBG16:
                    HOperatorSet.GenImage1(out halconImage, "uint2", width, height, image.PixelDataPtr);
                    return true;
                case MvGvspPixelType.PixelType_Gvsp_RGB8_Packed:
                    //GenImageInterleaved = 专门用来创建【RGB/BGR 连续打包】的彩色图像
                    //RGB8 打包格式（最常见）
                    HOperatorSet.GenImageInterleaved(
                        out halconImage,     // 输出：创建好的Halcon彩色图像
                        image.PixelDataPtr,  // 像素数据内存指针
                        "rgb",               // ✅ 颜色排列顺序：R → G → B
                        width,               // 图像宽度
                        height,              // 图像高度
                        0,                   // 行对齐：0 = 每行开头无额外填充
                        "byte",              // 每个通道是8位（0~255）
                        width,               // 输入图像每行宽度（和宽度一致）
                        height,              // 输入图像高度
                        0,                   // 起始偏移：从第0个像素开始
                        0,                   // 无额外填充
                        -1,                  // 自动计算通道数
                        0                    // 保留参数，固定0
                    );
                    return true;

                //BGR8 打包格式（OpenCV 常用）
                case MvGvspPixelType.PixelType_Gvsp_BGR8_Packed:
                    HOperatorSet.GenImageInterleaved(
                        out halconImage,
                        image.PixelDataPtr,
                        "bgr",               // ✅ 颜色排列顺序：B → G → R
                        width,
                        height,
                        0,
                        "byte",
                        width,
                        height,
                        0,
                        0,
                        -1,
                        0
                    );
                    return true;

                default:
                    return false;


            }
        }
    }
}
