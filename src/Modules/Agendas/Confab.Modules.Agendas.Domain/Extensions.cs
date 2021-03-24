using System.Runtime.CompilerServices;
using Microsoft.Extensions.DependencyInjection;

[assembly: InternalsVisibleTo("Confab.Modules.Agendas.Api")]
[assembly: InternalsVisibleTo("Confab.Modules.Agendas.Tests.Integration")]
[assembly: InternalsVisibleTo("Confab.Modules.Agendas.Tests.Unit")]
[assembly: InternalsVisibleTo("DynamicProxyGenAssembly2")]
namespace Confab.Modules.Agendas.Domain
{
    public static class Extensions
    {
        public static IServiceCollection AddDomain(this IServiceCollection services)
            => services;
    }
}