using System;
using System.Collections.Generic;
using System.Linq;
using Stravaig.RulesEngine.Compiler;

namespace Stravaig.RulesEngine
{
    public class RuleSet : IMatcher, ICompiler
    {
        private readonly object _syncRoot = new ();
        private readonly Dictionary<Type, IMatcher> _compiledMatchers = new ();
        
        public RuleGroup[] RuleGroups { get; }

        public RuleSet(params RuleGroup[] ruleGroups)
        {
            if (ruleGroups == null) throw new ArgumentNullException(nameof(ruleGroups));
            if (ruleGroups.Length == 0) throw new ArgumentException("Value cannot be an empty collection.", nameof(ruleGroups));
            RuleGroups = ruleGroups;
        }

        public RuleSet(IEnumerable<RuleGroup> ruleGroups)
            : this(ruleGroups.ToArray())
        {
        }

        public RuleSet(Rule rule)
            : this(BooleanOperator.And, true, rule)
        {
            
        }
        
        public RuleSet(BooleanOperator @operator, bool expectedResult, params Rule[] rules)
        {
            if (rules == null) throw new ArgumentNullException(nameof(rules));
            if (rules.Length == 0) throw new ArgumentException("Value cannot be an empty collection.", nameof(rules));

            RuleGroups = new[]
            {
                new RuleGroup(@operator, expectedResult, rules)
            };
        }
        
        public bool IsMatch<TContext>(TContext context)
        {
            IMatcher compiledMatcher;
            lock(_syncRoot)
                compiledMatcher = _compiledMatchers[typeof(TContext)];
            
            return compiledMatcher.IsMatch(context);
        }
        
        public bool DEBUG_IsMatch<TContext>(TContext context)
        {
            return RuleGroups.All(rg => rg.IsMatch(context));
        }

        public Func<TContext, bool> CompileToFunc<TContext>(ExpressionBuilder expr)
        {
            lock (_syncRoot)
            {
                if (_compiledMatchers.TryGetValue(typeof(TContext), out var compiledMatcher))
                    return compiledMatcher.IsMatch;

                var func = CompileExpressionToFunc<TContext>(expr);
                var wrapper = new CompiledExpressionWrapper<TContext>(typeof(TContext), func);
                _compiledMatchers.Add(typeof(TContext), wrapper);
                return wrapper.IsMatch;
            }
        }
        
        private Func<TContext, bool> CompileExpressionToFunc<TContext>(ExpressionBuilder expr)
        {
            Func<TContext, bool>? result = null;
            foreach (var rule in RuleGroups.Cast<ICompiler>())
            {
                var compiledRule = rule.CompileToFunc<TContext>(expr);
                result = result == null 
                    ? compiledRule 
                    : CreateAndFunc(result, compiledRule);
            }

            if (result == null)
                throw new InvalidOperationException("This RuleSet contains no rules or sub groups.");
            return result;
        }

        static Func<TContext, bool> CreateAndFunc<TContext>(Func<TContext, bool> resultSoFar, Func<TContext, bool> compiledRule)
        {
            return ctx => resultSoFar(ctx) && compiledRule(ctx);
        }
    }
}