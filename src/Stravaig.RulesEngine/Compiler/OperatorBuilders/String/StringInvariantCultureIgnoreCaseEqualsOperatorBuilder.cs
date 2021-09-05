using System;
using System.Linq.Expressions;

namespace Stravaig.RulesEngine.Compiler.OperatorBuilders.String
{
    public class StringInvariantCultureIgnoreCaseEqualsOperatorBuilder : StringEqualsOperatorBuilder
    {
        protected override string OperatorName => "InvariantCultureIgnoreCaseEquals";

        public override Expression Build(Expression leftPropertyExpression, string rightValueAsString)
        {
            return Build(leftPropertyExpression, rightValueAsString, StringComparison.InvariantCultureIgnoreCase);
        }
    }
}