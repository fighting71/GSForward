using Common.MySqlProvide;
using Dapper;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Diagnostics;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace GS.AppContext.Impl
{
    /// <summary>
    /// @auth : monster
    /// @since : 5/21/2020 3:09:14 PM
    /// @source : 
    /// @des : 
    /// </summary>
    public class MysqlQueryContext : IQueryContext
    {
        private readonly IFrommGnerate formGnerate;
        private readonly ISelectGnerate selectGnerate;
        private readonly IWhereGnerate whereGnerate;
        private readonly IEditGenerate editGenerate;
        private readonly DbConnection conn;

        public int AddAndGetKey<T>(T data)
        {
            StringBuilder build = editGenerate.CreateCommandString(data);

            build.AppendLine();
            build.Append("SELECT LAST_INSERT_ID();");

            int res = conn.ExecuteScalar<int>(build.ToString(), data);

            return conn.ExecuteScalar<int>(build.ToString(), data);

        }

        public Task<int> AddAndGetKeyAsync<T>(T data)
        {
            StringBuilder build = editGenerate.CreateCommandString(data);

            build.AppendLine();
            build.Append("SELECT LAST_INSERT_ID();");

            int res = conn.ExecuteScalar<int>(build.ToString(), data);

            return conn.ExecuteScalarAsync<int>(build.ToString(), data);

        }

        public int Add<T>(T data)
        {
            StringBuilder build = editGenerate.CreateCommandString(data);

            return conn.Execute(build.ToString(), data);

        }

        public Task<int> AddAsync<T>(T data)
        {
            StringBuilder build = editGenerate.CreateCommandString(data);

            return conn.ExecuteAsync(build.ToString(), data);

        }

        public MysqlQueryContext(IFrommGnerate formGnerate, ISelectGnerate selectGnerate, IWhereGnerate whereGnerate, IEditGenerate editGenerate
            , DbConnection conn)
        {
            this.formGnerate = formGnerate;
            this.selectGnerate = selectGnerate;
            this.whereGnerate = whereGnerate;
            this.editGenerate = editGenerate;
            this.conn = conn;
        }

        public Task<bool> AnyAsync<T>(Expression<Func<T, bool>> whereExpression)
        {
            StringBuilder builder = QueryString<T, T>(selectGnerate, formGnerate, whereGnerate, null, Expression.Constant(typeof(T)), whereExpression);

            builder.Insert(0, "SELECT EXISTS(");
            builder.Append(")");

            return conn.QueryFirstAsync<bool>(builder.ToString());
        }

        public bool Any<T>(Expression<Func<T, bool>> whereExpression)
        {
            StringBuilder builder = QueryString<T, T>(selectGnerate, formGnerate, whereGnerate, null, Expression.Constant(typeof(T)), whereExpression);

            builder.Insert(0, "SELECT EXISTS(");
            builder.Append(")");

            return conn.QueryFirst<bool>(builder.ToString());
        }

        public Task<IEnumerable<T>> QueryAsync<T>(Expression<Func<T, bool>> whereExpression)
        {

            StringBuilder stringBuilder = QueryString<T, T>(selectGnerate, formGnerate, whereGnerate, null, Expression.Constant(typeof(T)), whereExpression);

            return conn.QueryAsync<T>(stringBuilder.ToString());

        }

        public IEnumerable<T> Query<T>(Expression<Func<T, bool>> whereExpression)
        {

            StringBuilder stringBuilder = QueryString<T, T>(selectGnerate, formGnerate, whereGnerate, null, Expression.Constant(typeof(T)), whereExpression);

            return conn.Query<T>(stringBuilder.ToString());

        }

        private StringBuilder QueryString<TSource, TResult>(
        ISelectGnerate selectGnerate, IFrommGnerate formGnerate, IWhereGnerate whereGnerate,
        Expression<Func<TSource, TResult>> selectExpression, Expression formExpression, Expression<Func<TSource, bool>> whereExpression)
        {

            StringBuilder builder = new StringBuilder();

            builder.AppendLine(selectGnerate.Explain(selectExpression));
            builder.AppendLine(formGnerate.Explain(formExpression));
            builder.AppendLine(whereGnerate.Explain(whereExpression));

            return builder;
        }

    }
}
