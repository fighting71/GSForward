using Common.MySqlProvide;
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
    /// @since : 5/22/2020 2:38:01 PM
    /// @source : 
    /// @des : 
    /// </summary>
    public partial class MySqlDbContext : IDbContext
    {

        public Task<int> AddAndGetKeyAsync<T>(T data)
        {
            StringBuilder build = editGenerate.CreateCommandString(data);

            build.AppendLine();
            build.Append(QueryConst.GetIncrementKey);

            this.Log($"{MethodBase.GetCurrentMethod().Name}-invoke:\n{build}");

            return conn.ExecuteScalarAsync<int>(build.ToString(), data);

        }

        public Task<int> AddAsync<T>(T data)
        {
            StringBuilder build = editGenerate.CreateCommandString(data);

            this.Log($"{MethodBase.GetCurrentMethod().Name}-invoke:\n{build}");

            return conn.ExecuteAsync(build.ToString(), data);

        }

        public Task<int> UpdateAsync<T>(T data, Expression<Func<T, bool>> whereExpression)
        {

            Dictionary<string, object> param = new Dictionary<string, object>();

            StringBuilder build = editGenerate.UpdateCommandString(data, param);

            build.AppendLine();
            build.Append(whereGnerate.Explain(whereExpression, param));

            this.Log($"{MethodBase.GetCurrentMethod().Name}-invoke:\n{build}");

            return conn.ExecuteAsync(build.ToString(), param);

        }

    }
}
