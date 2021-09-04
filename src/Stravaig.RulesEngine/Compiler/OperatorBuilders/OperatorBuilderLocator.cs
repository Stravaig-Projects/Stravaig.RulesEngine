using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Stravaig.RulesEngine.Compiler.OperatorBuilders
{
    /// <summary>
    /// A locator for <see cref="OperatorBuilder"/> classes.
    /// </summary>
    public class OperatorBuilderLocator
    {
        private readonly Assembly[] _handlerAssemblies;
        private readonly Lazy<IReadOnlyDictionary<string, List<OperatorBuilder>>> _lazyLookup;

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
        /// <exception cref="ArgumentNullException">Thrown if the assemblies
        /// parameter is null.</exception>
        public OperatorBuilderLocator(IEnumerable<Assembly> assemblies)
        {
            if (assemblies == null) throw new ArgumentNullException(nameof(assemblies));
            _handlerAssemblies = new[] { typeof(OperatorBuilder).Assembly }
                .Union(assemblies)
                .ToArray();

            _lazyLookup = new Lazy<IReadOnlyDictionary<string, List<OperatorBuilder>>>(BuildDictionary);
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
        public OperatorBuilder GetBuilder(string name, Type? leftType)
        {
            if (_lazyLookup.Value.TryGetValue(name, out var builderList))
            {
                // TODO: pick the most specific type.
                builderList.FirstOrDefault()
                
                return result;
            }
            throw new OperatorBuilderNotFoundException(name);
        }

        private IReadOnlyDictionary<string, List<OperatorBuilder>> BuildDictionary()
        {
            var result = new Dictionary<string, List<OperatorBuilder>>();
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
                    bool newList = false;
                    if (!result.TryGetValue(name, out var handlerList))
                    {
                        newList = true;                        
                        handlerList = new List<OperatorBuilder>();
                    }
                    
                    handlerList.Add(handler);
                    if (newList)
                        result.Add(name, handlerList);
                }
            }

            return result;
        }
        
    }
}