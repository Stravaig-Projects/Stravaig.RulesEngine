using System;
using Stravaig.RulesEngine.Compiler;

namespace Stravaig.RulesEngine
{
    internal interface ICompiler
    {
        Func<TContext, bool> CompileToFunc<TContext>(ExpressionBuilder expr);
    }
}