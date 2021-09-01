using System;
using System.Runtime.Serialization;

namespace Stravaig.RulesEngine.OperatorHandlers
{
    public abstract class OperatorHandlerServiceLocatorException : Exception
    {
        public OperatorHandlerServiceLocatorException()
        {
        }

        public OperatorHandlerServiceLocatorException(string message) : base(message)
        {
        }

        public OperatorHandlerServiceLocatorException(string message, Exception inner) : base(message, inner)
        {
        }
    }
}