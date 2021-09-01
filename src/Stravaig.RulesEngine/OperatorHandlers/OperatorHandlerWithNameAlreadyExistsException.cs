using System;

namespace Stravaig.RulesEngine.OperatorHandlers
{
    public class OperatorHandlerWithNameAlreadyExistsException : OperatorHandlerServiceLocatorException
    {
        public OperatorHandlerWithNameAlreadyExistsException(string name, Type handlerType, Type existingHandlerType)
            : this(name, handlerType, existingHandlerType, DefaultMessage(name, handlerType, existingHandlerType))
        {
        }

        public OperatorHandlerWithNameAlreadyExistsException(string name, Type handlerType, Type existingHandlerType, string message) 
            : base(message)
        {
            HandlerName = name;
            HandlerType = handlerType;
            ExistingHandlerType = existingHandlerType;
        }

        public OperatorHandlerWithNameAlreadyExistsException(string name, Type handlerType, Type existingHandlerType, string message, Exception inner) 
            : base(message, inner)
        {
            HandlerName = name;
            HandlerType = handlerType;
            ExistingHandlerType = existingHandlerType;
        }
        
        public string HandlerName { get; }
        public Type HandlerType { get; }
        public Type ExistingHandlerType { get; }

        private static string DefaultMessage(string name, Type handlerType, Type existingHandlerType)
        {
            return $"An OperationHandler already exists with the name \"{name}\". Existing handler is {existingHandlerType.FullName}; Current handler is {handlerType.FullName}";
        }
    }
}