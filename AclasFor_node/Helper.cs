using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// DllImport
using System.Runtime.InteropServices;

namespace AclasFor_node
{
    class Helper
    {
        // 声明读写INI文件的API函数
        [DllImport("kernel32")]
        private static extern bool WritePrivateProfileString(string section, string key, string val, string filePath);
        [DllImport("kernel32")]
        // section=配置节，key=键名，value=键值，path=路径
        private static extern int GetPrivateProfileString(string section, string key, string def, byte[] retVal, int size, string filePath);

        // 读取字符串
        public static string ReadString(string Section, string Ident, string Default, string FileName)
        {
            Byte[] Buffer = new byte[65535];
            int bufLen = GetPrivateProfileString(Section, Ident, Default, Buffer, Buffer.GetUpperBound(0), FileName);
            string s = Encoding.GetEncoding(0).GetString(Buffer);
            s = s.Substring(0, bufLen);
            return s.Trim();
        }

        // 读取整数
        public static int ReadInteger(string Section, string Ident, int Default, string FileName)
        {
            string intStr = ReadString(Section, Ident, Convert.ToString(Default), FileName);
            return Convert.ToInt32(intStr);
        }

    }
}
