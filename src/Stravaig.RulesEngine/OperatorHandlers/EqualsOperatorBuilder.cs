using System.Linq.Expressions;

namespace Stravaig.RulesEngine.OperatorHandlers
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
        public override Expression Build(Expression left, Expression right)
        {
            return Expression.Equal(left, right);
        }
    }
}