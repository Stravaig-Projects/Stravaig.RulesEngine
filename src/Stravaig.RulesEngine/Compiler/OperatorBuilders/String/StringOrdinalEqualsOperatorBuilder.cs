using System;
using System.Linq.Expressions;

namespace Stravaig.RulesEngine.Compiler.OperatorBuilders.String
{
    public class StringOrdinalEqualsOperatorBuilder : StringEqualsOperatorBuilder
    {
        protected override string OperatorName => "OrdinalEquals";

        public override Expression Build(Expression leftPropertyExpression, string rightValueAsString)
        {
            return Build(leftPropertyExpression, rightValueAsString, StringComparison.Ordinal);
        }
    }
}