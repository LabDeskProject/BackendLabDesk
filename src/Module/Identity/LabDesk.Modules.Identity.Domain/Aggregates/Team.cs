using LabDesk.SeedWork.Domain;
using Microsoft.AspNetCore.Http.HttpResults;
using System;
using System.Collections.Generic;
using System.Text;

namespace LabDesk.Modules.Identity.Domain.Aggregates
{
    public class Team : AggregateRoot<Guid>
    {
        public Guid OrganizationId { get; private set; }
        public string Name { get; private set; } = default!;
        public string? Description { get; private set; }
        public Guid? TeamLeadId { get; private set; }

        private Team() { } //EF Core

        public Team(Guid id, Guid organizationId, string name, string? description = null, Guid? teamLeadId = null) : base(id)
        {
            if (organizationId == Guid.Empty)
                throw new ArgumentException("OrganizationId không hợp lệ.", nameof(organizationId));

            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Name không được để trống.", nameof(name));

            OrganizationId = organizationId;
            Name = name.Trim();
            Description = description?.Trim();
            TeamLeadId = teamLeadId;
            CreatedAt = DateTimeOffset.UtcNow;
        }

        public void AssignTeamLead(Guid teamLeadId)
        {
            if (teamLeadId == Guid.Empty)
                throw new ArgumentException("TeamLeadId không hợp lệ.", nameof(teamLeadId));

            TeamLeadId = teamLeadId;
            UpdatedAt = DateTimeOffset.UtcNow;
        }

        public void UpdateDetails(string name, string? description)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Name không được để trống.", nameof(name));

            Name = name.Trim();
            Description = description?.Trim();
            UpdatedAt = DateTimeOffset.UtcNow;
        }
    }
}
