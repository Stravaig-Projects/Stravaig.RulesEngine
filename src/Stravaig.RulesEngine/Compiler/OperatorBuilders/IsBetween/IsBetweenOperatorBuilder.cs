using System;
using System.Linq.Expressions;

namespace Stravaig.RulesEngine.Compiler.OperatorBuilders.IsBetween
{
    public abstract class IsBetweenOperatorBuilder<T> : OperatorBuilder
    {
        private const char Separator = '|';
        protected override string OperatorName => "IsBetween";

        protected IsBetweenOperatorBuilder()
            : base(typeof(T))
        {
        }
        
        public override Expression Build(Expression leftPropertyExpression, string rightValueAsString)
        {
            (T start, T end) = GetBounds(rightValueAsString);

            var startExpression = Expression.Constant(start);
            var geExpression = Expression.GreaterThanOrEqual(leftPropertyExpression, startExpression);

            var endExpression = Expression.Constant(end);
            var leExpression = Expression.LessThanOrEqual(leftPropertyExpression, endExpression);

            return Expression.AndAlso(geExpression, leExpression);
        }

        private (T, T) GetBounds(string rightValueAsString)
        {
            var index = rightValueAsString.IndexOf(Separator);
            if (index < 0)
                throw new InvalidOperationException($"Expected two values separated by a '{Separator}'. No separator found.");

            var lastIndex = rightValueAsString.LastIndexOf(Separator);
            if(lastIndex != index)
                throw new InvalidOperationException($"Expected two values separated by a '{Separator}'. More than one separator found.");

            string startString = rightValueAsString.Substring(0, index);
            T start = (T)Convert.ChangeType(startString, typeof(T));
            string endString = rightValueAsString.Substring(index + 1);
            T end = (T)Convert.ChangeType(endString, typeof(T));
            return (start, end);
        }
    }
}