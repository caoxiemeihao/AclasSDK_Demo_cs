using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Text.RegularExpressions;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Threading;

namespace AclasFor_node_SingleFile
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = Encoding.UTF8;

            Dictionary<string, string> dict = Utils.QueryString2Dict(args[0]);

            if (!dict.ContainsKey("host") || !dict.ContainsKey("filename") || !dict.ContainsKey("type"))
            {
                Utils.StdoutToNodeJs("error", new StateMsg(11, null,
                    "入参错误: [host, filename, type] 必须传入！\n传入参数: " + args[0]));
                return;
            }

            AclasSDK_Args aclasArgs = new AclasSDK_Args(
                dict["host"],
                Convert.ToUInt32(dict["type"]),
                dict["filename"]);

            // Aclas.StartTask(aclasArgs); // 20-07-29
            // 如果直接运行；在 electron 主进程中下发到 45 条后直接退出，还不报错！
            // 放在单独线程中运行即可解决 45 条意外退出问题
            Thread thread = new Thread(new ParameterizedThreadStart(Aclas.StartTask));
            thread.Start(aclasArgs);
        }

    }

    class StateMsg
    {
        public string msg;
        public int code;
        public string data;

        public StateMsg(int code = 0, string data = "", string msg = "")
        {
            this.code = code;
            this.data = data == null ? "null" : data;
            this.msg = msg == null ? "null" :  msg;
        }

        public override string ToString()
        {
            Dictionary<string, string> dict = new Dictionary<string, string>();
            dict["code"] = code.ToString();
            dict["data"] = data;
            dict["msg"] = msg;
            return Utils.Dict2JsonStr(dict);
        }
    }

    class Utils
    {
        // 给 Nodejs 通讯的写法
        public static void StdoutToNodeJs(string cmd, object data)
        {
            // 约定 ##数据## 格式
            Console.WriteLine("##{0}={1}##", cmd, data.ToString());
        }

        // 字典转 json 字符串
        public static string Dict2JsonStr(Dictionary<string, string> dict)
        {
            string val = "";
            foreach (KeyValuePair<string, string> item in dict)
            {
                bool isNumber = new Regex(@"^-?\d+$").IsMatch(item.Value);
                bool isBool = new Regex(@"^(true|false)$").IsMatch(item.Value);
                bool isNullStr = new Regex(@"^(null)$").IsMatch(item.Value);
                if (isNumber || isBool || isNullStr)
                    val += "\"" + item.Key + "\":" + item.Value + ",";
                else
                    val += "\"" + item.Key + "\":\"" + item.Value + "\",";
            }
            if (val.Length > 0)
            {
                val = val.Substring(0, val.Length - 1); // 去掉尾部的 ,
            }
            return "{" + val + "}";
        }

        // 解析 Query String
        public static Dictionary<string, string> QueryString2Dict(/*原始 http 字符串*/string querystring)
        {
            string[] tmpArr = querystring.Split('&');
            Dictionary<string, string> dict = new Dictionary<string, string>();
            for (int x = 0; x < tmpArr.Length; x++)
            {
                string[] tmp = tmpArr[x].Split('=');
                if (tmp.Length > 1)
                {
                    // UrlDecode
                    tmp[1] = Uri.UnescapeDataString(tmp[1]);
                    dict.Add(tmp[0], tmp[1]);
                };
            }
            return dict;
        }

    }


    // 下发部分
    /******************************************************************************/
    class ResponseAclas
    {
        public int code;
        public int index;
        public int total;

        public ResponseAclas(int code, int index = -1, int total = -1)
        {
            this.code = code;
            this.index = index;
            this.total = total;
        }

        public override string ToString()
        {
            Dictionary<string, string> dict = new Dictionary<string, string>();
            dict["code"] = code.ToString();
            dict["index"] = index.ToString();
            dict["total"] = total.ToString();
            return Utils.Dict2JsonStr(dict);
        }
    }

    class AclasSDK_Args
    {
        public string host; // 电子称 IP
        public uint dataType; // 下发类型
        public string fileName; // 下发文件路径

        public AclasSDK_Args(string host, uint dataType, string fileName)
        {
            this.host = host;
            this.dataType = dataType;
            this.fileName = fileName;
        }
    }

    partial class Aclas
    {
        const string DISPATCH_CMD = "dispatch";

        const string LibraryName = "AclasSDK.dll";
        // Success
        const int ASSDK_Err_Success = 0x0000;
        // Progress
        const int ASSDK_Err_Progress = 0x0001;
        // Terminate by hand
        const int ASSDK_ERR_Terminate = 0x0002;

        // ProtocolType
        const int ASSDK_ProtocolType_None = 0;
        const int ASSDK_ProtocolType_Pecr = 0;
        const int ASSDK_ProtocolType_Hecr = 2;
        const int ASSDK_ProtocolType_TSecr = 3;

        // ProcType
        const int ASSDK_ProcType_Down = 0;
        const int ASSDK_ProcType_UP = 1;
        const int ASSDK_ProcType_Edit = 2;
        const int ASSDK_ProcType_Del = 3;
        const int ASSDK_ProcType_List = 4;
        const int ASSDK_ProcType_Empty = 5;
        const int ASSDK_ProcType_Reserve = 0x0010;

        // DataType
        const int ASSDK_DataType_PLU = 0x0000;
        const int ASSDK_DataType_Unit = 0x0001;
        const int ASSDK_DataType_Department = 0x0002;
        const int ASSDK_DataType_HotKey = 0x0003;
        const int ASSDK_DataType_Group = 0x0004;
        const int ASSDK_DataType_Discount = 0x0005;
        const int ASSDK_DataType_Origin = 0x0006;
        const int ASSDK_DataType_Country = 0x0007;
        const int ASSDK_DataType_SlaughterHouse = 0x0008; // 屠宰场 😥
        const int ASSDK_DataType_Cuttinghall = 0x0009; // 切割大厅
        const int ASSDK_DataType_Tare = 0x000A; // 皮重
        const int ASSDK_DataType_Nutrition = 0x000B; // 营养
        const int ASSDK_DataType_Note1 = 0x000C;
        const int ASSDK_DataType_Note2 = 0x000D;
        const int ASSDK_DataType_Note3 = 0x000E;
        //const int ASSDK_DataType_TextMessage = 0x000F;
        const int ASSDK_DataType_Options = 0x0010;
        const int ASSDK_DataType_CustomBarcode = 0x0011;
        const int ASSDK_DataType_LabelPrintRecord = 0x0012;
        const int ASSDK_DataType_HeaderInfo = 0x0013;
        const int ASSDK_DataType_FooterInfo = 0x0014;
        const int ASSDK_DataType_AdvertisementInfo = 0x0015; // 广告宣传
        const int ASSDK_DataType_HeaderLogo = 0x0016;
        const int ASSDK_DataType_FooterLogo = 0x0017;
        const int ASSDK_DataType_LabelAdvertisement = 0x0018;
        const int ASSDK_DataType_VendorInfo = 0x0019;
        const int ASSDK_DataType_NutritionElement = 0x001A;
        const int ASSDK_DataType_NutritionInfo = 0x001B;
        const int ASSDK_DataType_Note4 = 0x001C;

        // DeviceInfo
        [StructLayoutAttribute(LayoutKind.Sequential, Pack = 1)]
        public struct TASSDKDeviceInfo
        {
            public UInt32 ProtocolType; // ProtocolType
            public UInt32 Addr;
            public UInt32 Port;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)]
            public byte[] Name;
            public UInt32 ID;
            public UInt32 Version;
            public Byte Country;
            public Byte DepartmentID;
            public Byte KeyType;
            public UInt64 PrinterDot;
            public long PrnStartDate;
            public UInt32 LabelPage;
            public UInt32 PrinterNo;
            public UInt16 PLUStorage;
            public UInt16 HotKeyCount;
            public UInt16 NutritionStorage;
            public UInt16 DiscountStorage;
            public UInt16 Note1Storage;
            public UInt16 Note2Storage;
            public UInt16 Note3Storage;
            public UInt16 Note4Storage;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 12)]
            public byte[] FirmwareVersion;//固件版本
            public Byte DefaultProtocol;//默认协议            
            public Byte LFCodeLen;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
            public byte[] DeviceId; //档口号
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
            public byte[] StockId;//门店号
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 155)]
            public byte[] Adjunct;// 保留参数
        }

        [UnmanagedFunctionPointerAttribute(CallingConvention.StdCall)]
        public delegate void TASSDKOnProgressEvent(uint nErrorCode, uint Index, uint Total, IntPtr lpUserData);

        [DllImport(LibraryName)]
        static public extern Boolean AclasSDK_Initialize(Pointer Adjuct = null);

        [DllImport(LibraryName)]
        static public extern void AclasSDK_Finalize();

        [DllImport(LibraryName, CharSet = CharSet.Ansi)]
        static public extern Boolean AclasSDK_GetDevicesInfo(UInt32 Addr, UInt32 Port, UInt32 ProtocolType, ref TASSDKDeviceInfo DeviceInfo);

        [DllImport(LibraryName, CharSet = CharSet.Ansi)]
        static public extern int AclasSDK_GetNetworkSectionDevicesInfo(UInt32 Addr, UInt32 Port, UInt32 ProtocolType, IntPtr lpDeviceInfos, UInt32 dwCount);

        [DllImport(LibraryName)]
        static public extern IntPtr AclasSDK_ExecTaskA(UInt32 Addr, UInt32 Port, UInt32 ProtocolType, UInt32 ProcType, UInt32 DataType, string FileName, TASSDKOnProgressEvent OnProgress, Pointer lpUserData);

        [DllImport(LibraryName, CallingConvention = CallingConvention.StdCall)]
        static public extern IntPtr AclasSDK_ExecTask(UInt32 Addr, UInt32 Port, UInt32 ProtocolType, UInt32 ProcType, UInt32 DataType, string FileName, TASSDKOnProgressEvent OnProgress, Pointer lpUserData);

        [DllImport(LibraryName)]
        static public extern IntPtr AclasSDK_ExecTaskW(UInt32 Addr, UInt32 Port, UInt32 ProtocolType, UInt32 ProcType, UInt32 DataType, string FileName, TASSDKOnProgressEvent OnProgress, Pointer lpUserData);

        [DllImport(LibraryName)]
        static public extern int AclasSDK_GetLastTaskError();

        [DllImport(LibraryName)]
        static public extern void AclasSDK_StopTask(IntPtr TaskHandle);

        [DllImport(LibraryName)]
        static public extern void AclasSDK_WaitForTask(IntPtr TaskHandle);

        static uint MakeHostToDWord(string sHost)
        {
            int i;
            string[] Segment;
            uint result;
            result = 0;

            Segment = sHost.Split('.');
            if (Segment.Length != 4)
                return result;
            for (i = 0; i < (Segment.Length); i++)
            {
                if ((Convert.ToUInt32(Segment[i]) >= 0) && (Convert.ToUInt32(Segment[i]) <= 255))
                {
                    result = result + Convert.ToUInt32(Convert.ToUInt32(Segment[i]) << ((3 - i) * 8));
                }
                else
                    return result;
            }
            return result;
        }

        public static void OnProgressEvent(UInt32 nErrorCode, UInt32 Index, UInt32 Total, IntPtr lpUserData)
        {
            ResponseAclas res = new ResponseAclas(
                Convert.ToInt32(nErrorCode),
                Convert.ToInt32(Index),
                Convert.ToInt32(Total));

            Utils.StdoutToNodeJs(DISPATCH_CMD, res);

            if (nErrorCode == ASSDK_Err_Success)
            {
                //Console.WriteLine("下发成功");
            }
            else if (nErrorCode == ASSDK_Err_Progress)
            {
                //Console.WriteLine("{0}/{1}", Index, Total);
            }
            else if (nErrorCode == ASSDK_ERR_Terminate)
            {
                //Console.WriteLine("主动结束");
            }
            else
            {
                //Console.WriteLine("下发失败 {0}", nErrorCode);
            }
        }

        public static void StartTask(object param)
        {
            AclasSDK_Args args = (AclasSDK_Args)param;
            AclasSDK_Initialize(null);

            uint iAddr = MakeHostToDWord(args.host);
            uint DataType = args.dataType;

            TASSDKDeviceInfo DeviceInfo = new TASSDKDeviceInfo();
            // 链接不上 bInfo = False；剩的去 ping 了
            // 链接超时时间、重试次数在 AclasSDK.ini 中配置
            // GetDeviceTimeOut=2000 + GetDeviceTryCount=2 --> 10 秒超时时间
            bool boolInfo = AclasSDK_GetDevicesInfo(iAddr, 0, ASSDK_ProtocolType_None, ref DeviceInfo);
            if (boolInfo)
            {
                Utils.StdoutToNodeJs(DISPATCH_CMD, new ResponseAclas(-1)); // 开始

                TASSDKOnProgressEvent OnProgress = new TASSDKOnProgressEvent(OnProgressEvent);

                AclasSDK_WaitForTask(AclasSDK_ExecTaskA(DeviceInfo.Addr, DeviceInfo.Port, DeviceInfo.ProtocolType, ASSDK_ProcType_Down, DataType, args.fileName, OnProgress, null));

                // 执行结束，直接释放
                AclasSDK_Finalize();
            }
            else
            {
                Utils.StdoutToNodeJs(DISPATCH_CMD, new ResponseAclas(501)); // 链接不上
            }

        }

    }
    /******************************************************************************/

}
