using System;
using System.Linq;
using System.Runtime.Serialization;

namespace Stravaig.RulesEngine.Compiler.OperatorBuilders
{
    /// <summary>
    /// Represents an error when unable to locate an <see cref="OperatorBuilder"/>.
    /// </summary>
    public class OperatorBuilderNotFoundException : OperatorBuilderServiceLocatorException
    {
        internal OperatorBuilderNotFoundException(string operatorName)
            : this(operatorName, DefaultMessage(operatorName))
        {
        }

        protected OperatorBuilderNotFoundException(string operatorName, string message) 
            : base(message)
        {
            OperatorName = operatorName;
        }
        
        /// <summary>
        /// The name of the operator that could not be found by the service
        /// locator.
        /// </summary>
        public string OperatorName { get; }
        
        private static string DefaultMessage(string name)
        {
            return $"No OperationBuilder classes supporting the operator named \"{name}\" were found.";
        }
    }

    public class OperatorBuilderForTypeNotFoundException : OperatorBuilderNotFoundException
    {
        internal OperatorBuilderForTypeNotFoundException(string operatorName, Type desiredType, Type?[] availableTypes)
            : base (operatorName, DefaultMessage(operatorName, desiredType, availableTypes))
        {
            DesiredType = desiredType;
            AvailableTypes = availableTypes;
        }

        public Type DesiredType { get; }
        
        public Type?[] AvailableTypes { get; }

        private static string DefaultMessage(string operatorName, Type desiredType, Type?[] availableTypes)
        {
            return $"An OperationBuilder named \"{operatorName}\" for {desiredType.FullName} was not found. The following types are supported: "
                + string.Join(", ", availableTypes.Select(t=>t?.FullName ?? "*"));
        }
    }
}