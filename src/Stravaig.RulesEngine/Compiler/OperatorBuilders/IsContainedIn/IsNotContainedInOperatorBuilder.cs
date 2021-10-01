using System;
using System.Linq.Expressions;

namespace Stravaig.RulesEngine.Compiler.OperatorBuilders.IsContainedIn
{
    public abstract class IsNotContainedInOperatorBuilder<T> : IsContainedInOperatorBuilder<T>
    {
        protected override string OperatorName => "IsNotContainedIn";

        public override Expression Build(Expression leftPropertyExpression, string rightValueAsString, Enum[] modifiers)
        {
            var expression = base.Build(leftPropertyExpression, rightValueAsString, modifiers);
            return Expression.Not(expression);
        }
    }
}