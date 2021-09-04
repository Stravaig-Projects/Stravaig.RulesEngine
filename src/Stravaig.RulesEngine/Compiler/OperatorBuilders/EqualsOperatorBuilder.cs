using System.Linq.Expressions;

namespace Stravaig.RulesEngine.Compiler.OperatorBuilders
{
    /// <summary>
    /// Represents the == (Equals) operator.
    /// </summary>
    public class EqualsOperatorBuilder : OperatorBuilder
    {
        private static readonly string[] _operatorNames = { "==", "Equals" };

        /// <inheritdoc />
        public override string[] OperatorNames => _operatorNames;

        /// <inheritdoc />
        public override Expression Build(Expression left, string right)
        {
            return Expression.Equal(left, GetRightExpessionFromConstantValue(right, left.Type));
        }
    }
}