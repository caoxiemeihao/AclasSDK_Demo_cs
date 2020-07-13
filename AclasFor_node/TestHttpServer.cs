using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.IO;

/// <summary>
/// 测试用的
/// 只能看看，不大好用；SendHeader、SendToBrowser 设计的有点问题 20-07-13
/// 参考链接: https://m.open-open.com/code/view/1423320823404
/// </summary>

namespace AclasFor_node
{
    class TestHttpServer
    {
        private TcpListener listener;
        private int port = 8009;

        public void StartServer()
        {
            try
            {
                listener = new TcpListener(port);
                listener.Start();

                Console.WriteLine("Web Server Running at 8009 ...Press ^C to Stop.");
                Thread th = new Thread(new ThreadStart(StartListen));
                th.Start();
            }
            catch (Exception e)
            {
                Console.WriteLine("ERROR>> 启动http服务器报错" + e);
            }
        }

        public void StartListen() {
            int startPos = 0;
            string request;
            string requestedFile;
            string dirname;
            string errorMessage;
            string localDir;
            string webServerRoot = "D:\\ypsx\\HttpServer_cs\\"; // 服务器虚拟目录
            string physicalFilePath = "";
            string formattedMessage = "";
            string response = "";

            while(true)
            {
                // 接受新链接
                Socket skt = listener.AcceptSocket();

                Console.WriteLine("Socket Type " + skt.SocketType);

                if (skt.Connected)
                {
                    Console.WriteLine("\nClient Connected!\n====\nClient IP {0}\n", skt.RemoteEndPoint);

                    Byte[] receive = new byte[1024];
                    int i = skt.Receive(receive, receive.Length, 0);

                    // 转换成字符串类型
                    string buffer = Encoding.ASCII.GetString(receive);

                    // 只处理 GET 请求
                    if (buffer.Substring(0, 3) != "GET")
                    {
                        Console.WriteLine("目前只处理 GET 请求");
                        skt.Close();
                        return;
                    }

                    // 查找 HTTP 的位置
                    startPos = buffer.IndexOf("HTTP", 1);
                    string httpVersion = buffer.Substring(startPos, 8);
                    // 解析请求类型和文件目录名
                    request = buffer.Substring(0, startPos - 1);
                    request.Replace("\\", "/");
                    // 如果结尾不是文件名也不是以"/"结尾则加"/"
                    if ((request.IndexOf(".") < 1) && (!request.EndsWith("/")))
                    {
                        request = request + "/";
                    }
                    // 解析请求文件名
                    startPos = request.LastIndexOf("/") + 1;
                    requestedFile = request.Substring(startPos);
                    // 解析请求文件目录
                    dirname = request.Substring(request.IndexOf("/"), request.LastIndexOf("/") - 3);
                    // 虚拟目录物理路径
                    localDir = webServerRoot;

                    Console.WriteLine("请求文件目录: " + localDir);

                    if (localDir.Length == 0)
                    {
                        errorMessage = "Error! Requested Directory does not exists";
                        SendHeader(httpVersion, "", errorMessage.Length, "404 Not Found", ref skt);
                        skt.Close();
                        continue;
                    }

                    if (requestedFile.Length == 0)
                    {
                        // 指定默认文件名
                        requestedFile = "index.html";
                    }

                    string mimeType = "text/html";
                    physicalFilePath = localDir + requestedFile;

                    Console.WriteLine("请求文件: " + physicalFilePath);

                    if (!File.Exists(physicalFilePath))
                    {
                        errorMessage = "404 Error! File Does Not Exists.";
                        SendHeader(httpVersion, "", errorMessage.Length, "404 Not Found", ref skt);
                        SendToBrowser(errorMessage, ref skt);

                        Console.WriteLine(formattedMessage);
                    } else
                    {
                        int totBytes = 0;
                        request = "";
                        FileStream fs = null;
                        BinaryReader reader = null;

                        try
                        {
                            fs = new FileStream(physicalFilePath, FileMode.Open, FileAccess.Read, FileShare.Read);
                            reader = new BinaryReader(fs);
                            byte[] bytes = new byte[fs.Length];

                            int read;
                            while((read = reader.Read(bytes, 0, 10)) != 0) {
                                response = response + Encoding.ASCII.GetString(bytes, 0, read);
                                totBytes = totBytes + read;
                            }
                            SendHeader(httpVersion, mimeType, totBytes, " 200 OK", ref skt);
                            SendToBrowser(bytes, ref skt);
                        } catch (Exception e)
                        {
                            Console.WriteLine("ERROR>> 读取静态文件: " + e);
                        } finally
                        {
                            if (fs != null) fs.Close();
                            if (reader != null) reader.Close();
                        }

                    }

                    skt.Close();

                }
            }

        }

        public void SendHeader(string httpVersion, string MIME_header, int totBytes, string statusCode, ref Socket skt)
        {
            string buffer = "";
            if (MIME_header.Length == 0)
            {
                MIME_header = "text/html"; // 默认 text/html
            }

            buffer = buffer
                + httpVersion + statusCode + "\r\n"
                + "Server: Aclas for node" + "\r\n"
                + "Content-Type: " + MIME_header + "\r\n"
                + "Accept-Ranges: bytes" + "\r\n"
                + "Content-Length: " + totBytes + "\r\n";
            Byte[] sendData = Encoding.ASCII.GetBytes(buffer);
            SendToBrowser(sendData, ref skt);
            Console.WriteLine("Total Bytes: " + totBytes.ToString());
        }

        public void SendToBrowser(Byte[] data, ref Socket skt)
        {
            int numBytes = 0;
            try
            {
                if (skt.Connected)
                {
                    if ((numBytes = skt.Send(data, data.Length, 0)) == -1)
                    {
                        Console.WriteLine("Socket Error cannot Send Packet");
                    } else
                    {
                        Console.WriteLine("No. of bytes send {0}", numBytes);
                    }
                } else
                {
                    Console.WriteLine("链接失败...");
                }
            } catch (Exception e)
            {
                Console.WriteLine("ERROR>> SendToBrowser 报错: {0}", e);
            }
        }

        public void SendToBrowser(string data, ref Socket skt)
        {
            SendToBrowser(Encoding.ASCII.GetBytes(data), ref skt);
        }

    }
}
