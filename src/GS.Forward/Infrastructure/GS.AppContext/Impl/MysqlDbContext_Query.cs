using Dapper;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace GS.AppContext.Impl
{
    /// <summary>
    /// @auth : monster
    /// @since : 5/22/2020 2:35:44 PM
    /// @source : 
    /// @des : 
    /// </summary>
    public partial class MySqlDbContext : IDbContext
    {

        public Task<bool> AnyAsync<T>(Expression<Func<T, bool>> whereExpression)
        {

            Dictionary<string, object> param = new Dictionary<string, object>();

            StringBuilder build = QueryString<T, T>(selectGnerate, formGnerate, whereGnerate, null, Expression.Constant(typeof(T)), whereExpression, param);

            build.Insert(0, "SELECT EXISTS(");
            build.Append(")");

            this.Log($"{MethodBase.GetCurrentMethod().Name}-invoke:\n{build}");

            return conn.QueryFirstAsync<bool>(build.ToString(), param);
        }

        public Task<IEnumerable<T>> QueryAsync<T>(Expression<Func<T, bool>> whereExpression) => QueryAsync<T, T>(null, whereExpression);

        public Task<IEnumerable<TResult>> QueryAsync<T, TResult>(Expression<Func<T, TResult>> selectExpression, Expression<Func<T, bool>> whereExpression)
        {

            Dictionary<string, object> param = new Dictionary<string, object>();

            StringBuilder build = QueryString<T, TResult>(selectGnerate, formGnerate, whereGnerate, selectExpression, Expression.Constant(typeof(T)), whereExpression, param);

            this.Log($"{MethodBase.GetCurrentMethod().Name}-invoke:\n{build}");

            return conn.QueryAsync<TResult>(build.ToString(), param);
        }

        public Task<TResult> QueryFirstOrDefaultAsync<T, TResult>(Expression<Func<T, TResult>> selectExpression, Expression<Func<T, bool>> whereExpression)
        {

            Dictionary<string, object> param = new Dictionary<string, object>();

            StringBuilder build = QueryString<T, TResult>(selectGnerate, formGnerate, whereGnerate, selectExpression, Expression.Constant(typeof(T)), whereExpression, param);

            this.Log($"{MethodBase.GetCurrentMethod().Name}-invoke:\n{build}");

            return conn.QueryFirstOrDefaultAsync<TResult>(build.ToString(), param);
        }

    }
}
