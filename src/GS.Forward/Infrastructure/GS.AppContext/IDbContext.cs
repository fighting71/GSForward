using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace GS.AppContext
{
    /// <summary>
    /// @auth : monster
    /// @since : 5/21/2020 3:08:57 PM
    /// @source : 
    /// @des : 
    /// </summary>
    public interface IDbContext
    {

        Task<TResult> QueryFirstOrDefaultAsync<T, TResult>(Expression<Func<T, TResult>> selectExpression, Expression<Func<T, bool>> whereExpression);

        Task<IEnumerable<TResult>> QueryAsync<T,TResult>(Expression<Func<T,TResult>> selectExpression,Expression<Func<T, bool>> whereExpression);

        Task<IEnumerable<T>> QueryAsync<T>(Expression<Func<T, bool>> whereExpression);

        Task<bool> AnyAsync<T>(Expression<Func<T, bool>> whereExpression);

        Task<int> AddAsync<T>(T data);

        Task<int> UpdateAsync<T>(T data, Expression<Func<T, bool>> whereExpression);

        Task<int> AddAndGetKeyAsync<T>(T data);

    }
}
