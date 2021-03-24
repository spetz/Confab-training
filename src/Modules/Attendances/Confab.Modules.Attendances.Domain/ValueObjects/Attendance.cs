using System;
using Confab.Modules.Attendances.Domain.Types;

namespace Confab.Modules.Attendances.Domain.ValueObjects
{
    public record Attendance(AttendableEventId AttendableEventId, SlotId SlotId, ParticipantId ParticipantId,
        DateTime From, DateTime To);
}