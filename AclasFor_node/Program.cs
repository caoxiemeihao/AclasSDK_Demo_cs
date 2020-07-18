using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;

using System.Runtime;
using System.IO;


namespace AclasFor_node
{
    internal delegate string FeedbackAjax(Dictionary<string, string> dict);

    enum CMD_ACLAS
    {
        下发中 = 1,
        成功 = 0,
        失败 = 3,
    }
    internal delegate void FeedbackAclas(CMD_ACLAS cmd, ResponseAclas res);
    //exports.code_dict = {
    //  256: '已初始化',
    //  257: '未初始化',
    //  258: '设备不存在',
    //  259: '不支持的协议类型',
    //  260: '该数据类型不支持此操作',
    //  261: '该数据类型不支持',
    //  264: '无法打开输入文件',
    //  265: '字段数与内容数不匹配',
    //  266: '通讯数据异常',
    //  267: '解析数据异常',
    //  268: 'CodePage错误',
    //  269: '无法创建输出文件',

    //  0: 'sucessed',
    //  1: 'processing',

    //  403: '[链接超时][默认40秒]',
    //  404: '[LoadLibrary][未加载到AclasSDK.dll]',
    //  405: 'polyfill addons.',
    //  406: '报错 子进程拉起多次失败',
    //  501: 'ping 超时',
    //  502: 'ping 不通',
    //}

    class Program
    {
        static void Main(string[] args)
        {
            // TestReadIni();
            // TestHttpServer();
            TestAjaxServer();

            string host = "192.168.1.3";
            uint dataType = 0x0000;
            string deskTop = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            string fileName = Path.Combine(deskTop, "plu.txt");

            // TestAclas(new AclasSDK_Args(host, dataType, fileName));
        }

        static string FeedbackAjax(Dictionary<string, string> dict)
        {
            Console.WriteLine("dict ---- {0}", dict);

            return AjaxServer.AssembleRes(new ResponseAjax("Hahahah | " + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")));
        }

        static void FeedbackAclas(CMD_ACLAS cmd, ResponseAclas res)
        {
            Console.WriteLine("{0}|coder:{1}|{2}/{3}", cmd, res.code, res.index, res.total);
        }

        // 测试方法
        static void TestReadIni()
        {
            string initPath = Environment.CurrentDirectory + "\\config.ini";
            Console.WriteLine(initPath);
            string str = Helper.ReadString("host", "host", "host", initPath);
            Console.ReadKey();
        }

        static void TestHttpServer() {
            TestHttpServer http = new TestHttpServer();
            http.StartServer();
        }

        static void TestAjaxServer()
        {
            AjaxServer ajax = new AjaxServer();
            ajax.StartAjaxServer(new FeedbackAjax(FeedbackAjax));
        }

        static void TestAclas(AclasSDK_Args args)
        {
            Aclas.StartTask(new FeedbackAclas(FeedbackAclas), args);
        }
    }
}
