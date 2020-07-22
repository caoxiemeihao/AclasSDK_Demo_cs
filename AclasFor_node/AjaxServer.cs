using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Text.RegularExpressions;

// 参考链接:
// http://blog.okbase.net/haobao/archive/60.html
// https://mp.weixin.qq.com/s/n0qfNsn9i2DCfRIRsWR0tw

namespace AclasFor_node
{
    class ResponseAjax
    {
        public string data;
        public int code;
        public string msg;

        public ResponseAjax(string data, int code = 0, string msg = "")
        {
            this.data = data;
            this.code = code;
            this.msg = msg;
        }

        public override string ToString()
        {
            return AjaxServer.AssembleRes(this);
        }
    }

    class AjaxServer
    {
        private TcpListener listener;
        private int port = 9497; // 9999 被 WXWork.exe 占用 😥
        private FeedbackAjax @feedbackAjax;
        private Dictionary<string, string> serverDict = new Dictionary<string, string>();

        public void StartAjaxServer(FeedbackAjax @delegate)
        {
            @feedbackAjax = @delegate;
            try
            {
                listener = new TcpListener(IPAddress.Any, port);
                listener.Start();

                //Console.WriteLine("Ajax Server Running at {0}\nPress ^C to Stop.", port); di
                serverDict["state"] = "start";
                serverDict["port"] = port.ToString();
                Program.StdoutToNodeJs("server", Dict2JsonStr(serverDict));

                Thread thread = new Thread(new ThreadStart(StartListen));
                thread.Start();
            }
            catch (Exception e)
            {
                //Console.WriteLine("[CATCH_ERROR] 启动 http 服务器失败:\n{0}", e);
                serverDict["state"] = "error";
                serverDict["error"] = e.ToString();
                Program.StdoutToNodeJs("server", Dict2JsonStr(serverDict));
            }
        }

        // 监听线程
        void StartListen()
        {
            while(true)
            {
                // 接收新链接
                // 这里会阻塞线程，每一次接收、处理一个链接后；while(true) 会回到这里继续等待
                Socket socket = listener.AcceptSocket();

                //Thread thread = new Thread(new ThreadStart(() => HancelAcceptSocket(socket)));
                Thread thread = new Thread(new ParameterizedThreadStart(HancelAcceptSocket));
                thread.Start(socket);
            }
        }

        // 处理 AcceptSocket
        void HancelAcceptSocket(object args)
        {
            Socket socket = (Socket)args;
            // socket.SendTimeout = 1000 * 5; // 5 秒超时

            // Console.WriteLine("Socket Type {0}", socket.SocketType);

            string now = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            // Console.WriteLine("\n<< Client Connected! [{0}]", socket.RemoteEndPoint);

            try
            {
                // 假定数据不会超过 2KB(只接受GT)
                byte[] receive = new byte[1024 * 2];
                // 浏览器发起请求，偶尔会卡死 20-07-13(20-07-13晚；多线程解决)
                int chunk = socket.Receive(receive, receive.Length, SocketFlags.None);

                // 转成字符串
                string buffer = Encoding.ASCII.GetString(receive);

                //Console.WriteLine("\n{0}", buffer.Substring(0, buffer.IndexOf("\r\n")));
                serverDict["state"] = "alive";
                serverDict["header"] = buffer.Substring(0, buffer.IndexOf("\r\n"));
                Program.StdoutToNodeJs("server", Dict2JsonStr(serverDict));

                // 只处理 GET 请求
                if (buffer.Substring(0, 3) != "GET")
                {
                    //Console.WriteLine("目前只处理 GIT 请求");
                    serverDict["state"] = "error";
                    serverDict["error"] = "目前只处理 GIT 请求";
                    Program.StdoutToNodeJs("server", Dict2JsonStr(serverDict));
                    socket.Close();
                    return;
                }
                // 过滤掉 favicon.ico
                string firstLine = buffer.Substring(0, buffer.IndexOf("\r\n"));
                if (Regex.IsMatch(firstLine, @"favicon\.ico"))
                {
                    //Console.WriteLine("过滤掉 favicon.ico");
                    socket.Close();
                    return;
                }

                // 查找 HTTP 位置
                // int startPos = buffer.IndexOf("HTTP", 1);
                // string httpVersion = buffer.Substring(startPos, 8);

                // SendToBrowser(ref socket, AssembleRes(new ResponseAjax(now)));
                SendToBrowser(ref socket, @feedbackAjax(QueryString2Dict(buffer)));
                socket.Close();
            }
            catch (Exception e)
            {
                // 关闭当前链接客户端
                socket.Close();
                //Console.WriteLine("[CATCH_ERROR] socket.Receive:\n{0}", e);
                serverDict["state"] = "error";
                serverDict["error"] = "[CATCH_ERROR] socket.Receive:\n{0}";
                Program.StdoutToNodeJs("server", Dict2JsonStr(serverDict));
            }
        }

        // 头部信息
        string WithHeader(
            int totBytes,
            string statusCode = "200 OK",    // 默认状态码
            string httpVersion = "HTTP/1.1", // 默认版本
            string MIME = "text/html"        // 默认 text/html
        )
        {
            string header = httpVersion + " " + statusCode + "\r\n" // 状态行
                                                                    // 报文首部
                + "Server: C#/Aclas" + "\r\n"
                + "Content-Type: " + MIME + "\r\n"
                + "Content-Length: " + totBytes + "\r\n"
                + "Access-Control-Allow-Origin: *" + "\r\n" // 跨域支持
                + "\r\n";                                   // 请求头和请求体之间的 空行
            return header;
        }

        void SendToBrowser(ref Socket socket, byte[] data)
        {
            string now = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            try
            {
                int numBytes;
                if (socket.Connected)
                {
                    numBytes = socket.Send(data, data.Length, SocketFlags.None);
                    if (numBytes == -1) {
                        //Console.WriteLine("Socket Error cannot Send Packet.");
                    }
                    else
                    {
                        // Console.WriteLine(">> No. of bytes send {0}. [{1}]", numBytes, now);
                        string s = Encoding.ASCII.GetString(data);
                        //Console.WriteLine("----\n{0}\n", s.Substring(s.IndexOf("\r\n\r\n") + 4));
                    }
                }
                else
                {
                    //Console.WriteLine("socket.Connected: false");
                }
            }
            catch (Exception e)
            {
                //Console.WriteLine("[CATCH_ERROR] SendToBrowser 报错:\n{0}", e);
            }
        }

        // 发送数据
        public void SendToBrowser(ref Socket socket, string data)
        {
            //打包发
            SendToBrowser(ref socket, Encoding.ASCII.GetBytes(WithHeader(data.Length) + data));

            //分开发
            //SendToBrowser(ref socket, Encoding.ASCII.GetBytes(WithHeader(data.Length)));
            //SendToBrowser(ref socket, Encoding.ASCII.GetBytes(data));
            //socket.Close();
        }

        // 拼装响应数据
        public static string AssembleRes(ResponseAjax json)
        {
            string data = new Regex(@"^(\{|\[\{)").IsMatch(json.data)
                ? json.data
                : "\"" + json.data + "\"";
            return "{\"code\":" + json.code + ",\"data\":" + data + ",\"msg\":\"" + json.msg + "\"}"; ;
        }

        // 解析 Query String
        public Dictionary<string, string> QueryString2Dict(/*原始 http 字符串*/string httpRaw)
        {
            //string firstLine = Regex.Split(httpRaw, @"\r\n")[0];
            string firstLine = httpRaw.Substring(0, httpRaw.IndexOf("\r\n"));
            string qeuryString = Regex.Match(firstLine, @"(?<=\?).+(?=\sHTTP)").ToString();
            string[] tmpArr = qeuryString.Split('&');
            Dictionary<string, string> dict = new Dictionary<string ,string>();
            for (int x = 0; x < tmpArr.Length; x++)
            {
                string[] tmp = tmpArr[x].Split('=');
                if (tmp.Length > 1) {
                    // UrlDecode
                    tmp[1] = Uri.UnescapeDataString(tmp[1]);
                    dict.Add(tmp[0], tmp[1]);
                };
            }
            return dict;
        }

        // 字典转 json 字符串
        public static string Dict2JsonStr(Dictionary<string, string> dict)
        {
            string val = "";
            foreach(KeyValuePair<string, string> item in dict)
            {
                bool isNumber = new Regex(@"^-?\d+$").IsMatch(item.Value);
                bool isBool = new Regex(@"^(true|false)$").IsMatch(item.Value);
                if (isNumber || isBool)
                {
                    val += "\"" + item.Key + "\":" + item.Value + ",";
                } else
                {

                    val += "\"" + item.Key + "\":\"" + item.Value + "\",";
                }
            }
            if (val.Length > 0)
            {
                val = val.Substring(0, val.Length - 1); // 去掉尾部的 ,
            }

            return "{" + val + "}";
        }

    }
}
