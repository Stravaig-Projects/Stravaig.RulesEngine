using System;

namespace Stravaig.RulesEngine.Compiler.OperatorBuilders
{
    /// <summary>
    /// Represents an error that occurs when locating the operator.
    /// </summary>
    public abstract class OperatorBuilderServiceLocatorException : Exception
    {
        /// <inheritdoc />
        protected OperatorBuilderServiceLocatorException()
        {
        }

        /// <inheritdoc />
        protected OperatorBuilderServiceLocatorException(string message) : base(message)
        {
        }

        /// <inheritdoc />
        protected OperatorBuilderServiceLocatorException(string message, Exception inner) : base(message, inner)
        {
        }
    }
}