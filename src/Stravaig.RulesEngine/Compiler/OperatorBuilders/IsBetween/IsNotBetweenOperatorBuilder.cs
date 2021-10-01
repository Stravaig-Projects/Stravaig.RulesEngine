using System;
using System.Linq.Expressions;

namespace Stravaig.RulesEngine.Compiler.OperatorBuilders.IsBetween
{
    public abstract class IsNotBetweenOperatorBuilder<T> : IsBetweenOperatorBuilder<T>
    {
        protected override string OperatorName => "IsNotBetween";

        public override Expression Build(Expression leftPropertyExpression, string rightValueAsString, Enum[] modifiers)
        {
            return Expression.Not(base.Build(leftPropertyExpression, rightValueAsString, modifiers));
        }
    }
}