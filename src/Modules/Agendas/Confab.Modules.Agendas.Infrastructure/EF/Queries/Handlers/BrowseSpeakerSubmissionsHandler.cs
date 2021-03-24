﻿using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Confab.Modules.Agendas.Application.Submissions.DTO;
using Confab.Modules.Agendas.Application.Submissions.Queries;
using Confab.Modules.Agendas.Domain.Submissions.Entities;
using Confab.Modules.Agendas.Infrastructure.EF.Mappings;
using Confab.Shared.Abstractions.Queries;
using Microsoft.EntityFrameworkCore;

namespace Confab.Modules.Agendas.Infrastructure.EF.Queries.Handlers
{
    internal sealed class BrowseSpeakerSubmissionsHandler : IQueryHandler<BrowseSpeakerSubmissions, IEnumerable<SubmissionDto>>
    {
        private readonly DbSet<Submission> _submissions;

        public BrowseSpeakerSubmissionsHandler(AgendasDbContext context)
            => _submissions = context.Submissions;

        public async Task<IEnumerable<SubmissionDto>> HandleAsync(BrowseSpeakerSubmissions query)
            => await _submissions
                .AsNoTracking()
                .Include(s => s.Speakers)
                .Where(s => s.Speakers.Any(ss => ss.Id == query.SpeakerId))
                .Select(s => s.AsDto())
                .ToListAsync();
    }
}