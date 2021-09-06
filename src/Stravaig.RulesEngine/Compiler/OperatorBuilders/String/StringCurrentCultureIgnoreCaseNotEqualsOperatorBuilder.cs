using System;
using System.Linq.Expressions;

namespace Stravaig.RulesEngine.Compiler.OperatorBuilders.String
{
    public class StringCurrentCultureIgnoreCaseNotEqualsOperatorBuilder : StringNotEqualsOperatorBuilder
    {
        protected override string OperatorName => "CurrentCultureIgnoreCaseNotEquals";

        public override Expression Build(Expression leftPropertyExpression, string rightValueAsString)
        {
            return Build(leftPropertyExpression, rightValueAsString, StringComparison.CurrentCultureIgnoreCase);
        }
    }
}