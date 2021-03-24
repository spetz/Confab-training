using System.Runtime.CompilerServices;
using Confab.Modules.Agendas.Domain.Agendas.Repositories;
using Confab.Modules.Agendas.Domain.CallForPapers.Repositories;
using Confab.Modules.Agendas.Domain.Submissions.Repositories;
using Confab.Modules.Agendas.Infrastructure.EF;
using Confab.Modules.Agendas.Infrastructure.EF.Repositories;
using Confab.Shared.Infrastructure.Postgres;
using Microsoft.Extensions.DependencyInjection;

[assembly: InternalsVisibleTo("Confab.Modules.Agendas.Api")]
[assembly: InternalsVisibleTo("Confab.Modules.Agendas.Tests.Integration")]
[assembly: InternalsVisibleTo("Confab.Modules.Agendas.Tests.Unit")]
[assembly: InternalsVisibleTo("DynamicProxyGenAssembly2")]
namespace Confab.Modules.Agendas.Infrastructure
{
    public static class Extensions
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services)
            => services
                .AddPostgres<AgendasDbContext>()
                .AddScoped<ISpeakerRepository, SpeakerRepository>()
                .AddScoped<ISubmissionRepository, SubmissionRepository>()
                .AddScoped<IAgendaItemsRepository, AgendaItemsRepository>()
                .AddScoped<ICallForPapersRepository, CallForPapersRepository>()
                .AddScoped<IAgendaTracksRepository, AgendaTracksRepository>();
    }
}