## Aclas 顶尖电子称 C# 版本；http 服务器、下发 插件


## 未调用 AclasSDK_Initialize
```bash
System.BadImageFormatException
  HResult=0x8007000B
  Message=试图加载格式不正确的程序。 (异常来自 HRESULT:0x8007000B)
  Source=AclasFor_node
  StackTrace:
   at AclasFor_node.Aclas.AclasSDK_GetDevicesInfo(UInt32 Addr, UInt32 Port, UInt32 ProtocolType, TASSDKDeviceInfo& DeviceInfo)
   at AclasFor_node.Aclas.StartTask(FeedbackAclas feedback, AclasSDK_Args args) in D:\ypsx\AclasSDK_Demo_cs\AclasFor_node\Aclas.cs:line 228
   at AclasFor_node.Program.TestAclas(AclasSDK_Args args) in D:\ypsx\AclasSDK_Demo_cs\AclasFor_node\Program.cs:line 77
   at AclasFor_node.Program.Main(String[] args) in D:\ypsx\AclasSDK_Demo_cs\AclasFor_node\Program.cs:line 42
```
- 将构建选项-平台AnyCpu 改成 x86 (20-07-18)

