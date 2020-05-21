using AccountDomain;
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

            new TestVisitorDemo().Run();

            { // dapper test
                DbConnection connection = new MySqlConnection("Server=localhost;Database=gsforward; User=root;Password=root;charset=utf8mb4;sslmode=none;");

                IEditGenerate generate = new EditGenerate();

                { // test add
                    //var user = new GSUser() { 

                    //    Contact = "666666",
                    //    Name = "test_generate",
                    //    NickName = "测试生成",
                    //    Email = "777@qq.com",
                    //    LoginPwd = "123456"

                    //};

                    //StringBuilder sql = generate.CreateCommandString(user);

                    //Console.WriteLine(sql);

                    //int res = connection.Execute(sql.ToString(), user);

                    //Console.WriteLine($"execute res : {res}");
                }

                {

                    //var user = new GSUser()
                    //{

                    //    Contact = "666666",
                    //    Name = "test_generate222",
                    //    NickName = "测试生成",
                    //    Email = "777@qq.com",
                    //    LoginPwd = "123456"

                    //};

                    //StringBuilder sql = generate.CreateCommandString(user);

                    //sql.AppendLine();
                    //sql.Append("SELECT LAST_INSERT_ID();");

                    //Console.WriteLine(sql);

                    //int res = connection.ExecuteScalar<int>(sql.ToString(), user);

                    //Console.WriteLine($"execute res : {res}");

                }

                //var res = connection.QueryFirst<bool>("SELECT EXISTS (SELECT Id FROM gsuser) ");

                //Console.WriteLine(res);
            }

            Console.WriteLine("Hello World!");

            Console.ReadKey();

        }


    }
}
