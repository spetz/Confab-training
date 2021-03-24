using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Confab.Modules.Tickets.Core.DTO;
using Confab.Modules.Tickets.Core.Entities;
using Confab.Modules.Tickets.Core.Repositories;
using Confab.Shared.Abstractions.Time;
using Microsoft.Extensions.Logging;

namespace Confab.Modules.Tickets.Core.Services
{
    internal class TicketSaleService : ITicketSaleService
    {
        private readonly IConferenceRepository _conferenceRepository;
        private readonly ITicketSaleRepository _ticketSaleRepository;
        private readonly ITicketRepository _ticketRepository;
        private readonly ITicketGenerator _ticketGenerator;
        private readonly IClock _clock;
        private readonly ILogger<TicketSaleService> _logger;

        public TicketSaleService(IConferenceRepository conferenceRepository, ITicketSaleRepository ticketSaleRepository,
            ITicketRepository ticketRepository, ITicketGenerator ticketGenerator, IClock clock,
            ILogger<TicketSaleService> logger)
        {
            _conferenceRepository = conferenceRepository;
            _ticketSaleRepository = ticketSaleRepository;
            _ticketRepository = ticketRepository;
            _ticketGenerator = ticketGenerator;
            _clock = clock;
            _logger = logger;
        }

        public async Task AddAsync(TicketSaleDto dto)
        {
            // To be implemented...
            await Task.CompletedTask;
        }

        public async Task<IEnumerable<TicketSaleInfoDto>> GetAllAsync(Guid conferenceId)
        {
            var conference = await _conferenceRepository.GetAsync(conferenceId);
            if (conference is null)
            {
                return null;
            }

            var ticketSales = await _ticketSaleRepository.BrowseForConferenceAsync(conferenceId);

            return ticketSales.Select(x => Map(x, conference));
        }

        public async Task<TicketSaleInfoDto> GetCurrentAsync(Guid conferenceId)
        {
            var conference = await _conferenceRepository.GetAsync(conferenceId);
            if (conference is null)
            {
                return null;
            }

            var now = _clock.CurrentDate();
            var ticketSale = await _ticketSaleRepository.GetCurrentForConferenceAsync(conferenceId, now);
            
            return ticketSale is not null ? Map(ticketSale, conference) : null;
        }

        private static TicketSaleInfoDto Map(TicketSale ticketSale, Conference conference)
        {
            int? availableTickets = null;
            var totalTickets = ticketSale.Amount;
            if (totalTickets.HasValue)
            {
                availableTickets = ticketSale.Tickets.Count(x => x.UserId is null);
            }

            return new TicketSaleInfoDto( ticketSale.Name, new ConferenceDto(conference.Id, conference.Name), ticketSale.Price,
                totalTickets, availableTickets, ticketSale.From, ticketSale.To);
        }
    }
}