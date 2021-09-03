using System;
using System.Collections.Generic;
using System.Linq;

namespace Stravaig.RulesEngine
{
    public class RuleSet : IMatcher
    {
        private readonly Dictionary<Type, IMatcher> _compiledMatchers = new Dictionary<Type, IMatcher>();
        
        public RuleGroup[] RuleGroups { get; }
        public bool IsMatch<TContext>(TContext context)
        {
            _compiledMatchers[typeof(TContext)].IsMatch(context);
            
            return RuleGroups.All(rg => rg.IsMatch(context));
        }
    }
}