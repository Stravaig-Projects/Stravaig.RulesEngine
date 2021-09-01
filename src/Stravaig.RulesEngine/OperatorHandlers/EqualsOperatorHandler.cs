using System.Linq.Expressions;

namespace Stravaig.RulesEngine.OperatorHandlers
{
    public class EqualsOperatorHandler : OperatorHandler
    {
        public static readonly string[] _operatorNames = { "==", "Equals" };

        /// <inheritdoc />
        public override string[] OperatorNames => _operatorNames;

        /// <inheritdoc />
        public override Expression Handle(Expression left, Expression right)
        {
            return Expression.Equal(left, right);
        }
    }
}