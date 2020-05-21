using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace GS.AppContext
{
    /// <summary>
    /// @auth : monster
    /// @since : 5/21/2020 3:08:57 PM
    /// @source : 
    /// @des : 
    /// </summary>
    public interface IQueryContext
    {

        Task<IEnumerable<T>> QueryAsync<T>(Expression<Func<T, bool>> whereExpression);

        IEnumerable<T> Query<T>(Expression<Func<T, bool>> whereExpression);

        Task<bool> AnyAsync<T>(Expression<Func<T, bool>> whereExpression);

        bool Any<T>(Expression<Func<T, bool>> whereExpression);

        int Add<T>(T data);

        Task<int> AddAndGetKeyAsync<T>(T data);

        int AddAndGetKey<T>(T data);

    }
}
