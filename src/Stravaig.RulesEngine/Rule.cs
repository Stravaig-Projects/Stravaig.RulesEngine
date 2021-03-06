using System;
using System.Collections.Specialized;
using System.Text;
using Stravaig.RulesEngine.Compiler;
using Stravaig.RulesEngine.Debugging;

namespace Stravaig.RulesEngine
{
    public class Rule : IMatcher, ICompiler
    {
        private readonly object _syncRoot = new object();
        private readonly HybridDictionary _compiledRules = new HybridDictionary();
        public string PropertyPath { get; }
        
        public string Operator { get; }
        
        public string Value { get; }
        
        public Enum[] Modifiers { get; }

        public Rule(string propertyPath, string @operator, string value, params Enum[] modifiers)
        {
            PropertyPath = propertyPath;
            Operator = @operator;
            Value = value;
            Modifiers = modifiers;
        }

        public bool IsMatch<TContext>(TContext context)
        {
            Func<TContext, bool>? matchFunction;
            lock (_syncRoot)
            {
                 matchFunction = _compiledRules[typeof(TContext)] as Func<TContext, bool>;
            }
            if (matchFunction == null)
                throw new InvalidOperationException($"Rule needs to be compiled for the context {typeof(TContext).FullName} first.");
            return matchFunction(context);
        }

        Func<TContext, bool> ICompiler.CompileToFunc<TContext>(ExpressionBuilder expr)
        {
            // ReSharper disable UseNegatedPatternMatching
            // Intent appears clearer without negated pattern matching.
            var contextType = typeof(TContext);
            lock (_syncRoot)
            {
                var result = _compiledRules[contextType] as Func<TContext, bool>;
                if (result == null)
                {
                    result = expr.Build<TContext>(PropertyPath, Operator, Value, Modifiers);
                    _compiledRules[contextType] = result;
                }

                return result;
            }
            // ReSharper restore UseNegatedPatternMatching
        }

        public override string ToString()
        {
            return $"{PropertyPath} {Operator} \"{Value}\"";
        }

        public void DEBUG_BuildRuleDefinition(StringBuilder sb, int indentLevel)
        {
            sb.Indent(indentLevel).Append(this);
        }
    }
}