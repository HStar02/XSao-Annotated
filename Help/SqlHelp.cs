using SqlSugar;

namespace XSao.Help
{
    // [提示：]【类级说明】SqlHelp 是全程序唯一的数据库访问入口（SqlSugarClient 工厂）。
    // [提示：]        项目里所有读写库的动作——登录校验、用户增删改、日志分页、产量统计、识别结果入库——
    // [提示：]        都走 SqlHelp.GetInstance()，任何地方都不允许再 new 一个 SqlSugarClient。
    public class SqlHelp
    {
        public SqlHelp() { }
        // [提示：]【字段职责】Instance＝全局唯一客户端；locker＝仅用于创建阶段的互斥对象，本身不存业务数据。
        // [提示：]【理论风险】Instance 未加 volatile：双重检查锁在弱内存模型下可能读到构造未完成的引用。
        // [提示：]            桌面单进程、首访并发极低，实际几乎不会触发；将来若改成多线程高频首访，建议补 volatile（改动需评审）。
        private static SqlSugarClient Instance;
        private static object locker = new object();
        #region 点拨：单例模式
        //【点拨·重点】这是"单例模式"：全程序只创建一个数据库连接。
        //static = 全局只有一份；先判空 + lock 双重检查 = 多线程同时调用也不会建出两个实例。
        // [提示：]【上下游】被 LoginForm / AddUser / DateLog / Output / UserFormCenter / New_HomePage / GLOSSED 调用。
        // [提示：]            AddUser.SaveUser 里 BeginTran→CommitTran/RollbackTran 三步必须落在同一个实例上，
        // [提示：]            单例正是这三步能合成一个事务的前提（各自 new 客户端则事务跨实例断开、回滚失效）。
        // [提示：]【风险】连接串明文写死在源码（账号、口令直接硬编码在源码里），随程序分发等于泄露数据库口令；
        // [提示：]        正式交付应挪到 App.config 并加密，源码只留占位。
        // [提示：]【参数】IsAutoCloseConnection=true：每次操作完自动关连接，避免长连接占满 SQL Server 连接池。
        #endregion
        public static SqlSugarClient GetInstance()   // 取全局唯一的 SqlSugarClient（双重检查锁单例）
        {
            if (Instance == null)
            {
                lock (locker)
                {
                    if (Instance == null)
                    {
                        Instance = new SqlSugarClient(new ConnectionConfig()
                        {
                            ConnectionString = "Server=localhost;Database=XSao_Demo;User Id=demo_user;Password=demo_password;TrustServerCertificate=True;",
                            DbType = DbType.SqlServer,
                            IsAutoCloseConnection = true
                        });
                    }
                }
            }
            return Instance;
        }
    }
}
