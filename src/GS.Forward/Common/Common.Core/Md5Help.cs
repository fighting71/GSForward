using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace Common.Core
{
    /// <summary>
    /// @auth : monster
    /// @since : 5/22/2020 11:08:44 AM
    /// @source : 
    /// @des : 
    /// </summary>
    public static class Md5Help
    {

        public static string Md5Hash(string input,Encoding encoding = null)
        {
            encoding = encoding ?? Encoding.UTF8;

            StringBuilder builder = new StringBuilder();
            foreach (byte num in Md5HashBytes(encoding.GetBytes(input)))
            {
                builder.AppendFormat("{0:x2}", num);
            }
            return builder.ToString();

        }

        public static byte[] Md5HashBytes(byte[] bytes)
        {
            MD5CryptoServiceProvider md5Hasher = new MD5CryptoServiceProvider();
            return md5Hasher.ComputeHash(bytes);
        }

    }
}
