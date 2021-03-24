using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Confab.Modules.Attendances.Application.Clients.Agendas;
using Confab.Modules.Attendances.Application.Clients.Agendas.DTO;

namespace Confab.Modules.Attendances.Infrastructure.Clients
{
    internal sealed class AgendasApiClient : IAgendasApiClient
    {
        public Task<RegularAgendaSlotDto> GetRegularAgendaSlotAsync(Guid id)
            => Task.FromResult(new RegularAgendaSlotDto()); // To be implemented...

        public Task<IEnumerable<AgendaTrackDto>> GetAgendaAsync(Guid conferenceId)
            => Task.FromResult(Enumerable.Empty<AgendaTrackDto>()); // To be implemented...
    }
}