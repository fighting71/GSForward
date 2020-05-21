using Common.MySqlProvide.CusAttr;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace Common.MySqlProvide.Generate
{
    /// <summary>
    /// @auth : monster
    /// @since : 5/20/2020 4:28:15 PM
    /// @source : 
    /// @des : 构建FROM
    /// </summary>
    public class FromGenerateVisitor : ExpressionVisitor, IFrommGnerate
    {

        IList<string> _tables;
        public string Explain(Expression expression)
        {

            _tables = new List<string>();

            Visit(expression);

            return $"FROM {string.Join(",", _tables)}";

        }

        protected override Expression VisitConstant(ConstantExpression c)
        {

            Type type = c.Value as Type;

            if (type == null)
                throw new NotSupportedException($"尚不支持 {c.Value}");

            AliasAttribute aliasAttribute = type.GetCustomAttribute<AliasAttribute>();

            if (aliasAttribute != null)
                _tables.Add(aliasAttribute.Name);
            else
                _tables.Add(type.Name);

            return c;
        }

    }
}
