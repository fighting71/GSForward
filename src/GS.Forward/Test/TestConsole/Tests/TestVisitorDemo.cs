using Common.MySqlProvide.Generate;
using Common.MySqlProvide.Visitor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace TestConsole.Tests
{
    /// <summary>
    /// @auth : monster
    /// @since : 5/21/2020 3:43:23 PM
    /// @source : 
    /// @des : 
    /// </summary>
    public class TestVisitorDemo
    {

        public void Run()
        {
            #region test visitor

            IList<Personal> list = new List<Personal>();

            IQueryable<Personal> queryable = list.AsQueryable();

            {

                //TestVisitor visitor = new TestVisitor();

                ////Expression<Func<Personal, bool>> expression = u => u.Age == 12 && u.Name == u.NickName + "123";
                ////Expression<Func<Personal, bool>> expression = u => !(u.Age >= 12);
                //Expression expression = Expression.Constant(typeof(Personal));

                //string name = "12312";
                //Expression<Func<Personal, bool>> expression = u => u.Name == name;

                //visitor.Visit(expression);

            }

            { // where visitor

                WhereGenerateVisitor visitor = new WhereGenerateVisitor();

                //Expression<Func<Personal, bool>> expression = u => u.Age == 12 && u.Name == u.NickName + "123";
                //string name = "12312";
                //Expression<Func<Personal, bool>> expression = u => u.Name == name;
                var data = new { Name = "test" };
                Expression<Func<Personal, bool>> expression = u => u.Name == data.Name;

                Console.WriteLine(visitor.Explain(expression));

            }

            {// select visitor
                //Console.WriteLine(queryable.SelectExpression(u => new { Info = u.Age, Name = "test" }));
                //Console.WriteLine(queryable.SelectExpression(u => u));
                //Console.WriteLine(queryable.SelectExpression(u => u.Name));
                //Console.WriteLine(queryable.SelectExpression(u => new { u.Age,u.Name }));
                //Console.WriteLine(queryable.SelectExpression(u => new { u.Age,u.Name,Back = u.Name }));
            }

            #endregion
        }

        public class Personal
        {
            public string Name { get; set; }

            public int Age { get; set; }

            public string NickName { get; set; }

        }

    }
}
