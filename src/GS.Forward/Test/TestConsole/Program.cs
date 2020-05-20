using Common.MySqlProvide.Extension;
using Common.MySqlProvide.Visitor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace TestConsole
{
    class Program
    {
        static void Main(string[] args)
        {

            #region test mysql visitor

            MysqlSingleVisitor visitor = new MysqlSingleVisitor();

            var queryable = new List<Personal>().AsQueryable();

            //IQueryable<Personal> queryable = list.AsQueryable();

            //Expression<Func<Personal, bool>> expression = u => u.Age > 10;

            //var expression2 = Expression.Call(null, ((MethodInfo)MethodBase.GetCurrentMethod())
            //    .MakeGenericMethod(new Type[] { typeof(Personal) }),
            //    new Expression[] { queryable.Expression, Expression.Quote(expression) });

            Console.WriteLine(queryable.GetSql(visitor, u => u.Age > 10));
            Console.WriteLine(queryable.GetSql(visitor, u => u.Age > 10 && u.Name == "test"));
            Console.WriteLine(queryable.GetSql(visitor, u => u.Age > 10 && u.Name != "test"));
            Console.WriteLine(queryable.GetSql(visitor, u => u.Age > 10 || u.Name == "test"));
            Console.WriteLine(queryable.GetSql(visitor, u => u.Age != 10 && !(u.Name == null)));
            Console.WriteLine(queryable.GetSql(visitor, u => u.Age > 10 && u.Age < 20));
            //queryable.GetSql(visitor, u => u.Age > 10 || u.Name.StartsWith("132"));

            #endregion

            Console.WriteLine("Hello World!");

            Console.ReadKey();

        }

        public class Personal
        {
            public string Name { get; set; }

            public int Age { get; set; }
        }

    }
}
