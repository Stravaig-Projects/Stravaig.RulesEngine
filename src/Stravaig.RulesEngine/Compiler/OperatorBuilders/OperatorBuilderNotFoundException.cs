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

        private OperatorBuilderNotFoundException(string operatorName, string message) 
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
            return $"An OperationHandler with the name \"{name}\" was not found.";
        }
    }
}