using System;
using System.Linq.Expressions;

namespace Stravaig.RulesEngine.Compiler.OperatorBuilders.General
{
    /// <summary>
    /// Represents the == (Equals) operator.
    /// </summary>
    public class NotEqualsOperatorBuilder : OperatorBuilder
    {
        public static readonly string[] StandardOperatorNames = { "!=", "NotEquals" };

        /// <inheritdoc />
        public override string[] OperatorNames => StandardOperatorNames;

        /// <inheritdoc />
        public override Expression Build(Expression left, string right, Enum[] modifiers)
        {
            return Expression.NotEqual(left, GetRightExpressionFromConstantValue(right, left.Type));
        }
    }
}