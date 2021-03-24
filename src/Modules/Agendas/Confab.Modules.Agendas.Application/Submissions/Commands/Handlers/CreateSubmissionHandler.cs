using System.Linq;
using System.Threading.Tasks;
using Confab.Modules.Agendas.Application.CallForPapers.Exceptions;
using Confab.Modules.Agendas.Domain.CallForPapers.Repositories;
using Confab.Modules.Agendas.Domain.Submissions.Entities;
using Confab.Modules.Agendas.Domain.Submissions.Exceptions;
using Confab.Modules.Agendas.Domain.Submissions.Repositories;
using Confab.Shared.Abstractions.Commands;
using Confab.Shared.Abstractions.Kernel.Types;

namespace Confab.Modules.Agendas.Application.Submissions.Commands.Handlers
{
    internal sealed class CreateSubmissionHandler : ICommandHandler<CreateSubmission>
    {
        private readonly ISpeakerRepository _speakerRepository;
        private readonly ISubmissionRepository _submissionRepository;
        private readonly ICallForPapersRepository _callForPapersRepository;

        public CreateSubmissionHandler(ISpeakerRepository speakerRepository, ISubmissionRepository submissionRepository,
            ICallForPapersRepository callForPapersRepository)
        {
            _speakerRepository = speakerRepository;
            _submissionRepository = submissionRepository;
            _callForPapersRepository = callForPapersRepository;
        }

        public async Task HandleAsync(CreateSubmission command)
        {
            var callForPapers = await _callForPapersRepository.GetAsync(command.ConferenceId);
            if (callForPapers is null)
            {
                throw new CallForPapersNotFoundException(command.ConferenceId);
            }

            if (!callForPapers.IsOpened)
            {
                throw new CallForPapersClosedException(command.ConferenceId);
            }
            
            var speakerIds = command.SpeakerIds.Select(id => new AggregateId(id));
            var speakers = await _speakerRepository.BrowseAsync(speakerIds);

            if (speakers.Count() != command.SpeakerIds.Count())
            {
                throw new MissingSubmissionSpeakersException(command.Id);
            }
            
            var submission = Submission.Create(command.Id, command.ConferenceId, command.Title, command.Description, 
                command.Level, command.Tags, speakers);
            
            await _submissionRepository.AddAsync(submission);
        }
    }
}