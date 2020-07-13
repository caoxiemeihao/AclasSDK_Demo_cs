using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Text.RegularExpressions;

namespace AclasFor_node
{
    class AjaxServer
    {
        private TcpListener listener;
        private int port = 8009;

        public void StartAjaxServer()
        {
            try
            {
                listener = new TcpListener(IPAddress.Any, port);
                listener.Start();

                Console.WriteLine("Ajax Server Running at {0}\nPress ^C to Stop.", port);

                Thread thread = new Thread(new ThreadStart(StartListen));
                thread.Start();
            }
            catch (Exception e)
            {
                Console.WriteLine("CATCH_ERROR>> 启动 http 服务器失败: " + e);
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
                socket.SendTimeout = 1000 * 5; // 5 秒超时

                // Console.WriteLine("Socket Type {0}", socket.SocketType);

                if (socket.Connected)
                {
                    Console.WriteLine("\n<< Client Connected! [{0}]", socket.RemoteEndPoint);
                    
                    try
                    {
                        byte[] receive = new byte[1024];
                        int i = socket.Receive(receive, receive.Length, SocketFlags.None);

                        // 转成成字符串
                        string buffer = Encoding.ASCII.GetString(receive);

                        // 只处理 GET 请求
                        if (buffer.Substring(0, 3) != "GET")
                        {
                            Console.WriteLine("目前只处理 GIT 请求");
                            socket.Close();
                            return;
                        }

                        // 查找 HTTP 位置
                        // int startPos = buffer.IndexOf("HTTP", 1);
                        // string httpVersion = buffer.Substring(startPos, 8);
                    
                        SendToBrowser(
                            ref socket,
                            AssembleRes(DateTime.Now.ToString("yyyy-MM-dd HH:MM:ss") + "0000")
                        );
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine("CATCH_ERROR>> socket.Receive: " + e);
                    }
                    finally
                    {
                        // 关闭当前链接客户端
                        socket.Close();
                    }

                }
            }
            // listener.Stop(); // 主动关闭监听
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
                + "Access-Control-Allow-Origin: *" + "\r\n"; // 跨域支持
            return header;
        }

        void SendToBrowser(ref Socket socket, byte[] data)
        {
            try
            {
                int numBytes;
                if (socket.Connected)
                {
                    numBytes = socket.Send(data, data.Length, SocketFlags.None);
                    if (numBytes == -1) {
                        Console.WriteLine("Socket Error cannot Send Packet.");
                    }
                    else
                    {
                        Console.WriteLine(">> No. of bytes send {0}", numBytes);
                    }
                }
                else
                {
                    Console.WriteLine("socket.Connected: false");
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("CATCH_ERROR>> SendToBrowser 报错: {0}", e);
            }
        }

        // 发送数据
        public void SendToBrowser(ref Socket socket, string data)
        {
            data = WithHeader(data.Length) // 请求头
                + "\r\n"                   // 请求头和请求体之间的 空行
                + data;                    // 请求体
            SendToBrowser(ref socket, Encoding.ASCII.GetBytes(data));
        }

        // 拼装响应数据
        public string AssembleRes(string data, int coed = 0, string msg = "")
        {
            return "{code:" + coed + ",\"data\":\"" + data + "\",\"msg\":\"" + msg + "\"}";
        }

        // 解析 Query String
        public Dictionary<string, string> QueryString2Dict(string httpRaw)
        {
            string firstLine = Regex.Split(httpRaw, @"\r\n")[0];
            string qeuryString = Regex.Match(firstLine, @"(?<=\?).+(?=\sHTTP)").ToString();
            string[] tmpArr = qeuryString.Split('&');
            Dictionary<string, string> dict = new Dictionary<string ,string>();
            for (int x = 0; x < tmpArr.Length; x++)
            {
                string[] tmp = tmpArr[x].Split('=');
                dict.Add(tmp[0], tmp[1]);
            }
            return dict;
        }

    }
}
