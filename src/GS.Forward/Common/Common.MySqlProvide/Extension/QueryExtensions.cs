using Common.MySqlProvide.Visitor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

namespace Common.MySqlProvide.Extension
{
    /// <summary>
    /// @auth : monster
    /// @since : 5/20/2020 4:26:58 PM
    /// @source : 
    /// @des : 
    /// </summary>
    public static class QueryExtensions
    {

        public static StringBuilder GetSql<TSource>(this IQueryable<TSource> source,
            IGenerateSql visitor, Expression<Func<TSource, bool>> predicate)
        {
            visitor.TranslateForm(source);
            visitor.TranslateWhere(predicate);
            return visitor.GetSql();
        }

    }
}
