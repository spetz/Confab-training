using System;
using Confab.Shared.Abstractions.Exceptions;

namespace Confab.Modules.Agendas.Application.Submissions.Exceptions
{
    public class EmptySubmissionTagsException : ConfabException
    {
        public Guid SubmissionId { get; }
        
        public EmptySubmissionTagsException(Guid agendaItemId) 
            : base($"Submission with ID: '{agendaItemId}' defines empty tags.")
            => SubmissionId = agendaItemId;
    }
}