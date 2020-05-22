using Common.Core.CusStruct;
using Common.MySqlProvide.CusAttr;
using System;
using System.Collections.Generic;
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
    public partial class WhereParameterGenerateVisitor : ExpressionVisitor
    {

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
                this.Append(c.Value);
            }
            return c;
        }

        protected override Expression VisitMember(MemberExpression m)
        {
            if (m.Expression != null)
            {
                if (m.Expression.NodeType == ExpressionType.Parameter)
                {
                    AliasAttribute alias = m.Member.GetCustomAttribute<AliasAttribute>();

                    alias = alias ?? new AliasAttribute(m.Member.Name);

                    // *** 此处添加AliasAttribute 方便解析时区分是取自源列还是参数值
                    this.Append(alias);

                    return m;
                }
                else if (m.Expression.NodeType == ExpressionType.Constant)
                {// 获取局部变量
                    var @object = ((ConstantExpression)m.Expression).Value; //这个是重点

                    if (m.Member.MemberType == MemberTypes.Field)
                    {
                        var value = ((FieldInfo)m.Member).GetValue(@object);
                        this.Append(value);
                        return m;
                    }
                    else if (m.Member.MemberType == MemberTypes.Property)
                    {
                        var value = ((PropertyInfo)m.Member).GetValue(@object);
                        this.Append(value);
                        return m;
                    }
                }
                else if (m.Expression.NodeType == ExpressionType.MemberAccess)
                {// TODO 获取对象属性值

                    MemberExpression outerMember = m;
                    PropertyInfo outerProp = (PropertyInfo)outerMember.Member;
                    MemberExpression innerMember = (MemberExpression)outerMember.Expression;
                    FieldInfo innerField = (FieldInfo)innerMember.Member;
                    ConstantExpression ce = (ConstantExpression)innerMember.Expression;
                    object innerObj = ce.Value;
                    object outerObj = innerField.GetValue(innerObj);
                    object value = outerProp.GetValue(outerObj, null);
                    this.Append(value);
                    return m;

                }
            }
            throw new NotSupportedException(string.Format("成员{0}不支持", m.Member.Name));
            //return base.VisitMember(m);
        }

    }

    public partial class WhereParameterGenerateVisitor : IWhereParameterGenerate
    {

        public string Explain(Expression expression, IDictionary<string, object> param)
        {
            // 参数清空
            head = node = null;
            this.param = param;

            Visit(expression);

            if (head == null) return null;

            return $"WHERE {Analysis(head)}";

        }

        private LinkNode<object, ExpressionType?> head, node;

        private void SetOptType(ExpressionType type)
        {
            if (head == null) head = node = new LinkNode<object, ExpressionType?>(type);
            else if (node.Flag == null) node.Flag = type;
            else
            {
                if (node.Flag == ExpressionType.Not)
                {
                    if (type == ExpressionType.Equal)
                        node.Flag = ExpressionType.NotEqual;
                    else if (type == ExpressionType.NotEqual)
                        node.Flag = ExpressionType.Equal;
                    else if (type == ExpressionType.LessThan)
                        node.Flag = ExpressionType.GreaterThanOrEqual;
                    else if (type == ExpressionType.GreaterThan)
                        node.Flag = ExpressionType.LessThanOrEqual;
                    else if (type == ExpressionType.GreaterThanOrEqual)
                        node.Flag = ExpressionType.LessThan;
                    else if (type == ExpressionType.LessThanOrEqual)
                        node.Flag = ExpressionType.GreaterThan;
                    else throw new NotSupportedException(string.Format("运算{0}不支持 NOT ", type));
                }
            }
        }

        private void Append(object value)
        {
            if (head == null) head = node = new LinkNode<object, ExpressionType?>(value);
            else if (node.Val == null) node.Val = value;
            else node = node.Next = new LinkNode<object, ExpressionType?>(value);
        }

        IDictionary<string, object> param;

        private string GetKey(object val)
        {
            // 若是where 中的 筛选值取自原有列
            if (val is AliasAttribute alias) return alias.Name;

            var key = $"p__{param.Count}";
            param.Add(key, val);
            return $"@{key}";
        }

        private object Analysis(LinkNode<object, ExpressionType?> node)
        {

            if (node.Next == null)
            {
                return GetKey(node.Val);
            }

            object right = Analysis(node.Next);

            switch (node.Flag)
            {
                case ExpressionType.AndAlso:
                case ExpressionType.And:
                    return $"{GetKey(node.Val)} AND {right}";
                case ExpressionType.Or:
                    return $"{GetKey(node.Val)} OR {right}";
                case ExpressionType.Equal:
                    if (right == null)
                        return $"{GetKey(node.Val)} IS NULL";
                    else
                        return $"{GetKey(node.Val)} = {right}";
                case ExpressionType.NotEqual:
                    if (right == null)
                        return $"{GetKey(node.Val)} IS NOT NULL";
                    else
                        return $"{GetKey(node.Val)} <> {right}";
                case ExpressionType.LessThan:
                    return $"{GetKey(node.Val)} < {GetKey(right)}";
                case ExpressionType.LessThanOrEqual:
                    return $"{GetKey(node.Val)} <= {GetKey(right)}";
                case ExpressionType.GreaterThan:
                    return $"{GetKey(node.Val)} > {GetKey(right)}";
                case ExpressionType.GreaterThanOrEqual:
                    return $"{GetKey(node.Val)} >= {GetKey(right)}";
                case ExpressionType.Add:
                    return $"({GetKey(GetKey(node.Val))} + {GetKey(right)})";
                default:
                    throw new NotSupportedException(string.Format("运算符{0}不支持", node.Flag));
            }
        }

    }

}
