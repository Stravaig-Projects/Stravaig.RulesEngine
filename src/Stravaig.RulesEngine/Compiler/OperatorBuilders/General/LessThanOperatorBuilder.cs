using System.Linq.Expressions;

namespace Stravaig.RulesEngine.Compiler.OperatorBuilders.General
{
    /// <summary>
    /// Represents the < (LessThan) operator.
    /// </summary>
    public class LessThanOperatorBuilder : OperatorBuilder
    {
        private static readonly string[] _operatorNames = { "<", "LessThan" };

        /// <inheritdoc />
        public override string[] OperatorNames => _operatorNames;

        /// <inheritdoc />
        public override Expression Build(Expression left, string right)
        {
            return Expression.LessThan(left, GetRightExpressionFromConstantValue(right, left.Type));
        }
    }
}