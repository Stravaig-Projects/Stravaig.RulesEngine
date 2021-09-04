using System;

namespace Stravaig.RulesEngine
{
    public class CompiledExpressionWrapper<TContext> : IMatcher
    {
        public Type ContextType { get; }
        public Func<TContext, bool> IsMatchFunction { get; }

        public CompiledExpressionWrapper(Type contextType, Func<TContext, bool> isMatchFunction)
        {
            ContextType = contextType;
            IsMatchFunction = isMatchFunction;
        }

        public bool IsMatch(TContext context)
        {
            return IsMatchFunction(context);
        }

        bool IMatcher.IsMatch<TContext1>(TContext1 context)
        {
            // This is hackity-hackity-hackland. Luckily this is an intermediate
            // step and expected to be refactored out soon.
            var objectContext = (object?)context;
            var classContext = (TContext)(objectContext ?? throw new InvalidOperationException("Could not cast because it was null."));
            return IsMatch(classContext);

        }
    }
}