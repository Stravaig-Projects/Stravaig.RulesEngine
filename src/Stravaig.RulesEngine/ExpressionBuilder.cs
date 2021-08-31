using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq.Expressions;
using System.Reflection;
using FastExpressionCompiler;

namespace Stravaig.RulesEngine
{
    public class ExpressionBuilder
    {
        public Func<TContext, bool> Build<TContext>(
            string propertyPath,
            string expression,
            string value)
        {
            if (propertyPath == null) throw new ArgumentNullException(nameof(propertyPath));
            if (expression == null) throw new ArgumentNullException(nameof(expression));
            if (value == null) throw new ArgumentNullException(nameof(value));

            var paramExpr = Expression.Parameter(typeof(TContext));
            var propertyExpression = BuildPropertyExpression<TContext>(propertyPath, paramExpr);
            
            
            var equalExpr = Expression.Equal(propertyExpression, Expression.Constant(value));
            var lambdaExpr = Expression.Lambda<Func<TContext, bool>>(equalExpr, paramExpr);
            var result = lambdaExpr.CompileFast();
            return result;
        }

        private static Expression BuildPropertyExpression<TContext>(
            string propertyPath,
            ParameterExpression paramExpr)
        {
            var parts = propertyPath.Split(".", StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);

            Expression result = paramExpr;
            var currentContext = typeof(TContext);
            var currentNodePath = string.Empty;
            foreach (var part in parts)
            {
                if (currentNodePath.Length > 0)
                    currentNodePath += ".";
                currentNodePath += part;

                var property = currentContext.GetProperty(part);
                if (property == null)
                {
                    property = currentContext.GetProperty(part, BindingFlags.NonPublic | BindingFlags.Instance);
                    if (property == null)
                        throw new PropertyPathNotFoundException(typeof(TContext), propertyPath, currentNodePath);
                    throw new PublicPropertyGetterRequiredException(typeof(TContext), propertyPath, currentNodePath);
                }
                var getterMethod = property.GetMethod;
                if (getterMethod == null)
                    throw new PropertyGetterRequiredException(typeof(TContext), propertyPath, currentNodePath);
                
                result = Expression.Call(result, getterMethod);
                currentContext = getterMethod.ReturnType;
            }
            return result;
        }
    }
}