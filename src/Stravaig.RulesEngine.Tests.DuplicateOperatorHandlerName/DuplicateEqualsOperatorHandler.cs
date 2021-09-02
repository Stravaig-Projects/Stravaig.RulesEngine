using System;
using System.Linq.Expressions;
using Stravaig.RulesEngine.OperatorHandlers;

namespace Stravaig.RulesEngine.Tests.DuplicateOperatorHandlerName
{
    public class DuplicateEqualsOperatorHandler : OperatorHandler
    {
        protected override string OperatorName => "==";

        public override Expression Handle(Expression left, Expression right)
        {
            throw new NotImplementedException();
        }
    }
}