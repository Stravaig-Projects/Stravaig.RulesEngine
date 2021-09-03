using System;
using Stravaig.RulesEngine.Compiler;

namespace Stravaig.RulesEngine
{
    public class Rule : IMatcher, ICompiler
    {
        public string PropertyPath { get; }
        
        public string Operator { get; }
        
        public string Value { get; }

        public Rule(string propertyPath, string @operator, string value)
        {
            PropertyPath = propertyPath;
            Operator = @operator;
            Value = value;
        }

        public bool IsMatch<TContext>(TContext context)
        {
            // TODO: Fix this up properly.
            return true;
        }

        Func<TContext, bool> ICompiler.CompileToFunc<TContext>(ExpressionBuilder expr)
        {
            return expr.Build<TContext>(PropertyPath, Operator, Value);
        }
    }
}