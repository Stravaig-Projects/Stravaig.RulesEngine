using System;

namespace Stravaig.RulesEngine
{
    /// <summary>
    /// Represents an error that occurs when not able to find the property
    /// getter when building an expression.
    /// </summary>
    public class PropertyGetterRequiredException : ExpressionBuilderException
    {
        /// <summary>
        /// Initialises a new instance of the PropertyGetterRequiredException
        /// class.
        /// </summary>
        /// <param name="contextType">The type of context the expression builder
        /// was working with at the time of the error.</param>
        /// <param name="propertyPath">The property path the expression builder
        /// was working with at the time of the error.</param>
        /// <param name="failingNode">The node of the property path that caused
        /// the expression to fail to build.</param>
        public PropertyGetterRequiredException(Type contextType, string propertyPath, string failingNode)
            : this(contextType, propertyPath, failingNode, DefaultMessage(contextType, propertyPath, failingNode))
        {
        }

        /// <summary>
        /// Initialises a new instance of the PropertyGetterRequiredException
        /// class.
        /// </summary>
        /// <param name="contextType">The type of context the expression builder
        /// was working with at the time of the error.</param>
        /// <param name="propertyPath">The property path the expression builder
        /// was working with at the time of the error.</param>
        /// <param name="failingNode">The node of the property path that caused
        /// the expression to fail to build.</param>
        public PropertyGetterRequiredException(Type contextType, string propertyPath, string failingNode, string message) 
            : base(contextType, propertyPath, message)
        {
            FailingNode = failingNode;
        }

        /// <summary>
        /// Initialises a new instance of the PropertyGetterRequiredException
        /// class.
        /// </summary>
        /// <param name="contextType">The type of context the expression builder
        /// was working with at the time of the error.</param>
        /// <param name="propertyPath">The property path the expression builder
        /// was working with at the time of the error.</param>
        /// <param name="failingNode">The node of the property path that caused
        /// the expression to fail to build.</param>
        public PropertyGetterRequiredException(Type contextType, string propertyPath, string failingNode, string message, Exception inner)
            : base(contextType, propertyPath, message, inner)
        {
            FailingNode = failingNode;
        }
        
        /// <summary>
        /// The node of the property path that caused the expression to fail to
        /// build.
        /// </summary>
        public string FailingNode { get; }

        private static string DefaultMessage(Type contextType, string propertyPath, string failingNode)
        {
            return $"The {failingNode} property requires a getter. The full requested path was [{contextType.FullName}]::{propertyPath}";
        }
    }
}