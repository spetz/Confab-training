﻿using System.Collections.Generic;
using System.Linq;
using Confab.Modules.Agendas.Domain.Agendas.Exceptions;
using Confab.Modules.Agendas.Domain.Submissions.Consts;
using Confab.Modules.Agendas.Domain.Submissions.Exceptions;
using Confab.Shared.Abstractions.Kernel.Types;

namespace Confab.Modules.Agendas.Domain.Submissions.Entities
{
    public sealed class Submission : AggregateRoot
    {
        public ConferenceId ConferenceId { get; private set; }
        public string Title { get; private set; }
        public string Description { get; private set; }
        public int Level { get; private set; }
        public string Status { get; private set; }
        public IEnumerable<string> Tags { get; private set; }
        public IEnumerable<Speaker> Speakers => _speakers;

        private ICollection<Speaker> _speakers;

        public Submission(AggregateId id, ConferenceId conferenceId, string title, string description, int level,
            string status, IEnumerable<string> tags, ICollection<Speaker> speakers, int version = 0)
            : this(id, conferenceId)
        {
            ConferenceId = conferenceId;
            Title = title;
            Description = description;
            Level = level;
            Status = status;
            Tags = tags;
            _speakers = speakers;
            Version = version;
        }

        public Submission(AggregateId id, ConferenceId conferenceId)
            => (Id, ConferenceId) = (id, conferenceId);

        public static Submission Create(AggregateId id, ConferenceId conferenceId, string title, string description,
            int level, IEnumerable<string> tags, IEnumerable<Speaker> speakers)
        {
            var submission = new Submission(id, conferenceId);
            submission.ChangeTitle(title);
            submission.ChangeDescription(description);
            submission.ChangeLevel(level);
            submission.ChangeTags(tags);
            submission.ChangeSpeakers(speakers);
            submission.ClearEvents();
            submission.Status = SubmissionStatus.Pending;
            submission.Version = 0;

            return submission;
        }

        public void ChangeTitle(string title)
        {
            if (string.IsNullOrWhiteSpace(title))
            {
                throw new EmptySubmissionTitleException(Id);
            }

            Title = title;
            IncrementVersion();
        }

        public void ChangeDescription(string description)
        {
            if (string.IsNullOrWhiteSpace(description))
            {
                throw new EmptySubmissionDescriptionException(Id);
            }

            Description = description;
            IncrementVersion();
        }

        public void ChangeLevel(int level)
        {
            if (IsNotInRange())
            {
                throw new InvalidSubmissionLevelException(Id);
            }

            Level = level;
            IncrementVersion();

            bool IsNotInRange() => level < 1 || level > 6;
        }

        public void ChangeSpeakers(IEnumerable<Speaker> speakers)
        {
            if (speakers is null || !speakers.Any())
            {
                throw new MissingSubmissionSpeakersException(Id);
            }

            _speakers = speakers.ToList();
            IncrementVersion();
        }

        public void ChangeTags(IEnumerable<string> tags)
        {
            if (tags is null || !tags.Any())
            {
                throw new EmptyAgendaItemTagsException(Id);
            }

            Tags = tags;
            IncrementVersion();
        }
    }
}