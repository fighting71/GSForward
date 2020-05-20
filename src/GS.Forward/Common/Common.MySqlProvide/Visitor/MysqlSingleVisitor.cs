using Common.MySqlProvide.CusAttr;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading;

namespace Common.MySqlProvide.Visitor
{
    /// <summary>
    /// @auth : monster
    /// @since : 5/20/2020 4:28:15 PM
    /// @source : 
    /// @des : mysql 单表查询visitor
    /// TODO: EXISTS/IS NULL/IS NOT NULL
    /// </summary>
    public class MysqlSingleVisitor : ExpressionVisitor, IGenerateSql
    {
        private ThreadLocal<StringBuilder> threadLocal = new ThreadLocal<StringBuilder>(() =>
        {
            return new StringBuilder();
        });

        private StringBuilder builder {get{ return threadLocal.Value; } }

        public StringBuilder GetSql() => builder;
        public StringBuilder TranslateWhere(Expression expression)
        {
            builder.AppendLine(" WHERE");
            this.Visit(expression);
            return builder;
        }

        public StringBuilder TranslateForm<T>(IQueryable<T> source)
        {
            if (threadLocal.IsValueCreated)
            {
                builder.Clear();
            }

            builder.Append("SELECT * FROM ");
            this.Visit(source.Expression);
            return builder;
        }

        protected override Expression VisitMethodCall(MethodCallExpression expression)
        {
            this.Visit(expression);
            return expression;
        }

        protected override Expression VisitUnary(UnaryExpression u)
        {
            switch (u.NodeType)
            {
                //case ExpressionType.Not:
                //    builder.Append(" NOT ");
                //    this.Visit(u.Operand);
                //    break;
                default:
                    throw new NotSupportedException(string.Format("运算{0}不支持", u.NodeType));
            }
            return u;
        }

        protected override Expression VisitBinary(BinaryExpression expression)
        {
            builder.Append("(");
            this.Visit(expression.Left);
            switch (expression.NodeType)
            {
                case ExpressionType.AndAlso:
                case ExpressionType.And:
                    builder.Append(" AND ");
                    break;
                case ExpressionType.Or:
                case ExpressionType.OrElse:
                    builder.Append(" OR");
                    break;
                case ExpressionType.Equal:
                    builder.Append(" = ");
                    break;
                case ExpressionType.NotEqual:
                    builder.Append(" <> ");
                    break;
                case ExpressionType.LessThan:
                    builder.Append(" < ");
                    break;
                case ExpressionType.LessThanOrEqual:
                    builder.Append(" <= ");
                    break;
                case ExpressionType.GreaterThan:
                    builder.Append(" > ");
                    break;
                case ExpressionType.GreaterThanOrEqual:
                    builder.Append(" >= ");
                    break;
                default:
                    throw new NotSupportedException(string.Format("运算符{0}不支持", expression.NodeType));
            }
            this.Visit(expression.Right);
            builder.Append(")");
            return expression;
        }

        private void VisitAndAlso(BinaryExpression expression)
        {
            Visit(expression.Left);
            Visit(expression.Right);
        }

        protected override Expression VisitConstant(ConstantExpression c)
        {
            IQueryable q = c.Value as IQueryable;
            if (q != null)
            {
                // 单表直接返回

                AliasAttribute aliasAttribute = q.ElementType.GetCustomAttribute<AliasAttribute>();

                if(aliasAttribute != null)
                {
                    builder.Append(aliasAttribute.Name);
                }
                else
                {
                    builder.Append(q.ElementType.Name);
                }

            }
            else if (c.Value == null)
            {
                builder.Append("NULL");
            }
            else
            {
                switch (Type.GetTypeCode(c.Value.GetType()))
                {
                    case TypeCode.Boolean:
                        builder.Append(((bool)c.Value) ? 1 : 0);
                        break;
                    case TypeCode.String:
                        builder.Append("'");
                        builder.Append(c.Value);
                        builder.Append("'");
                        break;
                    case TypeCode.Object:
                        throw new NotSupportedException(string.Format("常量{0}不支持", c.Value));
                    default:
                        builder.Append(c.Value);
                        break;
                }
            }
            return c;
        }

        protected override Expression VisitMember(MemberExpression m)
        {
            if (m.Expression != null && m.Expression.NodeType == ExpressionType.Parameter)
            {

                AliasAttribute alias = m.Member.GetCustomAttribute<AliasAttribute>();

                if(alias != null)
                    builder.Append(alias.Name);
                else 
                    builder.Append(m.Member.Name);
                return m;
            }
            throw new NotSupportedException(string.Format("成员{0}不支持", m.Member.Name));
        }

    }
}
