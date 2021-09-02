using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Stravaig.RulesEngine.OperatorHandlers
{
    /// <summary>
    /// A locator for <see cref="OperatorBuilder"/> classes.
    /// </summary>
    public class OperatorBuilderLocator
    {
        private readonly Assembly[] _handlerAssemblies;
        private readonly Lazy<IReadOnlyDictionary<string, OperatorBuilder>> _lazyLookup;

        /// <summary>
        /// Initialises the locator with the default set of operators.
        /// </summary>
        public OperatorBuilderLocator()
            : this(Array.Empty<Assembly>())
        {
        }
        
        /// <summary>
        /// Initialises the locator with additional operators retrieved from the
        /// given assemblies.
        /// </summary>
        /// <param name="assemblies">A collection of assemblies which contain
        /// additional <see cref="OperatorBuilder"/> classes.</param>
        /// <exception cref="ArgumentNullException">Thrown if the
        /// <see cref="assemblies"/> parameter is null.</exception>
        public OperatorBuilderLocator(IEnumerable<Assembly> assemblies)
        {
            if (assemblies == null) throw new ArgumentNullException(nameof(assemblies));
            _handlerAssemblies = new[] { typeof(OperatorBuilder).Assembly }
                .Union(assemblies)
                .ToArray();

            _lazyLookup = new Lazy<IReadOnlyDictionary<string, OperatorBuilder>>(BuildDictionary);
        }

        /// <summary>
        /// Initialises the locator with additional operators retrieved from the
        /// given assemblies.
        /// </summary>
        /// <param name="assemblies">A collection of assemblies which contain
        /// additional <see cref="OperatorBuilder"/> classes.</param>
        public OperatorBuilderLocator(params Assembly[] assemblies)
            : this((IEnumerable<Assembly>)assemblies)
        {
        }

        /// <summary>
        /// Gets the <see cref="OperatorBuilder"/> with the given name.
        /// </summary>
        /// <param name="name">The name of the builder to look up.</param>
        /// <returns>The builder that represents the named operation.</returns>
        /// <exception cref="OperatorBuilderNotFoundException">The named
        /// operator cannot be found.</exception>
        public OperatorBuilder GetBuilder(string name)
        {
            if (_lazyLookup.Value.TryGetValue(name, out var result))
                return result;
            throw new OperatorBuilderNotFoundException(name);
        }

        private IReadOnlyDictionary<string, OperatorBuilder> BuildDictionary()
        {
            var result = new Dictionary<string, OperatorBuilder>();
            var handlerTypes = _handlerAssemblies
                .SelectMany(a => a.DefinedTypes)
                .Where(t => t.IsSubclassOf(typeof(OperatorBuilder)));
            foreach (var handlerType in handlerTypes)
            {
                var handler = (OperatorBuilder?)Activator.CreateInstance(handlerType);
                if (handler == null)
                    continue;
                foreach (string name in handler.OperatorNames)
                {
                    if (result.TryAdd(name, handler) == false)
                    {
                        if (result.TryGetValue(name, out var existingHandler))
                            throw new OperatorBuilderWithNameAlreadyExistsException(name, handlerType, existingHandler.GetType());
                        
                        // Shouldn't really be able to get to this exception, but just in case...
                        throw new InvalidOperationException($"Failed to add handler {handlerType.FullName} with name \"{name}\".");
                    }
                }
            }

            return result;
        }
        
    }
}