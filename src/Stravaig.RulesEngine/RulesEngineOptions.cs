using System;
using System.Collections.Generic;
using System.Reflection;

namespace Stravaig.RulesEngine
{
    public class RulesEngineOptions<TKey>
    {
        public IEnumerable<Assembly> AdditionalOperatorAssemblies { get; set; } = Array.Empty<Assembly>();
        
        public IEqualityComparer<TKey> KeyEqualityComparer { get; set; } = EqualityComparer<TKey>.Default;
    }
}