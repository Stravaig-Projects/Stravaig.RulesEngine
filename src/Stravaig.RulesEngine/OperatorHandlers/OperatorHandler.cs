using System;
using System.Linq.Expressions;

namespace Stravaig.RulesEngine
{
    public abstract class OperatorHandler
    {
        protected virtual string OperatorName => string.Empty;
        public virtual string[] OperatorNames => new[] { OperatorName };
        public abstract Expression Handle(Expression left, Expression right);
    }
}