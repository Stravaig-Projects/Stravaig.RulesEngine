using System;
using System.Linq.Expressions;

namespace Stravaig.RulesEngine.Compiler.OperatorBuilders.String
{
    public abstract class StringEqualsOperatorBuilder : OperatorBuilder
    {
        protected StringEqualsOperatorBuilder()
            : base(typeof(string))
        {
            
        }
        
        protected virtual Expression Build(Expression leftPropertyExpression, string rightValueAsString, StringComparison stringComparison)
        {
            // result: left.Equals(right, stringComparison)
            var type = typeof(string);
            var equalsMethod = type.GetMethod(
                nameof(rightValueAsString.Equals), 
                new[]
                {
                    typeof(string),
                    typeof(StringComparison)
                });
            if (equalsMethod == null)
                throw new InvalidOperationException("string.Equals(string, StringComparison) was expected to exist.");
            
            var rightExpression = Expression.Constant(rightValueAsString);
            var comparisonExpression = Expression.Constant(stringComparison);

            var result = Expression.Call(
                leftPropertyExpression, // string property
                equalsMethod, // Equals(string, StringComparison) method
                rightExpression, // string arg0
                comparisonExpression); // StringComparison arg1

            return result;
        }
    }
}