using Common.MySqlProvide.CusAttr;
using System;
using System.Collections.Generic;
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
    /// @des : debug
    /// </summary>
    public class TestVisitor : ExpressionVisitor
    {

        protected override Expression VisitMember(MemberExpression node)
        {
            Console.WriteLine($"call {MethodBase.GetCurrentMethod().Name} : {node}");
            return node;
            //return base.VisitMember(node);
        }

        public override Expression Visit(Expression node)
        {
            return base.Visit(node);
        }

        protected override Expression VisitBinary(BinaryExpression node)
        {
            Console.WriteLine($"call {MethodBase.GetCurrentMethod().Name} : {node}");
            return base.VisitBinary(node);
        }

        protected override Expression VisitBlock(BlockExpression node)
        {
            Console.WriteLine($"call {MethodBase.GetCurrentMethod().Name} : {node}");
            return base.VisitBlock(node);
        }

        protected override CatchBlock VisitCatchBlock(CatchBlock node)
        {
            Console.WriteLine($"call {MethodBase.GetCurrentMethod().Name} : {node}");
            return base.VisitCatchBlock(node);
        }

        protected override Expression VisitConditional(ConditionalExpression node)
        {
            Console.WriteLine($"call {MethodBase.GetCurrentMethod().Name} : {node}");
            return base.VisitConditional(node);
        }

        protected override Expression VisitConstant(ConstantExpression node)
        {
            Console.WriteLine($"call {MethodBase.GetCurrentMethod().Name} : {node}");
            return base.VisitConstant(node);
        }

        protected override Expression VisitDebugInfo(DebugInfoExpression node)
        {
            Console.WriteLine($"call {MethodBase.GetCurrentMethod().Name} : {node}");
            return base.VisitDebugInfo(node);
        }

        protected override Expression VisitDefault(DefaultExpression node)
        {
            Console.WriteLine($"call {MethodBase.GetCurrentMethod().Name} : {node}");
            return base.VisitDefault(node);
        }

        protected override Expression VisitDynamic(DynamicExpression node)
        {
            Console.WriteLine($"call {MethodBase.GetCurrentMethod().Name} : {node}");
            return base.VisitDynamic(node);
        }

        protected override ElementInit VisitElementInit(ElementInit node)
        {
            Console.WriteLine($"call {MethodBase.GetCurrentMethod().Name} : {node}");
            return base.VisitElementInit(node);
        }

        protected override Expression VisitExtension(Expression node)
        {
            Console.WriteLine($"call {MethodBase.GetCurrentMethod().Name} : {node}");
            return base.VisitExtension(node);
        }

        protected override Expression VisitGoto(GotoExpression node)
        {
            Console.WriteLine($"call {MethodBase.GetCurrentMethod().Name} : {node}");
            return base.VisitGoto(node);
        }

        protected override Expression VisitIndex(IndexExpression node)
        {
            Console.WriteLine($"call {MethodBase.GetCurrentMethod().Name} : {node}");
            return base.VisitIndex(node);
        }

        protected override Expression VisitInvocation(InvocationExpression node)
        {
            Console.WriteLine($"call {MethodBase.GetCurrentMethod().Name} : {node}");
            return base.VisitInvocation(node);
        }

        protected override Expression VisitLabel(LabelExpression node)
        {
            Console.WriteLine($"call {MethodBase.GetCurrentMethod().Name} : {node}");
            return base.VisitLabel(node);
        }

        protected override LabelTarget VisitLabelTarget(LabelTarget node)
        {
            Console.WriteLine($"call {MethodBase.GetCurrentMethod().Name} : {node}");
            return base.VisitLabelTarget(node);
        }

        protected override Expression VisitLambda<T>(Expression<T> node)
        {
            Console.WriteLine($"call {MethodBase.GetCurrentMethod().Name} : {node}");
            return base.VisitLambda(node);
        }

        protected override Expression VisitListInit(ListInitExpression node)
        {
            Console.WriteLine($"call {MethodBase.GetCurrentMethod().Name} : {node}");
            return base.VisitListInit(node);
        }

        protected override Expression VisitLoop(LoopExpression node)
        {
            Console.WriteLine($"call {MethodBase.GetCurrentMethod().Name} : {node}");
            return base.VisitLoop(node);
        }

        protected override MemberAssignment VisitMemberAssignment(MemberAssignment node)
        {
            Console.WriteLine($"call {MethodBase.GetCurrentMethod().Name} : {node}");
            return base.VisitMemberAssignment(node);
        }

        protected override MemberBinding VisitMemberBinding(MemberBinding node)
        {
            Console.WriteLine($"call {MethodBase.GetCurrentMethod().Name} : {node}");
            return base.VisitMemberBinding(node);
        }

        protected override Expression VisitMemberInit(MemberInitExpression node)
        {
            Console.WriteLine($"call {MethodBase.GetCurrentMethod().Name} : {node}");
            return base.VisitMemberInit(node);
        }

        protected override MemberListBinding VisitMemberListBinding(MemberListBinding node)
        {
            Console.WriteLine($"call {MethodBase.GetCurrentMethod().Name} : {node}");
            return base.VisitMemberListBinding(node);
        }

        protected override MemberMemberBinding VisitMemberMemberBinding(MemberMemberBinding node)
        {
            Console.WriteLine($"call {MethodBase.GetCurrentMethod().Name} : {node}");
            return base.VisitMemberMemberBinding(node);
        }

        protected override Expression VisitMethodCall(MethodCallExpression node)
        {
            Console.WriteLine($"call {MethodBase.GetCurrentMethod().Name} : {node}");
            return base.VisitMethodCall(node);
        }

        protected override Expression VisitNew(NewExpression node)
        {
            Console.WriteLine($"call {MethodBase.GetCurrentMethod().Name} : {node}");
            return base.VisitNew(node);
        }

        protected override Expression VisitNewArray(NewArrayExpression node)
        {
            Console.WriteLine($"call {MethodBase.GetCurrentMethod().Name} : {node}");
            return base.VisitNewArray(node);
        }

        protected override Expression VisitParameter(ParameterExpression node)
        {
            Console.WriteLine($"call {MethodBase.GetCurrentMethod().Name} : {node}");
            return base.VisitParameter(node);
        }

        protected override Expression VisitRuntimeVariables(RuntimeVariablesExpression node)
        {
            Console.WriteLine($"call {MethodBase.GetCurrentMethod().Name} : {node}");
            return base.VisitRuntimeVariables(node);
        }

        protected override Expression VisitSwitch(SwitchExpression node)
        {
            Console.WriteLine($"call {MethodBase.GetCurrentMethod().Name} : {node}");
            return base.VisitSwitch(node);
        }

        protected override SwitchCase VisitSwitchCase(SwitchCase node)
        {
            Console.WriteLine($"call {MethodBase.GetCurrentMethod().Name} : {node}");
            return base.VisitSwitchCase(node);
        }

        protected override Expression VisitTry(TryExpression node)
        {
            Console.WriteLine($"call {MethodBase.GetCurrentMethod().Name} : {node}");
            return base.VisitTry(node);
        }

        protected override Expression VisitTypeBinary(TypeBinaryExpression node)
        {
            Console.WriteLine($"call {MethodBase.GetCurrentMethod().Name} : {node}");
            return base.VisitTypeBinary(node);
        }

        protected override Expression VisitUnary(UnaryExpression node)
        {
            Console.WriteLine($"call {MethodBase.GetCurrentMethod().Name} : {node}");
            return base.VisitUnary(node);
        }



    }
}
