# XSao · 线扫识别上位机（逐行注释版）

基于 **C# WinForms + HALCON** 的线扫相机识别上位机，本仓库是原项目 `XSao` 的**逐行注释学习版**：业务代码基本不动，全部加上逐行讲解、逐段设计意图和问题标注，用来看懂"一个工业视觉上位机到底是怎么搭起来的"。

> 这不是一个能直接上产线的成品，是一份带注解的工程样本。生产使用前请自行评估可靠性与安全性，见文末免责声明。

---

## 一、这是什么

原项目为线扫相机 + PLC 触发的字符/条码识别上位机，四个业务页：

| 页面 | 作用 |
|---|---|
| `LoginForm` | 登录校验（账号 + 口令，走 SQL Server 用户表） |
| `HomePage` / `New_HomePage` | 主界面 / 导航容器 |
| `CameraManagement` | 相机取流、PLC 触发拍照、HALCON 识别、结果绘制与入库 |
| `Output` | 按识别结果统计产量（OK/NG 流水） |
| `DateLog` | 操作与识别日志分页查询 |
| `AddUser` / `UserFormCenter` | 用户与权限维护（增删改 + 按钮级权限） |

注释版在原代码上做了三类标注：

- `// [提示：]` —— 解释这行在干什么、上下游是谁、为什么这么写
- `// [优化：]` —— 指出原实现的坑、优化点和验证方法
- `#region 点拨：` / 问答块 —— 针对具体知识点的展开讲解

配套文档：`注释说明.md`（注释规则与阅读顺序）、`优化说明.md`（优化项与未做项的原因）、`问答索引.md`（注释里每个问答的定位表）。

---

## 二、目录结构

```
XSao_注释版/
├─ CameraManagement.cs / Designer   相机页：取流、PLC 触发、识别、绘制
├─ LoginForm / AddUser / UserFormCenter   登录与用户权限
├─ HomePage / New_HomePage / Output / DateLog   导航、产量、日志
├─ Entity/        数据实体（User、Authority、Log、ResultCount、桥表）
├─ VO/            视图对象（用户+权限合并）
├─ Help/          CameraHelp 相机封装、HkServerHelp 海康 SDK、HalconHelp、GLOSSED 识别流程、PlcHelp Modbus、SqlHelp 数据访问
├─ Halcon/        HDevelop 训练/识别程序与已训练模型（GLOSSED_MLP.omc）
├─ Halcon_New/    调整后的 HDevelop 程序
├─ Properties/    程序集信息与资源
└─ *.md           注释说明、优化说明、问答索引
```

---

## 三、运行前提

| 依赖 | 说明 |
|---|---|
| Visual Studio 2022 | 打开 `XSao.slnx`（`.slnx` 是新版解决方案格式；老版本 VS 可直接加 `XSao.csproj`） |
| .NET Framework 4.8 | 目标框架 |
| HALCON | 需要 `halcondotnet.dll`（HALCON 20.x 及以上），放到 `bin\Debug\` |
| 海康 MVS SDK | 需要 `MvCameraControl.Net.dll`，放到 `bin\Debug\` |
| SQL Server | 用户、日志、产量三张流水表所在库 |
| NuGet 包 | SunnyUI 3.9.9、SqlSugar 5.1.4、Newtonsoft.Json 13.0.4、NModbus4、S7netplus 等，见 `packages.config` |

`XSao.csproj` 里用 `HintPath` 引用了上述两个 SDK 的 dll（路径为 `bin\Debug\halcondotnet.dll`、`bin\Debug\MvCameraControl.Net.dll`）。**SDK 的 dll 未随仓库分发**（第三方 SDK 有各自的授权协议），首次运行前请自行安装并拷入，或改 `HintPath` 指向本机安装路径。

---

## 四、快速开始

```bat
git clone <本仓库地址>
cd XSao_注释版

:: 还原 NuGet 包（生成 packages\ 目录）
nuget restore packages.config -PackagesDirectory ..\packages

:: 打开解决方案
start XSao.slnx
```

然后：

1. 把 HALCON 的 `halcondotnet.dll`、海康 MVS 的 `MvCameraControl.Net.dll` 拷贝到 `bin\Debug\`。
2. 打开 `Help\SqlHelp.cs`，把连接串改成自己的库：
   `Server=localhost;Database=XSao_Demo;User Id=demo_user;Password=demo_password;TrustServerCertificate=True;`
3. 建库建表（实体见 `Entity/`：用户表、权限表、桥表、日志表、产量流水表），表名以代码里引用的为准。
4. 相机与 PLC 的地址在 `CameraManagement.cs` 顶部常量与 `Help\PlcHelp.cs` 字段里，改成现场实际值。

---

## 五、脱敏说明

本仓库发布前做过脱敏处理，与原项目源码存在以下差异：

| 位置 | 原值 | 本仓库 |
|---|---|---|
| `Help/SqlHelp.cs` 连接串 | 真实数据库账号与口令 | 占位：`XSao_Demo` / `demo_user` / `demo_password` |
| `Help/SqlHelp.cs` 风险注释 | 直接写出账号口令 | 改为"账号、口令直接硬编码在源码里" |
| `Help/PlcHelp.cs`、`CameraManagement.cs` | 现场 PLC 内网 IP | 示例地址 `192.168.0.10` |
| `CameraManagement.cs` 注释 | 现场 PLC 程序文件名 | 去掉文件名，只保留"现场 PLC 导出程序" |

构建产物（`bin/`、`obj/`、`packages/`）、IDE 状态（`.vs/`）、本机用户配置（`*.user`、`*.suo`）与运行时登录缓存（`userInfo.ini`）均未提交。

---

## 六、已知问题（有意保留）

注释版**没有**修改会改变运行行为的缺陷，只为看懂服务。已知项包括：口令明文存库明文比对、连接串硬编码、`StopCamera` 标志未复位、事务在异常分支未回滚、部分回调未判空、HALCON 图像句柄未释放等。完整清单与优先级见 `优化说明.md` 第三节。

如果你是来学"怎么改"的，这些坑就是练习素材。

---

## 七、许可证

[MIT](LICENSE)。可以自由使用、修改、再分发，保留版权与许可声明即可。

## 八、免责声明

本代码仅作学习参考。作者不对其适用性、可靠性作任何担保；用于产线、生产环境或商业项目所产生的一切后果由使用者自行承担。HALCON、海康 MVS、SunnyUI、SqlSugar 等第三方组件的授权请遵守其各自许可协议。
