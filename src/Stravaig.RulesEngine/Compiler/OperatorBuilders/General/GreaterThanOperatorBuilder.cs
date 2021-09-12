using System.Linq.Expressions;

namespace Stravaig.RulesEngine.Compiler.OperatorBuilders.General
{
    /// <summary>
    /// Represents the > (GreaterThan) operator.
    /// </summary>
    public class GreaterThanOperatorBuilder : OperatorBuilder
    {
        private static readonly string[] _operatorNames = { ">", "GreaterThan" };

        /// <inheritdoc />
        public override string[] OperatorNames => _operatorNames;

        /// <inheritdoc />
        public override Expression Build(Expression left, string right)
        {
            return Expression.GreaterThan(left, GetRightExpressionFromConstantValue(right, left.Type));
        }
    }
}