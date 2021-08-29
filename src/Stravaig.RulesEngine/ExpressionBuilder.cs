using System;
using System.Linq.Expressions;
using FastExpressionCompiler;

namespace Stravaig.RulesEngine
{
    public class ExpressionBuilder
    {
        public Func<TContext, bool> Build<TContext>(string propertyPath, string expression, string value)
        {
            var paramExpr = Expression.Parameter(typeof(TContext));
            var propertyExpression = Expression.Call(paramExpr, 
                typeof(TContext).GetProperty(propertyPath).GetMethod);
            
            
            var equalExpr = Expression.Equal(propertyExpression, Expression.Constant(value));
            var lambdaExpr = Expression.Lambda<Func<TContext, bool>>(equalExpr, paramExpr);
            var result = lambdaExpr.CompileFast();
            return result;
        }
    }
}