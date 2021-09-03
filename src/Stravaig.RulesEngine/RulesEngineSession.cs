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

        public IEnumerable<TKey> FindMatches<TContext>(TContext context)
        {
            foreach (var kvp in _snapshot)
            {
                if (kvp.Value.IsMatch(context))
                    yield return kvp.Key;
            }
        }
    }
}