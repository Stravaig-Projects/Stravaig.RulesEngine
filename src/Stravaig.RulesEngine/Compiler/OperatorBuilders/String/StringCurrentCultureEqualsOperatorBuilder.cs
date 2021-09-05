using System;
using System.Linq.Expressions;

namespace Stravaig.RulesEngine.Compiler.OperatorBuilders.String
{
    public class StringCurrentCultureEqualsOperatorBuilder : StringEqualsOperatorBuilder
    {
        protected override string OperatorName => "CurrentCultureEquals";

        public override Expression Build(Expression leftPropertyExpression, string rightValueAsString)
        {
            return Build(leftPropertyExpression, rightValueAsString, StringComparison.CurrentCulture);
        }
    }
}