using System;
using System.Collections.Generic;
using System.Linq;

namespace Stravaig.RulesEngine
{
    public class RuleRepository<TKey>
    {
        private readonly object _syncRoot = new object();
        private readonly Dictionary<TKey, RuleSet> _ruleSets = new Dictionary<TKey, RuleSet>();

        public void Load(IEnumerable<KeyValuePair<TKey, RuleSet>> rules)
        {
            if (rules == null) throw new ArgumentNullException(nameof(rules));
            lock (_syncRoot)
            {
                foreach (var kvp in rules)
                {
                    _ruleSets[kvp.Key] = kvp.Value;
                }
            }
        }
        
        public void Load(Func<IEnumerable<KeyValuePair<TKey, RuleSet>>> keyValueFactory)
        {
            if (keyValueFactory == null) throw new ArgumentNullException(nameof(keyValueFactory));
            var keyValuePairs = keyValueFactory();
            Load(keyValuePairs);
        }

        public RulesEngineSession<TKey, TContext> StartSession<TContext>(Func<TKey, bool>? filterPredicate = null)
        {
            Dictionary<TKey, RuleSet> snapshot;
            lock (_syncRoot)
            {
                if(filterPredicate == null)
                    snapshot = new Dictionary<TKey, RuleSet>(_ruleSets);
                else
                {
                    snapshot = new Dictionary<TKey, RuleSet>(
                        _ruleSets
                            .Where(kvp => filterPredicate(kvp.Key)));
                }
            }

            foreach (var ruleSet in snapshot.Values)
            {
                // TODO: Compile the rules
            }

            return new RulesEngineSession<TKey, TContext>(snapshot);
        }
    }

    public class RuleRepository : RuleRepository<object>
    {
    }
}