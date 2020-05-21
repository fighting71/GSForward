using System;
using System.Linq;
using System.Linq.Expressions;
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

        public static StringBuilder Query<TSource, TResult>(this IQueryable<TSource> source,
            ISelectGnerate selectGnerate, IFrommGnerate formGnerate, IWhereGnerate whereGnerate,
            Expression<Func<TSource, TResult>> selectExpression, Expression<Func<TSource, bool>> whereExpression)
        {
            return QueryString(selectGnerate, formGnerate, whereGnerate, selectExpression, source.Expression, whereExpression);
        }

        public static StringBuilder QueryString<TSource, TResult>(
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
