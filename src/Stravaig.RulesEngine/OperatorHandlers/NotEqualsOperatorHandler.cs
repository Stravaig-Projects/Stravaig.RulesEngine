using System.Linq.Expressions;

namespace Stravaig.RulesEngine.OperatorHandlers
{
    /// <summary>
    /// Represents the == (Equals) operator.
    /// </summary>
    public class NotEqualsOperatorHandler : OperatorHandler
    {
        private static readonly string[] _operatorNames = { "!=", "NotEquals" };

        /// <inheritdoc />
        public override string[] OperatorNames => _operatorNames;

        /// <inheritdoc />
        public override Expression Handle(Expression left, Expression right)
        {
            return Expression.NotEqual(left, right);
        }
    }
}