using System;
using Microsoft.Extensions.DependencyInjection;

namespace Stravaig.RulesEngine.DependencyInjection
{
    public static class RulesEngineDependencyInjectionExtensions
    {
        public static IServiceCollection AddStravaigRulesEngine<TKey>(
            this IServiceCollection services)
        {
            return services.AddStravaigRulesEngine<TKey>(null);
        }
        
        public static IServiceCollection AddStravaigRulesEngine<TKey>(
            this IServiceCollection services,
            Action<RulesEngineOptions<TKey>>? configureOptions)
        {
            if (services == null) throw new ArgumentNullException(nameof(services));

            services.AddSingleton<RulesEngineOptions<TKey>>(p =>
            {
                var options = new RulesEngineOptions<TKey>();
                configureOptions?.Invoke(options);
                return options;
            });

            services.AddSingleton<IRuleRepository<TKey>>(
                p => new RuleRepository<TKey>(
                    p.GetRequiredService<RulesEngineOptions<TKey>>()));
            return services;
        }
    }
}