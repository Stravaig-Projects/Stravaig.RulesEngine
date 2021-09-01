using System;

namespace Stravaig.RulesEngine.OperatorHandlers
{
    public class OperatorHandlerNotFoundException : Exception
    {
        public OperatorHandlerNotFoundException(string name)
            : this(name, DefaultMessage(name))
        {
        }

        public OperatorHandlerNotFoundException(string name, string message) 
            : base(message)
        {
            Name = name;
        }

        public OperatorHandlerNotFoundException(string name, string message, Exception inner)
            : base(message, inner)
        {
            Name = name;
        }

        public string Name { get; }
        
        private static string DefaultMessage(string name)
        {
            return $"An OperationHandler with the name \"{name}\" was not found.";
        }
    }
}