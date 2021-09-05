using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Stravaig.RulesEngine.Compiler.OperatorBuilders
{
    /// <summary>
    /// Represents a binary operator that takes a left and right part and
    /// results in a Boolean.
    /// </summary>
    public abstract class OperatorBuilder
    {
        public OperatorBuilder()
        {
        }

        public OperatorBuilder(Type leftType)
        {
            LeftType = leftType;
        }
        /// <summary>
        /// The single name of this operator
        /// </summary>
        /// <remarks>Override this only if the handler has a single name.</remarks>
        protected virtual string OperatorName => string.Empty;
        
        /// <summary>
        /// The names of this operator. If not overriden will by be array
        /// containing a single element with the value of
        /// <see cref="OperatorName"/>.
        /// </summary>
        /// <remarks>Override this only the handler has multiple names.</remarks>
        public virtual string[] OperatorNames => new[] { OperatorName };
        
        public Type? LeftType { get; }
        
        public Type? RightType { get; }
        
        /// <summary>
        /// Builds the expression that evaluates the left expression against the
        /// right expression.
        /// </summary>
        /// <param name="left">The expression on the left of the operator.</param>
        /// <param name="right">The expression on the right of the operator.</param>
        /// <returns>An expression that evaluates left against right</returns>
        public abstract Expression Build(Expression leftPropertyExpression, string rightValueAsString);

        protected ConstantExpression GetRightExpressionFromConstantValue(object value, Type propertyType)
        {
            object convertedValue = Convert.ChangeType(value, RightType ?? propertyType);
            var valueExpression = Expression.Constant(convertedValue);
            return valueExpression;
        }

        public bool CanMatchType(Type desiredLeftType)
        {
            return LeftType == null ||
                   LeftType == desiredLeftType ||
                   desiredLeftType.IsSubclassOf(LeftType);
        }


        public static IComparer<Type?> CompareWithRespectTo(Type desiredLeftType)
        {
            // 1 = This instance is greater than other
            // 0 = This instance is the same as other
            // -1 = this instance is less than other.

            return new OperatorBuilderTypeComparer(desiredLeftType);
        }
    }
}