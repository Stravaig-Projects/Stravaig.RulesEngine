using System;
using System.Linq.Expressions;
using System.Reflection;
using FastExpressionCompiler;
using Stravaig.RulesEngine.Compiler.OperatorBuilders;

namespace Stravaig.RulesEngine.Compiler
{
    /// <summary>
    /// Provides a means to build and compile expressions representing rules.
    /// </summary>
    public class ExpressionBuilder
    {
        private readonly OperatorBuilderLocator _serviceLocator;

        public ExpressionBuilder()
            : this (new OperatorBuilderLocator())
        {
            
        }

        public ExpressionBuilder(OperatorBuilderLocator serviceLocator)
        {
            _serviceLocator = serviceLocator;
        }
        
        /// <summary>
        /// Builds an expression that represents a rule.
        /// </summary>
        /// <param name="propertyPath">The dotted path to the property to be examined</param>
        /// <param name="operator">The expression used to evaluate the value extracted from the propertyPath and the value parameter.</param>
        /// <param name="value">The value to be evaluated.</param>
        /// <typeparam name="TContext">The type used at the root of the propertyPath</typeparam>
        /// <returns>A function that can be used to evaluate the rule.</returns>
        /// <exception cref="ArgumentNullException">An argument to the method was null</exception>
        /// <exception cref="PropertyPathNotFoundException">The path to the property could not be found.</exception>
        /// <exception cref="PublicPropertyGetterRequiredException">The path to the property could not be evaluated because a getter was inaccessible.</exception>
        /// <exception cref="PropertyGetterRequiredException">The path to the property could not be evaluated because a property was set only.</exception>
        public Func<TContext, bool> Build<TContext>(
            string propertyPath,
            string @operator,
            string value)
        {
            if (propertyPath == null) throw new ArgumentNullException(nameof(propertyPath));
            if (@operator == null) throw new ArgumentNullException(nameof(@operator));
            if (value == null) throw new ArgumentNullException(nameof(value));

            var paramExpr = Expression.Parameter(typeof(TContext), "context");
            var (propertyExpression, propertyType) = BuildPropertyExpression<TContext>(propertyPath, paramExpr);

            object convertedValue = Convert.ChangeType(value, propertyType);
            var valueExpression = Expression.Constant(convertedValue);

            var handler = _serviceLocator.GetBuilder(@operator);
            var evaluationExpression = handler.Build(propertyExpression, valueExpression);
            var lambdaExpr = Expression.Lambda<Func<TContext, bool>>(evaluationExpression, paramExpr);

            var result = lambdaExpr.CompileFast();
            return result;
        }

        private static (Expression, Type) BuildPropertyExpression<TContext>(
            string propertyPath,
            ParameterExpression paramExpr)
        {
            var parts = propertyPath.Split(".", StringSplitOptions.RemoveEmptyEntries);

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
            return (result, currentContext);
        }
    }
}