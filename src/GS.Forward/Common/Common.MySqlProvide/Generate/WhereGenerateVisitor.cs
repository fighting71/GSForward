using Common.MySqlProvide.CusAttr;
using System;
using System.Linq.Expressions;
using System.Reflection;

namespace Common.MySqlProvide.Generate
{

    /// <summary>
    /// @auth : monster
    /// @since : 5/20/2020 4:28:15 PM
    /// @source : 
    /// @des : 构建WHERE
    /// </summary>
    public partial class WhereGenerateVisitor : ExpressionVisitor,IWhereGnerate
    {

        public string Explain(Expression expression)
        {
            Visit(expression);

            if (head == null) return null;

            return $"WHERE {head.ToString()}";

        }


        protected override Expression VisitUnary(UnaryExpression u)
        {
            SetOptType(u.NodeType);
            return u;
        }

        protected override Expression VisitBinary(BinaryExpression expression)
        {

            this.Visit(expression.Left);

            SetOptType(expression.NodeType);

            this.Visit(expression.Right);
            return expression;
        }

        protected override Expression VisitConstant(ConstantExpression c)
        {
            if (c.Value == null)
            {
                this.Append(null);
            }
            else
            {
                switch (Type.GetTypeCode(c.Value.GetType()))
                {
                    case TypeCode.Boolean:
                        this.Append((((bool)c.Value) ? 1 : 0).ToString());
                        break;
                    case TypeCode.String:
                        this.Append($"'{c.Value}'");
                        break;
                    case TypeCode.DateTime:
                        this.Append($"'{(DateTime)c.Value:yyyy-MM-dd hh:mm:ss ffff}'");
                        break;
                    case TypeCode.Object:
                        throw new NotSupportedException(string.Format("常量{0}不支持", c.Value));
                        break;
                    default:
                        this.Append(c.Value.ToString());
                        break;
                }
            }
            return c;
        }

        protected override Expression VisitMember(MemberExpression m)
        {
            if (m.Expression != null && m.Expression.NodeType == ExpressionType.Parameter)
            {
                if(m.Expression.NodeType == ExpressionType.Parameter)
                {
                    AliasAttribute alias = m.Member.GetCustomAttribute<AliasAttribute>();

                    if (alias != null)
                        this.Append(alias.Name);
                    else
                        this.Append(m.Member.Name);
                    return m;
                }
                else if(m.Expression.NodeType == ExpressionType.Constant)
                {// 获取局部变量
                    var @object =((ConstantExpression)m.Expression).Value; //这个是重点

                    if (m.Member.MemberType == MemberTypes.Field)
                    {
                        var value = ((FieldInfo)m.Member).GetValue(@object);
                        this.Append(value.ToString());
                        return m;
                    }
                    else if (m.Member.MemberType == MemberTypes.Property)
                    {
                        var value = ((PropertyInfo)m.Member).GetValue(@object);
                        this.Append(value.ToString());
                        return m;
                    }
                }
                else if (m.Expression.NodeType == ExpressionType.MemberAccess)
                {// TODO 获取对象值

                    var type = m.Expression.GetType();

                    var @object = ((MemberExpression)m.Expression).Member; //这个是重点

                    if (m.Member.MemberType == MemberTypes.Field)
                    {
                        var value = ((FieldInfo)m.Member).GetValue(@object);
                        this.Append(value.ToString());
                        return m;
                    }
                    else if (m.Member.MemberType == MemberTypes.Property)
                    {
                        var value = ((PropertyInfo)m.Member).GetValue(@object);
                        this.Append(value.ToString());
                        return m;
                    }

                }
            }
            throw new NotSupportedException(string.Format("成员{0}不支持", m.Member.Name));
            //return base.VisitMember(m);
        }

    }

    public partial class WhereGenerateVisitor
    {

        private WhereNode head, node;

        private void SetOptType(ExpressionType type)
        {
            if (head == null) head = node = new WhereNode(type);
            else if (node.OptType == null) node.OptType = type;
            else
            {
                if (node.OptType == ExpressionType.Not)
                {
                    if (type == ExpressionType.Equal)
                        node.OptType = ExpressionType.NotEqual;
                    else if (type == ExpressionType.NotEqual)
                        node.OptType = ExpressionType.Equal;
                    else if (type == ExpressionType.LessThan)
                        node.OptType = ExpressionType.GreaterThanOrEqual;
                    else if (type == ExpressionType.GreaterThan)
                        node.OptType = ExpressionType.LessThanOrEqual;
                    else if (type == ExpressionType.GreaterThanOrEqual)
                        node.OptType = ExpressionType.LessThan;
                    else if (type == ExpressionType.LessThanOrEqual)
                        node.OptType = ExpressionType.GreaterThan;
                    else throw new NotSupportedException(string.Format("运算{0}不支持 NOT ", type));
                }
            }
        }

        private void Append(string str)
        {
            if (head == null) head = node = new WhereNode(str);
            else if (node.Val == null) node.Val = str;
            else node = node.Next = new WhereNode(str);
        }
    }

}
