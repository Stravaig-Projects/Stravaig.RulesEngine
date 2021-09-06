using System;
using System.Linq.Expressions;

namespace Stravaig.RulesEngine.Compiler.OperatorBuilders.String
{
    public class StringOrdinalNotEqualsOperatorBuilder : StringNotEqualsOperatorBuilder
    {
        protected override string OperatorName => "OrdinalNotEquals";

        public override Expression Build(Expression leftPropertyExpression, string rightValueAsString)
        {
            return Build(leftPropertyExpression, rightValueAsString, StringComparison.Ordinal);
        }
    }
}