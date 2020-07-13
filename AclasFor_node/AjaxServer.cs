using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Net;
using System.Net.Sockets;
using System.Threading;

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
        private void StartListen()
        {
            while(true)
            {
                // 接收新链接
                Socket socket = listener.AcceptSocket();

                Console.WriteLine("\n<-<-<-<-");
                Console.WriteLine("Socket Type {0}", socket.SocketType);

                if (socket.Connected)
                {
                    Console.WriteLine("Client Connected!");
                    Console.WriteLine("Client IP {0}", socket.RemoteEndPoint);

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

                    Console.WriteLine("\n****\n");
                    Console.WriteLine(buffer);
                    Console.WriteLine("\n****\n");

                    // 查找 HTTP 位置
                    int startPos = buffer.IndexOf("HTTP", 1);
                    string httpVersion = buffer.Substring(startPos, 8);

                    // SendHeader("", "", 10, "", ref socket);
                    SendData("{\"code\":\"0\",\"data\":\"Hello C#\"}", ref socket);

                    Console.WriteLine("->->->->\n");

                    socket.Close();
                }
            }
        }

        // 发送数据
        public void SendData(string data, ref Socket socket)
        {
            SendHeader("", "", data.Length, "", data, ref socket);
        }

        // 发送 header
        public void SendHeader(string httpVersion, string MIME, int totBytes, string statusCode, string data, ref Socket socket)
        {
            if (httpVersion.Length == 0) httpVersion = "HTTP/1.1"; // 默认版本
            if (MIME.Length == 0) MIME = "text/html"; // 默认 text/html
            if (statusCode.Length == 0) statusCode = "200 OK"; // 默认状态码

            string buffer = httpVersion + " " + statusCode + "\r\n"
                + "Server: C#/Aclas" + "\r\n"
                + "Content-Type: " + MIME + "\r\n"
                + "Content-Length: " + totBytes + "\r\n"
                + "\r\n";
            SendToBrowser(buffer + data, ref socket);
        }

        // 发送 body
        public void SendToBrowser(byte[] data, ref Socket socket)
        {
            try
            {
                int numBytes;
                if (socket.Connected)
                {
                    if ((numBytes = socket.Send(data, data.Length, SocketFlags.None)) == -1) {
                        Console.WriteLine("Socket Error cannot Send Packet.");
                    } else
                    {
                        Console.WriteLine("No. of bytes send {0}", numBytes);
                    }
                } else
                {
                    Console.WriteLine("socket.Connected: false");
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("CATCH_ERROR>> SendToBrowser 报错: {0}", e);
            }
        }

        public void SendToBrowser(string data, ref Socket socket)
        {
            SendToBrowser(Encoding.ASCII.GetBytes(data), ref socket);
        }

    }
}
