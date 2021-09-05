using System;
using System.Linq.Expressions;

namespace Stravaig.RulesEngine.Compiler.OperatorBuilders.String
{
    public class StringInvariantCultureEqualsOperatorBuilder : StringEqualsOperatorBuilder
    {
        protected override string OperatorName => "InvariantCultureEquals";

        public override Expression Build(Expression leftPropertyExpression, string rightValueAsString)
        {
            return Build(leftPropertyExpression, rightValueAsString, StringComparison.InvariantCulture);
        }
    }
}
