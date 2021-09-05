using System;
using System.Linq.Expressions;

namespace Stravaig.RulesEngine.Compiler.OperatorBuilders.String
{
    public class StringCurrentCultureIgnoreCaseEqualsOperatorBuilder : StringEqualsOperatorBuilder
    {
        protected override string OperatorName => "CurrentCultureIgnoreCaseEquals";

        public override Expression Build(Expression leftPropertyExpression, string rightValueAsString)
        {
            return Build(leftPropertyExpression, rightValueAsString, StringComparison.CurrentCultureIgnoreCase);
        }
    }
}