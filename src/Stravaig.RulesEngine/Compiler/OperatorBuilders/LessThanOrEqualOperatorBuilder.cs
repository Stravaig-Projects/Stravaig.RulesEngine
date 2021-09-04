using System.Linq.Expressions;

namespace Stravaig.RulesEngine.Compiler.OperatorBuilders
{
    /// <summary>
    /// Represents the <= (LessThanOrEqual) operator.
    /// </summary>
    public class LessThanOrEqualOperatorBuilder : OperatorBuilder
    {
        private static readonly string[] _operatorNames = { "<=", "LessThanOrEqual" };

        /// <inheritdoc />
        public override string[] OperatorNames => _operatorNames;

        /// <inheritdoc />
        public override Expression Build(Expression left, Expression right)
        {
            return Expression.LessThanOrEqual(left, right);
        }
    }
}