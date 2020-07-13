using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;

namespace AclasFor_node
{
    class Program
    {
        static void Main(string[] args)
        {
            // TestReadIni();
            // TestHttpServer();
            TestAjaxServer();
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
            ajax.StartAjaxServer();
        }
    }
}
