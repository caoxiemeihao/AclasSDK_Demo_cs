using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;

using System.Runtime;
using System.IO;
using System.Threading;


namespace AclasFor_node
{
    // Ajax 服务器回调
    internal delegate string FeedbackAjax(Dictionary<string, string> dict);
    // 下发电子称回调
    internal delegate void FeedbackAclas(CMD_ACLAS cmd, ResponseAclas res);

    class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = Encoding.UTF8;
                
            // TestReadIni();
            // TestHttpServer();
            RunAjaxServer();

            //string host = "192.168.1.3";
            //uint dataType = 0x0000;
            //string deskTop = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            //string fileName = Path.Combine(deskTop, "plu.txt");
            //RunAclas(new AclasSDK_Args(host, dataType, fileName));
        }

        static string FeedbackAjax(Dictionary<string, string> dict)
        {
            string now = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            State.queryStringDict = dict;

            if (!dict.ContainsKey("cmd"))
            {
                return new ResponseAjax("", 1, "Parameter \"cmd\" is necessary.").ToString();
            }

            if (dict["cmd"] == "start") // 下发
            {
                if (State.processing) return new ResponseAjax("", 11, "The current task is not completed.").ToString();

                string host = "";
                uint dataType = 0x000;
                string fileNmae = "";
                if (dict.ContainsKey("host")) host = dict["host"];
                if (dict.ContainsKey("file")) fileNmae = dict["file"];
                if (host.Length == 0)
                {
                    return new ResponseAjax("", 1, "Parameter \"host\" is necessary.").ToString();
                } else if (fileNmae.Length == 0)
                {
                    return new ResponseAjax("", 1, "Parameter \"file\" is necessary.").ToString();
                }

                // 启动下载线程
                Thread thread = new Thread(new ParameterizedThreadStart(RunAclas));
                thread.Start(new AclasSDK_Args(host, dataType, fileNmae));
                State.resAclas = new ResponseAclas(-1, -1, -1);
                State.processing = true;

                StdoutToNodeJs("start", State.ToJsonStr());

                return new ResponseAjax("", 0, "Start:" + host).ToString();
            }
            else if (dict["cmd"] == "state") // 获取当前状态
            {
                return new ResponseAjax(State.ToJsonStr()).ToString();
            }

            return new ResponseAjax(now, 0, "Not defined cmd:" + dict["cmd"]).ToString();
        }

        static void FeedbackAclas(CMD_ACLAS cmd, ResponseAclas res)
        {
            // 会扰乱 stdout.on('data') 的解析
            //Console.WriteLine("[{0}:{1}] {2}/{3}", cmd, res.code, res.index, res.total);
            State.resAclas = res;
            State.processing = res.code == 1; // 1 正常进程

            StdoutToNodeJs("dispatch", State.ToJsonStr());
        }

        // 测试方法
        static void TestReadIni()
        {
            string initPath = Environment.CurrentDirectory + "\\config.ini";
            Console.WriteLine(initPath);
            string str = Helper.ReadString("host", "host", "host", initPath);
            Console.ReadKey();
        }
        // 测试方法
        static void TestHttpServer() {
            TestHttpServer http = new TestHttpServer();
            http.StartServer();
        }

        static void RunAjaxServer()
        {
            AjaxServer ajax = new AjaxServer();
            ajax.StartAjaxServer(new FeedbackAjax(FeedbackAjax));
        }

        static void RunAclas(object args)
        {
            Aclas.StartTask(new FeedbackAclas(FeedbackAclas), (AclasSDK_Args)args);
        }

        // 给 Nodejs 通讯的写法
        public static void StdoutToNodeJs(string cmd, string data)
        {
            // 约定 ##数据## 格式
            Console.WriteLine("##{0}={1}##", cmd, data);
        }
    }

    // 中中转数据
    class State
    {
        public static ResponseAclas resAclas;
        public static bool processing = false;
        public static Dictionary<string, string> queryStringDict;

        public static string ToJsonStr()
        {
            Dictionary<string, string> d = new Dictionary<string, string>();
            d["processing"] = State.processing ? "true" : "false";
            d["code"] = State.resAclas.code.ToString();
            d["index"] = State.resAclas.index.ToString();
            d["total"] = State.resAclas.total.ToString();
            // d["param"] = AjaxServer.Dict2JsonStr(State.queryStringDict);

            return AjaxServer.Dict2JsonStr(d);
        }
    }

    enum CMD_ACLAS
    {
        下发中 = 1,
        成功 = 0,
        失败 = 3,
    }
}

/* exports.code_dict = {
  256: '已初始化',
  257: '未初始化',
  258: '设备不存在',
  259: '不支持的协议类型',
  260: '该数据类型不支持此操作',
  261: '该数据类型不支持',
  264: '无法打开输入文件',
  265: '字段数与内容数不匹配',
  266: '通讯数据异常',
  267: '解析数据异常',
  268: 'CodePage错误',
  269: '无法创建输出文件',

  0: 'sucessed',
  1: 'processing',

  403: '[链接超时][默认40秒]',
  404: '[LoadLibrary][未加载到AclasSDK.dll]',
  405: 'polyfill addons.',
  406: '报错 子进程拉起多次失败',
  501: 'ping 超时',
  502: 'ping 不通',
} */
