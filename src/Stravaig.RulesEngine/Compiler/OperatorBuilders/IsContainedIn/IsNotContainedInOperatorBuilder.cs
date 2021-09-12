using System.Linq.Expressions;

namespace Stravaig.RulesEngine.Compiler.OperatorBuilders.IsContainedIn
{
    public abstract class IsNotContainedInOperatorBuilder<T> : IsContainedInOperatorBuilder<T>
    {
        protected override string OperatorName => "IsNotContainedIn";

        public override Expression Build(Expression leftPropertyExpression, string rightValueAsString)
        {
            var expression = base.Build(leftPropertyExpression, rightValueAsString);
            return Expression.Not(expression);
        }
    }
}