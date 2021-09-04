using System;
using System.Collections.Generic;
using System.Linq;
using Stravaig.RulesEngine.Compiler;

namespace Stravaig.RulesEngine
{
    public class RuleGroup : IMatcher, ICompiler
    {
        public BooleanOperator BooleanOperator { get; }
        
        public bool ExpectedResult { get; }
        
        public Rule[] Rules { get; }
        
        public RuleGroup[] RuleGroups { get; }

        public RuleGroup(BooleanOperator @operator, bool expectedResult, params Rule[] rules)
            : this(@operator, expectedResult, rules, Array.Empty<RuleGroup>())
        {
        }

        public RuleGroup(BooleanOperator @operator, bool expectedResult, params RuleGroup[] ruleGroups)
            : this(@operator, expectedResult, Array.Empty<Rule>(), ruleGroups)
        {
        }

        public RuleGroup(BooleanOperator @operator, bool expectedResult, Rule[]? rules = null, RuleGroup[]? ruleGroups = null)
        {
            BooleanOperator = @operator;
            ExpectedResult = expectedResult;
            Rules = rules ?? Array.Empty<Rule>();
            RuleGroups = ruleGroups ?? Array.Empty<RuleGroup>();
        }

        private IEnumerable<IMatcher> Matchers => Rules.Union((IEnumerable<IMatcher>)RuleGroups);

        public bool IsMatch<TContext>(TContext context)
        {
            switch (BooleanOperator)
            {
                case BooleanOperator.And:
                    return Matchers.All(m => m.IsMatch(context)) == ExpectedResult;
                case BooleanOperator.Or:
                    return Matchers.Any(m => m.IsMatch(context)) == ExpectedResult;
            }

            throw new InvalidOperationException($"BooleanOperator has an unexpected value of {BooleanOperator}");
        }

        Func<TContext, bool> ICompiler.CompileToFunc<TContext>(ExpressionBuilder expr)
        {
            Func<TContext, bool> result = CompileExpressionToFunc<TContext>(expr);
            return ctx => result(ctx) == ExpectedResult;
        }

        private Func<TContext, bool> CompileExpressionToFunc<TContext>(ExpressionBuilder expr)
        {
            var booleanFunction = GetBooleanOperationFunction<TContext>();

            Func<TContext, bool>? result = null;
            foreach (var rule in Rules.Cast<ICompiler>())
            {
                var compiledRule = rule.CompileToFunc<TContext>(expr);
                result = result == null 
                    ? compiledRule 
                    : booleanFunction(result, compiledRule);
            }

            foreach (var rule in RuleGroups.Cast<ICompiler>())
            {
                var compiledRule = rule.CompileToFunc<TContext>(expr);
                result = result == null 
                    ? compiledRule 
                    : booleanFunction(result, compiledRule);
            }

            if (result == null)
                throw new InvalidOperationException("This RuleGroup contains no rules or sub groups.");
            return result;
        }

        private Func<Func<TContext, bool>, Func<TContext, bool>, Func<TContext, bool>> GetBooleanOperationFunction<TContext>()
        {
            return BooleanOperator switch
            {
                BooleanOperator.And => CreateAndFunc,
                BooleanOperator.Or => CreateOrFunc,
                _ => throw new InvalidOperationException($"BooleanOperator has an unexpected value of {BooleanOperator}")
            };
        }
        
        private static Func<TContext, bool> CreateAndFunc<TContext>(Func<TContext, bool> resultSoFar, Func<TContext, bool> compiledRule)
        {
            return ctx => resultSoFar(ctx) && compiledRule(ctx);
        }

        private static Func<TContext, bool> CreateOrFunc<TContext>(Func<TContext, bool> resultSoFar, Func<TContext, bool> compiledRule)
        {
            return ctx => resultSoFar(ctx) || compiledRule(ctx);
        }
    }
}