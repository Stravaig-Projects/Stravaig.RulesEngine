using System;
using System.Linq.Expressions;
using Stravaig.RulesEngine.Compiler.OperatorBuilders;

namespace Stravaig.RulesEngine.Tests.DuplicateOperatorHandlerName
{
    public class DuplicateEqualsOperatorBuilder : OperatorBuilder
    {
        protected override string OperatorName => "==";

        public override Expression Build(Expression left, string right)
        {
            throw new NotImplementedException();
        }
    }
}