using System.Collections.Generic;
using System.Reflection;
using Confab.Shared.Infrastructure.Modules;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Confab.Shared.Infrastructure.Contracts
{
    public static class Extensions
    {
        internal static IServiceCollection AddContracts(this IServiceCollection services,
            IEnumerable<Assembly> assemblies)
        {
            services.AddSingleton<IContractRegistry>(sp =>
            {
                var registry = new ContractRegistry(sp.GetRequiredService<IModuleRegistry>(),
                    sp.GetRequiredService<ILogger<ContractRegistry>>());
                registry.Init(assemblies);

                return registry;
            });

            return services;
        }
        
        internal static IApplicationBuilder ValidateContracts(this IApplicationBuilder app,
            IEnumerable<Assembly> assemblies)
        {
            var contractRegistry = app.ApplicationServices.GetRequiredService<IContractRegistry>();
            contractRegistry.Validate(assemblies);
            
            return app;
        }
        
        public static IContractRegistry UseContracts(this IApplicationBuilder app)
            => app.ApplicationServices.GetRequiredService<IContractRegistry>();
    }
}