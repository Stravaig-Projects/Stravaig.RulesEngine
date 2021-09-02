using System.Linq.Expressions;

namespace Stravaig.RulesEngine.OperatorHandlers
{
    /// <summary>
    /// Represents the == (Equals) operator.
    /// </summary>
    public class EqualsOperatorHandler : OperatorHandler
    {
        private static readonly string[] _operatorNames = { "==", "Equals" };

        /// <inheritdoc />
        public override string[] OperatorNames => _operatorNames;

        /// <inheritdoc />
        public override Expression Handle(Expression left, Expression right)
        {
            return Expression.Equal(left, right);
        }
    }
}