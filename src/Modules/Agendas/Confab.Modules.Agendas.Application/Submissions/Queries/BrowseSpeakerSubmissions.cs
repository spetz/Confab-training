using System;
using System.Collections.Generic;
using Confab.Modules.Agendas.Application.Submissions.DTO;
using Confab.Shared.Abstractions.Queries;

namespace Confab.Modules.Agendas.Application.Submissions.Queries
{
    public class BrowseSpeakerSubmissions : IQuery<IEnumerable<SubmissionDto>>
    {
        public Guid SpeakerId { get; set; }
    }
}