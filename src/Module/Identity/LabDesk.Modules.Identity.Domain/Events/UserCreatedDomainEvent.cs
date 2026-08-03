using System;
using System.Collections.Generic;
using System.Text;

namespace LabDesk.Modules.Identity.Domain.Events
{
    public record UserCreatedDomainEvent(Guid UserId, Guid OrganizationId,
    string Email,
    string Role) : IDomainEvent
    {
        public Guid EventId { get; } = Guid.NewGuid();

        public DateTime OccurredOn { get; } = DateTime.UtcNow;
    }
}
