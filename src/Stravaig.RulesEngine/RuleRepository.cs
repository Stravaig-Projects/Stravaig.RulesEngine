using System;
using System.Collections.Generic;
using System.Linq;
using Stravaig.RulesEngine.Compiler;
using Stravaig.RulesEngine.Compiler.OperatorBuilders;

namespace Stravaig.RulesEngine
{
    public class RuleRepository<TKey>
    {
        private readonly RulesEngineOptions<TKey> _options;
        private readonly ExpressionBuilder _expressionBuilder;
        private readonly object _syncRoot = new object();
        private readonly Dictionary<TKey, RuleSet> _ruleSets;

        public RuleRepository()
            : this (new RulesEngineOptions<TKey>())
        {
        }
        
        public RuleRepository(RulesEngineOptions<TKey> options)
        {
            _options = options;
            _expressionBuilder = new ExpressionBuilder(new OperatorBuilderLocator(options.AdditionalOperatorAssemblies));
            _ruleSets = new Dictionary<TKey, RuleSet>(options.KeyEqualityComparer);
        }
        
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

        public TKey[] RuleSetKeys
        {
            get
            {
                lock(_syncRoot) 
                    return _ruleSets.Keys.ToArray();
            }
        }

        public RulesEngineSession<TKey, TContext> StartSession<TContext>(Func<TKey, bool>? filterPredicate = null)
        {
            Dictionary<TKey, RuleSet> snapshot;
            lock (_syncRoot)
            {
                if(filterPredicate == null)
                    snapshot = new Dictionary<TKey, RuleSet>(_ruleSets, _options.KeyEqualityComparer);
                else
                {
                    snapshot = new Dictionary<TKey, RuleSet>(
                        _ruleSets
                            .Where(kvp => filterPredicate(kvp.Key)),
                        _options.KeyEqualityComparer);
                }
            }

            foreach (var ruleSet in snapshot.Values)
            {
                ruleSet.CompileToFunc<TContext>(_expressionBuilder);
            }

            return new RulesEngineSession<TKey, TContext>(snapshot);
        }
    }

    public class RuleRepository : RuleRepository<object>
    {
    }
}