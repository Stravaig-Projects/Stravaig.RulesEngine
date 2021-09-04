using System.Linq.Expressions;

namespace Stravaig.RulesEngine.Compiler.OperatorBuilders
{
    /// <summary>
    /// Represents the >= (GreaterThanOrEqual) operator.
    /// </summary>
    public class GreaterThanOrEqualOperatorBuilder : OperatorBuilder
    {
        private static readonly string[] _operatorNames = { ">=", "GreaterThanOrEqual" };

        /// <inheritdoc />
        public override string[] OperatorNames => _operatorNames;

        /// <inheritdoc />
        public override Expression Build(Expression left, string right)
        {
            return Expression.GreaterThanOrEqual(left, GetRightExpessionFromConstantValue(right, left.Type));
        }
    }
}