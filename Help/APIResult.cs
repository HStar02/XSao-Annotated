namespace XSao.Help
{
    #region 提示：类级说明
    //<T>一个一个测试，string，bool，int，List<string>---1029-计划一
    // [提示：]【类级说明】APIResult<T>＝统一返回信封：IsSuccess（成没成）+ Message（给人看的话）+ data（真正的数据）。
    // [提示：]        用途：把“方法失败”从“抛异常”改为“返回对象”，调用方用 if(!result.IsSuccess) 处理，界面不必 try/catch。
    // [提示：]        上游：HkServerHelp 的 GetDevice/OpenDevice/StartGrabbing/StopCamera 全部返回它；
    // [提示：]        下游：CameraManagement 逐个判 IsSuccess、取 Message 弹提示并写日志。
    // [提示：]        泛型 T 就是这个信封里装的数据类型：List<string>（设备列表）、bool（开关动作）等。
    #endregion
    public class APIResult<T>
    {
        public bool IsSuccess { get; set; }

        public string Message { get; set; }

        public T data { get; set; }
        #region 提示：命名
        // [提示：]【命名】data 用小写不符合 .NET 公共成员命名规范（应为 Data）；
        // [提示：]        但它与 IsSuccess/Message 并列当信封字段用，改名会波及所有调用点，暂不动（见问题清单低优先级项）。
        //T是一个泛型，什么类型都可以，先定义规范，然后下面用什么
        //什么都可以
        #endregion
        public static APIResult<T> Success()                 // 成功信封（不带数据）
        {
            return new APIResult<T>() { IsSuccess = true };
        }
        public static APIResult<T> Success(T data)           // 成功信封：把业务数据装进 data
        {
            return new APIResult<T>() { IsSuccess = true, data = data };
        }
        #region 问题：APIResult<T> 里 Success(T data) 与 Success(string message) 两个重载的本质区别是什么？
        // 【答：本质是同一个方法名按实参类型分派，两者都是静态工厂，返回的都是 IsSuccess=true 的新对象：
        //       ①Success(T data)：把业务数据装进 data 字段，Message 保持 null；
        //       ②Success(string message)：把提示文字装进 Message，data 保持默认值。
        //       编译器按实参的静态类型选版本：传 List<string> → 命中 T 版；传字符串且 T≠string → 命中 string 版。
        //       依据：C# 重载决议优先无需装箱、无需额外转换的候选。
        //       影响范围：调用方若写 APIResult<string>.Success(“xx”)，此时 T 就是 string，
        //       两个重载的参数类型完全相同 → 编译器报 CS0121“调用不明确”，属潜伏隐患。
        //       本项目现有调用（APIResult<List<string>>.Success(list)、APIResult<bool>.Success(“连接成功”)）都不触发。
        //       验证方式：临时加一行 APIResult<string>.Success(“t”) 编译，会立刻报二义性，删掉即恢复。】
        #endregion
        public static APIResult<T> Success(string message)   // 成功信封：只带一句提示
        {
            return new APIResult<T>() { IsSuccess = true, Message = message };
        }
        // [提示：]【调用约定】Error 系列只把 IsSuccess 置 false，别的什么都不做；
        // [提示：]            所以调用方必须先判 IsSuccess 再读 Message/data——无参 Error() 的 Message 为 null，
        // [提示：]            界面若直接 Show(Message) 会弹出空白提示框（CameraManagement 里就存在这种路径）。
        public static APIResult<T> Error()                   // 失败信封（不带提示）
        {
            return new APIResult<T>() { IsSuccess = false };
        }
        public static APIResult<T> Error(string message)     // 失败信封：带失败原因
        {
            return new APIResult<T>() { IsSuccess = false, Message = message };
            #region 问题：在 Error(string message) 里给 Message 赋值，为什么不会把这个属性“写死”、影响到别处的调用？
            //这里Message不会把        public string Message { get; set; }，写死，只在这个方法里面，
            //其他地方调用的时候，会根据这个方法返回的Message
            //本身public string Message { get; set; }一直存在
            // 【答：因为属性只是类型上的一个格子定义，每个 new 出来的对象各有一份自己的格子：
            //      ①public string Message { get; set; } 只声明“有这么一个可读写字段”，本身不带任何值；
            //      ②Error(“xx”) 每次都 new APIResult<T>()，把 “xx” 装进这个新对象的格子里再返回；
            //      ③别处 Error(“yy”) 装在另一个新对象的格子里，两边互不影响。
            //      所谓不会写死，指的是：格子定义只有一份，格子里的值随对象走。
            //      依据：C# 实例字段/属性属于对象，不属于类型。
            //      验证方式：var a=APIResult<int>.Error(“A”); var b=APIResult<int>.Error(“B”); 打印 a.Message 与 b.Message，分别是 A 和 B。】
            //【点拨】属性只是"储物格"，本身没值；每次Error("xxx")调用都把"xxx"装进【那个新盒子】的格子里。
            //        所以不会写死——不同地方调用各装各的消息，格子定义只有一份。
            #endregion
        }

    }
}
