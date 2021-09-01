using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Stravaig.RulesEngine.OperatorHandlers
{
    public class OperatorHandlerServiceLocator
    {
        private readonly Assembly[] _handlerAssemblies;
        private readonly Lazy<IReadOnlyDictionary<string, OperatorHandler>> _lazyLookup;

        public OperatorHandlerServiceLocator()
            : this(Array.Empty<Assembly>())
        {
        }
        
        public OperatorHandlerServiceLocator(IEnumerable<Assembly> assemblies)
        {
            if (assemblies == null) throw new ArgumentNullException(nameof(assemblies));
            _handlerAssemblies = new[] { typeof(OperatorHandler).Assembly }
                .Union(assemblies)
                .ToArray();

            _lazyLookup = new Lazy<IReadOnlyDictionary<string, OperatorHandler>>(BuildDictionary);
        }

        public OperatorHandlerServiceLocator(params Assembly[] assemblies)
            : this((IEnumerable<Assembly>)assemblies)
        {
        }

        public OperatorHandler GetHandlerFromName(string name)
        {
            if (_lazyLookup.Value.TryGetValue(name, out var result))
                return result;
            throw new OperatorHandlerNotFoundException(name);
        }

        private IReadOnlyDictionary<string, OperatorHandler> BuildDictionary()
        {
            var result = new Dictionary<string, OperatorHandler>();
            var handlerTypes = _handlerAssemblies
                .SelectMany(a => a.DefinedTypes)
                .Where(t => t.IsSubclassOf(typeof(OperatorHandler)));
            foreach (var handlerType in handlerTypes)
            {
                var handler = (OperatorHandler?)Activator.CreateInstance(handlerType);
                if (handler == null)
                    continue;
                foreach (string name in handler.OperatorNames)
                {
                    if (result.TryAdd(name, handler) == false)
                    {
                        if (result.TryGetValue(name, out var existingHandler))
                            throw new OperatorHandlerWithNameAlreadyExistsException(name, handlerType, existingHandler.GetType());
                        
                        // Shouldn't really be able to get to this exception, but just in case...
                        throw new InvalidOperationException($"Failed to add handler {handlerType.FullName} with name \"{name}\".");
                    }
                }
            }

            return result;
        }
        
    }
}