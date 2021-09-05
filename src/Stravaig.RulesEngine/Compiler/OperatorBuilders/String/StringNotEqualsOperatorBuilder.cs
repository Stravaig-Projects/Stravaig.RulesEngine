using System;
using System.Linq.Expressions;

namespace Stravaig.RulesEngine.Compiler.OperatorBuilders.String
{
    public abstract class StringNotEqualsOperatorBuilder : StringEqualsOperatorBuilder
    {
        protected override Expression Build(Expression leftPropertyExpression, string rightValueAsString, StringComparison stringComparison)
        {
            // result: !(left.Equals(right, stringComparison))
            var equalsExpression = base.Build(leftPropertyExpression, rightValueAsString, stringComparison);
            var result = Expression.Not(equalsExpression);
            return result;
        }
    }
}