using System.Linq.Expressions;

namespace Stravaig.RulesEngine.OperatorHandlers
{
    public class NotEqualsOperatorHandler : OperatorHandler
    {
        public static readonly string[] _operatorNames = { "!=", "NotEquals" };

        /// <inheritdoc />
        public override string[] OperatorNames => _operatorNames;

        /// <inheritdoc />
        public override Expression Handle(Expression left, Expression right)
        {
            return Expression.NotEqual(left, right);
        }
    }
}