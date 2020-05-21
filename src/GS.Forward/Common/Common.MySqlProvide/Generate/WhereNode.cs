using System;
using System.Linq.Expressions;

namespace Common.MySqlProvide.Generate
{
    /// <summary>
    /// @auth : monster
    /// @since : 5/21/2020 2:39:44 PM
    /// @source : 
    /// @des : 
    /// </summary>
    public partial class WhereGenerateVisitor
    {

        /// <summary>
        /// 单向链表
        /// </summary>
        private sealed class WhereNode
        {

            public WhereNode() { }

            public WhereNode(string val) { Val = val; }
            public WhereNode(ExpressionType type) { OptType = type; }

            public string Val { get; set; }

            public ExpressionType? OptType { get; set; }

            public WhereNode Next { get; set; }

            public override string ToString()
            {

                if (Next == null) return Val;

                string right = Next.ToString();

                switch (OptType)
                {
                    case ExpressionType.AndAlso:
                    case ExpressionType.And:
                        return $"{Val} AND {right}";
                    case ExpressionType.Or:
                        return $"({Val} OR {right})";
                    case ExpressionType.Equal:
                        if (right == null)
                            return $"{Val} IS NULL";
                        else
                            return $"{Val} = {right}";
                    case ExpressionType.NotEqual:
                        if (right == null)
                            return $"{Val} IS NOT NULL";
                        else
                            return $"{Val} <> {right}";
                    case ExpressionType.LessThan:
                        return $"{Val} < {right}";
                    case ExpressionType.LessThanOrEqual:
                        return $"{Val} <= {right}";
                    case ExpressionType.GreaterThan:
                        return $"{Val} > {right}";
                    case ExpressionType.GreaterThanOrEqual:
                        return $"{Val} >= {right}";
                    case ExpressionType.Add:
                        return $"({Val} + {right})";
                    default:
                        throw new NotSupportedException(string.Format("运算符{0}不支持", OptType));
                }
            }
        }

    }
}
