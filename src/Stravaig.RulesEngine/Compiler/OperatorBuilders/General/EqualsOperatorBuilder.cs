using System;
using System.Linq.Expressions;

namespace Stravaig.RulesEngine.Compiler.OperatorBuilders.General
{
    /// <summary>
    /// Represents the == (Equals) operator.
    /// </summary>
    public class EqualsOperatorBuilder : OperatorBuilder
    {
        public static readonly string[] StandardOperatorNames = { "==", "Equals" };

        /// <inheritdoc />
        public override string[] OperatorNames => StandardOperatorNames;

        /// <inheritdoc />
        public override Expression Build(Expression left, string right, Enum[] modifiers)
        {
            return Expression.Equal(left, GetRightExpressionFromConstantValue(right, left.Type));
        }
    }
}