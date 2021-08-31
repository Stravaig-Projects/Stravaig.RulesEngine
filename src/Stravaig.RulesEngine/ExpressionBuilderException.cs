using System;

namespace Stravaig.RulesEngine
{
    /// <summary>
    /// Represents an error that occurs when building the expression.
    /// </summary>
    public abstract class ExpressionBuilderException : Exception
    {
        /// <summary>
        /// Initialises a new instance of the ExpressionBuilderException class
        /// </summary>
        /// <param name="contextType">The type of context the expression builder
        /// was working with at the time of the error.</param>
        /// <param name="propertyPath">The property path the expression builder
        /// was working with at the time of the error.</param>
        protected ExpressionBuilderException(Type contextType, string propertyPath)
        {
            PropertyPath = propertyPath;
            ContextType = contextType;
        }

        /// <summary>
        /// Initialises a new instance of the ExpressionBuilderException class
        /// with a specified message.
        /// </summary>
        /// <param name="contextType">The type of context the expression builder
        /// was working with at the time of the error.</param>
        /// <param name="propertyPath">The property path the expression builder
        /// was working with at the time of the error.</param>
        /// <param name="message">The message that describes the error.</param>
        protected ExpressionBuilderException(Type contextType, string propertyPath, string message)
            : base(message)
        {
            PropertyPath = propertyPath;
            ContextType = contextType;
        }

        /// <summary>
        /// Initialises a new instance of the ExpressionBuilderException class
        /// with a specified message and a reference to the inner exception that
        /// is the cause of this exception.
        /// </summary>
        /// <param name="contextType">The type of context the expression builder
        /// was working with at the time of the error.</param>
        /// <param name="propertyPath">The property path the expression builder
        /// was working with at the time of the error.</param>
        /// <param name="message">The message that describes the error.</param>
        /// <param name="inner">The exception that is the cause of the current
        /// exception.</param>
        protected ExpressionBuilderException(Type contextType, string propertyPath, string message, Exception inner)
            : base(message, inner)
        {
            PropertyPath = propertyPath;
            ContextType = contextType;
        }
        
        /// <summary>
        /// The property path the expression builder was working with at the
        /// time of the error.
        /// </summary>
        public string PropertyPath { get; }
        
        /// <summary>
        /// The type of context the expression builder was working with at the
        /// time of the error.
        /// </summary>
        public Type ContextType { get; }
    }
}