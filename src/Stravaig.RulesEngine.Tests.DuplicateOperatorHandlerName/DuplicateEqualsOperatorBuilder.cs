using System;
using System.Linq.Expressions;
using Stravaig.RulesEngine.OperatorHandlers;

namespace Stravaig.RulesEngine.Tests.DuplicateOperatorHandlerName
{
    public class DuplicateEqualsOperatorBuilder : OperatorBuilder
    {
        protected override string OperatorName => "==";

        public override Expression Build(Expression left, Expression right)
        {
            throw new NotImplementedException();
        }
    }
}