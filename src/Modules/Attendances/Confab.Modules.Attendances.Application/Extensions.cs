using System.Runtime.CompilerServices;
using Microsoft.Extensions.DependencyInjection;

[assembly: InternalsVisibleTo("Confab.Modules.Attendances.Api")]
[assembly: InternalsVisibleTo("Confab.Modules.Attendances.Tests.Integration")]
[assembly: InternalsVisibleTo("Confab.Modules.Attendances.Tests.Unit")]
[assembly: InternalsVisibleTo("DynamicProxyGenAssembly2")]
namespace Confab.Modules.Attendances.Application
{
    internal static class Extensions
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            return services;
        }
    }
}