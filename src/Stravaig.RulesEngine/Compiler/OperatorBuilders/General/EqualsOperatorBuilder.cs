using System.Linq.Expressions;

namespace Stravaig.RulesEngine.Compiler.OperatorBuilders
{
    /// <summary>
    /// Represents the == (Equals) operator.
    /// </summary>
    public class EqualsOperatorBuilder : OperatorBuilder
    {
        internal static readonly string[] StandardOperatorNames = { "==", "Equals" };

        /// <inheritdoc />
        public override string[] OperatorNames => StandardOperatorNames;

        /// <inheritdoc />
        public override Expression Build(Expression left, string right)
        {
            return Expression.Equal(left, GetRightExpressionFromConstantValue(right, left.Type));
        }
    }
}