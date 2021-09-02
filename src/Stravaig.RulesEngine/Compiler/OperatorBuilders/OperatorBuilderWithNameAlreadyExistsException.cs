using System;

namespace Stravaig.RulesEngine.Compiler.OperatorBuilders
{
    /// <summary>
    /// Represents and error that occurs when building the service locator look
    /// up when a second operator with a name used by an existing operator is
    /// added.
    /// </summary>
    public class OperatorBuilderWithNameAlreadyExistsException : OperatorBuilderServiceLocatorException
    {
        /// <summary>
        /// Initialise the exception with details of the error.
        /// </summary>
        /// <param name="name">The duplicated name of the operator</param>
        /// <param name="builderType">The operator builder that was being added
        /// to the look up.</param>
        /// <param name="existingBuilderType">The existing operator builder that
        /// is already in the look up.</param>
        public OperatorBuilderWithNameAlreadyExistsException(string name, Type builderType, Type existingBuilderType)
            : this(name, builderType, existingBuilderType, DefaultMessage(name, builderType, existingBuilderType))
        {
        }

        private OperatorBuilderWithNameAlreadyExistsException(string name, Type builderType, Type existingBuilderType, string message) 
            : base(message)
        {
            OperatorName = name;
            BuilderType = builderType;
            ExistingBuilderType = existingBuilderType;
        }

        /// <summary>
        /// The name of the operator that is duplicated.
        /// </summary>
        public string OperatorName { get; }
        
        /// <summary>
        /// The type of the builder that failed.
        /// </summary>
        public Type BuilderType { get; }
        
        /// <summary>
        /// The type of the builder that was already in the look up.
        /// </summary>
        public Type ExistingBuilderType { get; }

        private static string DefaultMessage(string name, Type builderType, Type existingBuilderType)
        {
            return $"An OperatorBuilder already exists with the name \"{name}\". Existing builder is {existingBuilderType.FullName}; Current builder is {builderType.FullName}";
        }
    }
}