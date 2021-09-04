using System.Linq.Expressions;

namespace Stravaig.RulesEngine.Compiler.OperatorBuilders
{
    /// <summary>
    /// Represents the == (Equals) operator.
    /// </summary>
    public class NotEqualsOperatorBuilder : OperatorBuilder
    {
        private static readonly string[] _operatorNames = { "!=", "NotEquals" };

        /// <inheritdoc />
        public override string[] OperatorNames => _operatorNames;

        /// <inheritdoc />
        public override Expression Build(Expression left, string right)
        {
            return Expression.NotEqual(left, GetRightExpessionFromConstantValue(right, left.Type));
        }
    }
}