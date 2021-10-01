using System;
using System.Linq.Expressions;
using Stravaig.RulesEngine.Compiler.OperatorBuilders.General;

namespace Stravaig.RulesEngine.Compiler.OperatorBuilders.String
{
    public class StringEqualsOperatorBuilder : OperatorBuilder
    {
        public StringEqualsOperatorBuilder()
            : base(typeof(string))
        {
            
        }

        public override string[] OperatorNames => EqualsOperatorBuilder.StandardOperatorNames;

        public override Expression Build(Expression leftPropertyExpression, string rightValueAsString, Enum[] modifiers)
        {
            var modifier = GetModifier(modifiers, StringComparison.Ordinal);
            return Build(leftPropertyExpression, rightValueAsString, modifier);
        }

        protected virtual Expression Build(Expression leftPropertyExpression, string rightValueAsString, StringComparison stringComparison)
        {
            // result|> left == null ? (right == null ? true : false) : left.Equals(right, stringComparison);
            // reduced to:
            // result|> left == null ? right == null : left.Equals(right, stringComparison);
            
            // left == null
            var nullString = Expression.Constant(null, typeof(string));
            var leftIsNull = Expression.Equal(leftPropertyExpression, nullString);
            
            // right == null
            var rightIsNull = Expression.Equal(Expression.Constant(rightValueAsString), nullString);
            
            var equalsExpression = BuildEqualsExpression(leftPropertyExpression, rightValueAsString, stringComparison);
            return Expression.Condition(leftIsNull, rightIsNull, equalsExpression);
        }

        protected static Expression BuildEqualsExpression(Expression leftPropertyExpression, string rightValueAsString, StringComparison stringComparison)
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