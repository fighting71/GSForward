using AccountDomain;
using Common.MySqlProvide;
using Common.MySqlProvide.Generate;
using Dapper;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Text;

namespace TestConsole.Tests
{
    /// <summary>
    /// @auth : monster
    /// @since : 5/26/2020 10:38:47 AM
    /// @source : 
    /// @des : 
    /// </summary>
    public class DbTest
    {

        public void Run()
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

            {

                var sql = @"SELECT * FROM gsuser WHERE Id = @Id";

                object param = new Dictionary<string, object>() { { "@Id", 1 } };

                IEnumerable<GSUser> gsUser = connection.Query<GSUser>(sql, param);


            }

            //var res = connection.QueryFirst<bool>("SELECT EXISTS (SELECT Id FROM gsuser) ");

            //Console.WriteLine(res);
        }

    }
}
