using System;
using System.Linq.Expressions;

namespace Stravaig.RulesEngine.Compiler.OperatorBuilders.String
{
    public class StringOrdinalIgnoreCaseEqualsOperatorBuilder : StringEqualsOperatorBuilder
    {
        protected override string OperatorName => "OrdinalIgnoreCaseEquals";

        public override Expression Build(Expression leftPropertyExpression, string rightValueAsString)
        {
            return Build(leftPropertyExpression, rightValueAsString, StringComparison.OrdinalIgnoreCase);
        }
    }
}