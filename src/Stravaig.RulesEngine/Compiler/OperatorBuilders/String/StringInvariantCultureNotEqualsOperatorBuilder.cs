using System;
using System.Linq.Expressions;

namespace Stravaig.RulesEngine.Compiler.OperatorBuilders.String
{
    public class StringInvariantCultureNotEqualsOperatorBuilder : StringNotEqualsOperatorBuilder
    {
        protected override string OperatorName => "InvariantCultureNotEquals";

        public override Expression Build(Expression leftPropertyExpression, string rightValueAsString)
        {
            return Build(leftPropertyExpression, rightValueAsString, StringComparison.InvariantCulture);
        }
    }
}