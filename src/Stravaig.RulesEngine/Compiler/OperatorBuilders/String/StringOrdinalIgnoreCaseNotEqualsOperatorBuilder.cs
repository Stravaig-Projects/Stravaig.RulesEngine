using System;
using System.Linq.Expressions;

namespace Stravaig.RulesEngine.Compiler.OperatorBuilders.String
{
    public class StringOrdinalIgnoreCaseNotEqualsOperatorBuilder : StringNotEqualsOperatorBuilder
    {
        protected override string OperatorName => "OrdinalIgnoreCaseNotEquals";

        public override Expression Build(Expression leftPropertyExpression, string rightValueAsString)
        {
            return Build(leftPropertyExpression, rightValueAsString, StringComparison.OrdinalIgnoreCase);
        }
    }
}