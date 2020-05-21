using Common.MySqlProvide.CusAttr;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading;

namespace Common.MySqlProvide.Generate
{
    /// <summary>
    /// @auth : monster
    /// @since : 5/20/2020 4:28:15 PM
    /// @source : 
    /// @des : 构建SELECT
    /// </summary>
    public class SelectGenerateVisitor : ExpressionVisitor,ISelectGnerate
    {

        private string[] _aliasMapper;
        IList<string> _fieldMapper;

        public string Explain(Expression expression)
        {

            _fieldMapper = new List<string>();
            _aliasMapper = null;

            Visit(expression);

            if (_fieldMapper.Count == 0)
                return "SELECT *";
            else return $"SELECT {string.Join(",", _fieldMapper)}";

        }

        protected override Expression VisitNew(NewExpression node)
        {

            _aliasMapper = node.Members.Select(u => u.Name).ToArray();

            return base.VisitNew(node);
        }

        protected override Expression VisitMember(MemberExpression m)
        {
            if (m.Expression != null && m.Expression.NodeType == ExpressionType.Parameter)
            {

                AliasAttribute alias = m.Member.GetCustomAttribute<AliasAttribute>();

                var field = alias?.Name ?? m.Member.Name;

                if (_aliasMapper != null)
                {
                    string mapper = _aliasMapper[_fieldMapper.Count];

                    if (mapper.Equals(field))
                        _fieldMapper.Add(field);
                    else
                        _fieldMapper.Add($"{field} AS {mapper}");
                }
                else
                {
                    _fieldMapper.Add(field);
                }
                return m;

            }
            throw new NotSupportedException(string.Format("成员{0}不支持", m.Member.Name));
        }

        protected override Expression VisitConstant(ConstantExpression node)
        {

            switch (Type.GetTypeCode(node.Value.GetType()))
            {
                case TypeCode.String:
                    _fieldMapper.Add($"'{node.Value}' AS {_aliasMapper[_fieldMapper.Count]}");
                    break;

                case TypeCode.Boolean:
                    _fieldMapper.Add($"{(((bool)node.Value) ? 1 : 0)} AS {_aliasMapper[_fieldMapper.Count]}");
                    break;
                default:
                    _fieldMapper.Add($"{node.Value} AS {_aliasMapper[_fieldMapper.Count]}");
                    break;
            }

            return node;
        }

    }
}
