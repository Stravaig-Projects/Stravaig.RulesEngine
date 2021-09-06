using System;
using System.Linq.Expressions;

namespace Stravaig.RulesEngine.Compiler.OperatorBuilders.String
{
    public class StringCurrentCultureNotEqualsOperatorBuilder : StringNotEqualsOperatorBuilder
    {
        protected override string OperatorName => "CurrentCultureNotEquals";

        public override Expression Build(Expression leftPropertyExpression, string rightValueAsString)
        {
            return Build(leftPropertyExpression, rightValueAsString, StringComparison.CurrentCulture);
        }
    }
}