using System;
using System.Linq.Expressions;

namespace Stravaig.RulesEngine.Compiler.OperatorBuilders.String
{
    public abstract class StringNotEqualsOperatorBuilder : StringEqualsOperatorBuilder
    {
        protected override Expression Build(Expression leftPropertyExpression, string rightValueAsString, StringComparison stringComparison)
        {
            // result|> left == null ? (right == null ? false : true) : !(left.Equals(right, stringComparison));
            // reduced to:
            // result|> left == null ? right != null : !left.Equals(right, stringComparison);
            
            // left == null
            var nullString = Expression.Constant(null, typeof(string));
            var leftIsNull = Expression.Equal(leftPropertyExpression, nullString);
            
            // right != null
            var rightIsNotNull = Expression.NotEqual(Expression.Constant(rightValueAsString), nullString);
            
            // !(left.Equals(right, stringComparison))
            var equalsExpression = BuildEqualsExpression(leftPropertyExpression, rightValueAsString, stringComparison);
            var notEqualsExpression = Expression.Not(equalsExpression);
            
            return Expression.Condition(leftIsNull, rightIsNotNull, notEqualsExpression);
        }
    }
}