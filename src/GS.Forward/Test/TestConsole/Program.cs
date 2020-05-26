using AccountDomain;
using Common.Core;
using Common.MySqlProvide;
using Common.MySqlProvide.Extension;
using Common.MySqlProvide.Generate;
using Common.MySqlProvide.Visitor;
using Dapper;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using TestConsole.Tests;

namespace TestConsole
{
    class Program
    {
        static void Main(string[] args)
        {

            string key = "(!@^#*(&";


            byte[] saltByte = new byte[] { 12, 31, 123, 41, 11, 55, 33, 23 };
            string token = EncryptionHelp.AESEncrypt("1", key, saltByte);

            Console.WriteLine(token);

            string value = EncryptionHelp.AESDecrypt(token, key, saltByte);

            Console.WriteLine(value);

            //new TestVisitorDemo().Run();

            Console.WriteLine("Hello World!");

            Console.ReadKey();

        }


    }
}
