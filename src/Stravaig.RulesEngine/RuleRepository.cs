using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Stravaig.RulesEngine.Compiler;
using Stravaig.RulesEngine.Compiler.OperatorBuilders;

namespace Stravaig.RulesEngine
{
    public interface IRuleRepository<TKey>
    {
        // ReSharper disable once InconsistentNaming
        string DEBUG_AvailableBuilders { get; }
        TKey[] RuleSetKeys { get; }
        void Load(IEnumerable<KeyValuePair<TKey, RuleSet>> rules);
        void Load(Func<IEnumerable<KeyValuePair<TKey, RuleSet>>> keyValueFactory);
        RuleSet GetRuleSet([DisallowNull] TKey key);
        RulesEngineSession<TKey, TContext> StartSession<TContext>(Func<TKey, bool>? filterPredicate = null);
    }

    public class RuleRepository<TKey> : IRuleRepository<TKey>
    {
        private readonly RulesEngineOptions<TKey> _options;
        private readonly ILogger<RuleRepository<TKey>> _logger;
        private readonly ExpressionBuilder _expressionBuilder;
        private readonly object _syncRoot = new ();
        private readonly Dictionary<TKey, RuleSet> _ruleSets;

        public RuleRepository()
            : this (new RulesEngineOptions<TKey>(), new NullLogger<RuleRepository<TKey>>())
        {
        }

        public RuleRepository(RulesEngineOptions<TKey> options)
            : this(options, new NullLogger<RuleRepository<TKey>>())
        {
        }

        public RuleRepository(RulesEngineOptions<TKey> options, ILogger<RuleRepository<TKey>> logger)
        {
            _options = options;
            _logger = logger;
            _expressionBuilder = new ExpressionBuilder(new OperatorBuilderLocator(options.AdditionalOperatorAssemblies));
            _ruleSets = new Dictionary<TKey, RuleSet>(options.KeyEqualityComparer);
        }
        
        public string DEBUG_AvailableBuilders => _expressionBuilder.DEBUG_AvailableBuilders;
        
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

        public RuleSet GetRuleSet([DisallowNull] TKey key)
        {
            if (key == null) throw new ArgumentNullException(nameof(key));
            lock (_syncRoot)
                return _ruleSets[key];
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
}