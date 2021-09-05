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
        private readonly Type[] _builderTypes;
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
        public OperatorBuilderLocator(IEnumerable<Assembly> assemblies) :
            this(GetOperatorBuilderTypesFromAssemblies(assemblies))
        {
        }

        private static Type[] GetOperatorBuilderTypesFromAssemblies(IEnumerable<Assembly> assemblies)
        {
            if (assemblies == null) throw new ArgumentNullException(nameof(assemblies));
            var builderAssemblies = new[] { typeof(OperatorBuilder).Assembly }
                .Union(assemblies)
                .ToArray();

            return builderAssemblies
                .SelectMany(a => a.DefinedTypes)
                .Where(t => !t.IsAbstract)
                .Cast<Type>()
                .ToArray();
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
            _lazyLookup = new Lazy<IReadOnlyDictionary<string, List<OperatorBuilder>>>(BuildDictionary);
        }

        public OperatorBuilderLocator(params Type[] operatorBuilderTypes)
        {
            _builderTypes = operatorBuilderTypes
                .Where(t => t.IsSubclassOf(typeof(OperatorBuilder)))
                .ToArray();

            _lazyLookup = new Lazy<IReadOnlyDictionary<string, List<OperatorBuilder>>>(BuildDictionary);
        }
        
        /// <summary>
        /// Gets the <see cref="OperatorBuilder"/> with the given name.
        /// </summary>
        /// <param name="name">The name of the builder to look up.</param>
        /// <returns>The builder that represents the named operation.</returns>
        /// <exception cref="OperatorBuilderNotFoundException">The named
        /// operator cannot be found.</exception>
        public OperatorBuilder GetBuilder(string name, Type desiredLeftType)
        {
            if (_lazyLookup.Value.TryGetValue(name, out var builderList))
            {
                var result = GetBestOperatorBuilder(builderList, desiredLeftType);
                if (result != null)
                    return result;
            }
            throw new OperatorBuilderNotFoundException(name);
        }

        private static OperatorBuilder? GetBestOperatorBuilder(IReadOnlyList<OperatorBuilder> builderList, Type desiredLeftType)
        {
            return builderList
                .Where(ob => ob.CanMatchType(desiredLeftType))
                .OrderByDescending(ob=>ob.LeftType, OperatorBuilder.CompareWithRespectTo(desiredLeftType))
                .FirstOrDefault();
        }

        private IReadOnlyDictionary<string, List<OperatorBuilder>> BuildDictionary()
        {
            var result = new Dictionary<string, List<OperatorBuilder>>();
            foreach (var builderType in _builderTypes)
            {
                var handler = (OperatorBuilder?)Activator.CreateInstance(builderType);
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