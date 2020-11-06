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

## 20-07-29 test-single.js

```bash
$ node test-single.js
[ { cmd: 'dispatch', json: { code: -1, index: -1, total: -1 } } ]
[ { cmd: 'dispatch', json: { code: 1, index: 0, total: 50 } } ]
[ { cmd: 'dispatch', json: { code: 1, index: 1, total: 50 } } ]
[ { cmd: 'dispatch', json: { code: 1, index: 2, total: 50 } } ]
[ { cmd: 'dispatch', json: { code: 1, index: 3, total: 50 } } ]
[ { cmd: 'dispatch', json: { code: 1, index: 4, total: 50 } } ]
[ { cmd: 'dispatch', json: { code: 1, index: 5, total: 50 } } ]
[ { cmd: 'dispatch', json: { code: 1, index: 6, total: 50 } } ]
[ { cmd: 'dispatch', json: { code: 1, index: 7, total: 50 } } ]
[ { cmd: 'dispatch', json: { code: 1, index: 8, total: 50 } } ]
[ { cmd: 'dispatch', json: { code: 1, index: 9, total: 50 } } ]
[ { cmd: 'dispatch', json: { code: 1, index: 10, total: 50 } } ]
[ { cmd: 'dispatch', json: { code: 1, index: 11, total: 50 } } ]
[ { cmd: 'dispatch', json: { code: 1, index: 12, total: 50 } } ]
[ { cmd: 'dispatch', json: { code: 1, index: 13, total: 50 } } ]
[ { cmd: 'dispatch', json: { code: 1, index: 14, total: 50 } } ]
[ { cmd: 'dispatch', json: { code: 1, index: 15, total: 50 } } ]
[ { cmd: 'dispatch', json: { code: 1, index: 16, total: 50 } } ]
[ { cmd: 'dispatch', json: { code: 1, index: 17, total: 50 } } ]
[ { cmd: 'dispatch', json: { code: 1, index: 18, total: 50 } } ]
[ { cmd: 'dispatch', json: { code: 1, index: 19, total: 50 } } ]
[ { cmd: 'dispatch', json: { code: 1, index: 20, total: 50 } } ]
[ { cmd: 'dispatch', json: { code: 1, index: 21, total: 50 } } ]
[ { cmd: 'dispatch', json: { code: 1, index: 22, total: 50 } } ]
[ { cmd: 'dispatch', json: { code: 1, index: 23, total: 50 } } ]
[ { cmd: 'dispatch', json: { code: 1, index: 24, total: 50 } } ]
[ { cmd: 'dispatch', json: { code: 1, index: 25, total: 50 } } ]
[ { cmd: 'dispatch', json: { code: 1, index: 26, total: 50 } } ]
[ { cmd: 'dispatch', json: { code: 1, index: 27, total: 50 } } ]
[ { cmd: 'dispatch', json: { code: 1, index: 28, total: 50 } } ]
[ { cmd: 'dispatch', json: { code: 1, index: 29, total: 50 } } ]
[ { cmd: 'dispatch', json: { code: 1, index: 30, total: 50 } } ]
[ { cmd: 'dispatch', json: { code: 1, index: 31, total: 50 } } ]
[ { cmd: 'dispatch', json: { code: 1, index: 32, total: 50 } } ]
[ { cmd: 'dispatch', json: { code: 1, index: 33, total: 50 } } ]
[ { cmd: 'dispatch', json: { code: 1, index: 34, total: 50 } } ]
[ { cmd: 'dispatch', json: { code: 1, index: 35, total: 50 } } ]
[ { cmd: 'dispatch', json: { code: 1, index: 36, total: 50 } } ]
[ { cmd: 'dispatch', json: { code: 1, index: 37, total: 50 } } ]
[ { cmd: 'dispatch', json: { code: 1, index: 38, total: 50 } } ]
[ { cmd: 'dispatch', json: { code: 1, index: 39, total: 50 } } ]
[ { cmd: 'dispatch', json: { code: 1, index: 40, total: 50 } } ]
[ { cmd: 'dispatch', json: { code: 1, index: 41, total: 50 } } ]
[ { cmd: 'dispatch', json: { code: 1, index: 42, total: 50 } } ]
[ { cmd: 'dispatch', json: { code: 1, index: 43, total: 50 } } ]
[ { cmd: 'dispatch', json: { code: 1, index: 44, total: 50 } } ]
[ { cmd: 'dispatch', json: { code: 1, index: 45, total: 50 } } ]
[ { cmd: 'dispatch', json: { code: 1, index: 46, total: 50 } } ]
[ { cmd: 'dispatch', json: { code: 1, index: 47, total: 50 } } ]
[ { cmd: 'dispatch', json: { code: 1, index: 48, total: 50 } } ]
[ { cmd: 'dispatch', json: { code: 1, index: 49, total: 50 } } ]
[
  { cmd: 'dispatch', json: { code: 1, index: 50, total: 50 } },
  { cmd: 'dispatch', json: { code: 0, index: 50, total: 50 } }
]
```

## 20-11-06 踩坑

- 报错
  ```bash
  
   由于 Exception.ToString() 失败，因此无法打印异常字符串。
  ```
- `AclasSDK.dll` 貌似是 `Debug` 版本的；所以程序也要编译成 `Debug` 版本
- 官方回答
  ```
  demo里的 AclasSDK.dll 也是release版的
  可以改成release试
  ```
