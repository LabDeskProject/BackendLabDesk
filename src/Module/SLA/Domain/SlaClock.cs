using SLA.Domain.Enum;
using System;
using System.Collections.Generic;
using System.Text;

namespace SLA.Domain
{
    public sealed class SlaClock
    {
        private SlaClock() { }
        public Guid Id { get; private set; }
        public Guid OrganizationId { get; private set; }
        public Guid RequestId { get; private set; }
        public Guid ServiceId { get; private set; }
        public Guid PolicyId { get; private set; }
        public DateTime StartedAtUtc { get; private set; }
        public DateTime ResponseDueAtUtc { get; private set; }
        public DateTime ResolutionDueAtUtc { get; private set; }
        public DateTime? FirstResponseAtUtc { get; private set; }
        public DateTime? ResolvedAtUtc { get; private set; }
        public SlaClockStatus Status { get; private set; }
        public static SlaClock Start(Guid organizationId, Guid requestId, Guid serviceId, SlaPolicy policy, DateTime startedAtUtc)
        {
            if (policy.IsArchived || policy.OrganizationId != organizationId || policy.ServiceId != serviceId) 
                throw new InvalidOperationException("Active SLA policy for this organization and service is required.");
            return new SlaClock { Id = Guid.NewGuid(), OrganizationId = organizationId, RequestId = requestId, ServiceId = serviceId, PolicyId = policy.Id, StartedAtUtc = startedAtUtc, ResponseDueAtUtc = startedAtUtc.AddMinutes(policy.ResponseMinutes), ResolutionDueAtUtc = startedAtUtc.AddMinutes(policy.ResolutionMinutes), Status = SlaClockStatus.Running };
        }
        public void RecordResponse(DateTime respondedAtUtc)
        {
            if (FirstResponseAtUtc is not null || Status is SlaClockStatus.Resolved or SlaClockStatus.Cancelled) 
                throw new InvalidOperationException("Response cannot be recorded for this SLA clock.");
            FirstResponseAtUtc = respondedAtUtc;
            if (respondedAtUtc > ResponseDueAtUtc) Status = SlaClockStatus.ResponseBreached;
        }
        public void Resolve(DateTime resolvedAtUtc)
        {
            if (Status is SlaClockStatus.Resolved or SlaClockStatus.Cancelled) 
                throw new InvalidOperationException("SLA clock is already terminal.");
            ResolvedAtUtc = resolvedAtUtc;
            Status = resolvedAtUtc > ResolutionDueAtUtc ? SlaClockStatus.ResolutionBreached : SlaClockStatus.Resolved;
        }
        public void Cancel() { if (Status is SlaClockStatus.Resolved or SlaClockStatus.Cancelled) 
                throw new InvalidOperationException("SLA clock is already terminal."); Status = SlaClockStatus.Cancelled; }
    }

}
