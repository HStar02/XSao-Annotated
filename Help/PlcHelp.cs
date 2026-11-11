using Modbus.Device;
using System.Net.Sockets;

namespace XSao.Help
{
    #region 提示：类级说明
    // [提示：]【类级说明】PLC 通信封装：只负责"连接 / 读保持寄存器 / 写保持寄存器"，不判断业务含义。
    // [提示：]        读到 16 代表什么、要不要拍照，都是 CameraManagement 的事，通信类不掺和。
    // [提示：]【为什么从 S7.Net 换成 Modbus】现场老师装的是 NModbus（Modbus TCP），而且原代码读的 "40001"
    // [提示：]        本来就是 Modbus 保持寄存器的编号，S7 协议不认这种写法，硬读只会报地址错误。
    // [提示：]【地址换算（最容易错的地方）】Modbus 的 4xxxx 是"1 基"编号：
    // [提示：]        40001 → 起始地址 0、40002 → 1、40100 → 99。
    // [提示：]        所以代码里一律传偏移值 0，不要传 40001，否则读出来的是 400 号寄存器，完全不是那个点。
    #endregion
    public class PlcHelp
    {
        // [提示：]【状态位】true=链路当前可用，false=没连上/刚掉线。注意它只说"通不通"，不说"读到什么"。
        public bool IsPlc_Connect { get; private set; }

        // [提示：]【配置】现场 IP 还没定，先占个位；真正连线前把 ip 改成和 PLC 一致的值。
        // [提示：]        做成字段+构造函数参数，是为了以后改配置不用动方法签名。
        private string ip = "192.168.0.10"; 
        private int port = 502;          // Modbus TCP 默认端口
        private byte slaveId = 1;        // 从站号：西门子做 Modbus 服务器时常见 1，也可能不是

        private TcpClient tcpClient;
        private IModbusMaster master;

        public PlcHelp()                                                   // 空构造：用字段里的默认 IP/端口/从站号
        {
        }

        // [提示：]【重载构造】现场参数变了用这个传，别去改字段默认值。
        public PlcHelp(string ip, int port, byte slaveId)                  // 构造：用传入参数覆盖 IP/端口/从站号
        {
            this.ip = ip;
            this.port = port;
            this.slaveId = slaveId;
        }

        #region 问题：连接为什么要返回 bool？原来的写法是 while (plcHelp.ConnectPlc())，不是也能跑？
        // 【答：①返回值只说明"这一次连成没成"，不该拿来当循环开关。
        //          原写法 while (ConnectPlc()) 等于每一圈循环都重新 new Plc + Open 一次，
        //          既慢，又会让 PLC 的可用连接被反复占用（PLC 的连接数是有限的，占满后新连接直接被拒）。
        //      ②正确用法：连接只调一次，连不上就返回 false，由调用方退避几秒再试。
        //      ③先 DisConnectPlc 再连：重复 Connect 不关旧 socket，会留下半开连接，
        //          跑几个小时后表现为"突然连不上了"。
        //      影响范围：CameraManagement 的 PLC 轮询线程只在 IsPlc_Connect 为 false 时才调它。】
        #endregion
        public bool ConnectPlc()                                           // 连接PLC（先断开旧连接再连）
        {
            try
            {
                DisConnectPlc();                       // 先清旧连接，防止 socket 泄漏
                tcpClient = new TcpClient();
                tcpClient.Connect(ip, port);           // 同步连接，必须在后台线程调，别放 UI 线程
                master = ModbusIpMaster.CreateIp(tcpClient);
                master.Transport.ReadTimeout = 500;    // 读超时 500ms：PLC 掉线时不会让线程卡死
                master.Transport.WriteTimeout = 500;
                IsPlc_Connect = tcpClient.Connected;
                return IsPlc_Connect;
            }
            catch
            {
                // [提示：]通信层吞异常、只回 false：要不要弹提示是 UI 的事，通信类不依赖 UI。
                IsPlc_Connect = false;
                return false;
            }
        }

        // [提示：]【断开】关 socket、清状态。窗体关闭时必须调，否则 PLC 侧会一直显示"连接被占用"。
        public void DisConnectPlc()                                        // 断开PLC并清空连接状态
        {
            try
            {
                if (master != null)
                {
                    master.Dispose();
                }
            }
            catch
            {
            }
            master = null;
            try
            {
                if (tcpClient != null)
                {
                    tcpClient.Close();
                }
            }
            catch
            {
            }
            tcpClient = null;
            IsPlc_Connect = false;
        }

        #region 问题：为什么把原来的 ReadPlc(string) 换成 TryReadUShort，而不是继续用 404 当错误码？
        // 【答：①404 本来是网页的"找不到"，被借来当错误码就会和真实数据撞车：
        //          真的读到 404 和"根本没连上"在调用方眼里一模一样，没法区分。
        //      ②改成 bool TryReadUShort(地址, out 值)：成功与否用 bool，数据用 out，两件事分开表达。
        //      ③读失败时顺手把 IsPlc_Connect 置 false：读崩基本等于链路坏了，
        //          让轮询线程下一轮去重连，而不是每 10ms 报一次错把日志刷爆。
        //      影响范围：CameraManagement 用它读触发点（传偏移 0 对应 40001）。】
        #endregion
        public bool TryReadUShort(ushort startAddress, out ushort value)   // 读保持寄存器（功能码03），失败返回 false
        {
            value = 0;
            if (!IsPlc_Connect || master == null)
            {
                return false;
            }
            try
            {
                // [提示：]ReadHoldingRegisters(从站号, 起始地址, 寄存器个数)：读 4xxxx 保持寄存器，功能码 03
                ushort[] registers = master.ReadHoldingRegisters(slaveId, startAddress, 1);
                if (registers == null || registers.Length < 1)
                {
                    return false;
                }
                value = registers[0];
                return true;
            }
            catch
            {
                IsPlc_Connect = false;
                return false;
            }
        }

        // [提示：]【写单个寄存器】功能码 06。业务上"暂不回 PLC"，接口先留着：
        // [提示：]        以后要回 OK/NG 或完成位，直接用它，不用再改通信层。
        public bool TryWriteUShort(ushort startAddress, ushort value)      // 写单个寄存器（功能码06，备用）
        {
            if (!IsPlc_Connect || master == null)
            {
                return false;
            }
            try
            {
                master.WriteSingleRegister(slaveId, startAddress, value);
                return true;
            }
            catch
            {
                IsPlc_Connect = false;
                return false;
            }
        }
    }
}
