using System.Collections.Generic;

namespace Stravaig.RulesEngine
{
    public class RulesEngineSession<TKey, TContext>
    {
        private readonly IReadOnlyDictionary<TKey, RuleSet> _snapshot;

        public RulesEngineSession(IReadOnlyDictionary<TKey, RuleSet> snapshot)
        {
            _snapshot = snapshot;
        }

        public IEnumerable<TKey> FindMatches(TContext context)
        {
            foreach (var kvp in _snapshot)
            {
                if (kvp.Value.IsMatch(context))
                    yield return kvp.Key;
            }
        }
        
        public IEnumerable<TKey> DEBUG_FindMatches(TContext context)
        {
            foreach (var kvp in _snapshot)
            {
                if (kvp.Value.DEBUG_IsMatch(context))
                    yield return kvp.Key;
            }
        }

        public RuleSet GetRuleSet(TKey key)
        {
            var result = _snapshot[key];
            if (result == null)
                throw new KeyNotFoundException();
            return result;
        }
    }
}