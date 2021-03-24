using System;
using System.Collections.Generic;
using System.Linq;
using Confab.Modules.Attendances.Domain.Types;
using Confab.Modules.Attendances.Domain.ValueObjects;
using Confab.Shared.Abstractions.Kernel.Types;

namespace Confab.Modules.Attendances.Domain.Entities
{
    public class AttendableEvent : AggregateRoot<AttendableEventId>
    {
        private readonly HashSet<Slot> _slots = new();
        public ConferenceId ConferenceId { get; private set; }
        public DateTime From { get; private set; }
        public DateTime To { get; private set; }
        public IEnumerable<Slot> Slots => _slots;

        private AttendableEvent()
        {
        }

        public AttendableEvent(AttendableEventId id, ConferenceId conferenceId, DateTime from, DateTime to,
            IEnumerable<Slot> slots = null)
        {
            Id = id;
            ConferenceId = conferenceId;
            From = from;
            To = to;
            _slots = new HashSet<Slot>(slots ?? Enumerable.Empty<Slot>());
        }

        public Attendance Attend(Participant participant)
        {
            // To be implemented...
            return null; 
        }

        public void AddSlots(IEnumerable<Slot> slots)
        {
            foreach (var slot in slots)
            {
                _slots.Add(slot);
            }
        }

        public void AddSlots(params Slot[] slots)
        {
            foreach (var slot in slots)
            {
                _slots.Add(slot);
            }
        }
    }
}