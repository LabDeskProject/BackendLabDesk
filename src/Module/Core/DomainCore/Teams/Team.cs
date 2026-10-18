using DomainCore.Organizations;
using System;
using System.Collections.Generic;
using System.Text;

namespace DomainCore.Teams
{
    public sealed class Team
    {
        public Guid Id { get; private set; }

        public Guid OrganizationId { get; private set; }

        public string Name { get; private set; }

        public string? Description { get; private set; }

        public DateTime CreatedAt { get; private set; }
        public Organization Organization { get; private set; } = null!;

        public ICollection<TeamMember> Members { get; private set; } = new List<TeamMember>();


        private Team()
        {
        }


        public Team(
            Guid id,
            Guid organizationId,
            string name,
            string? description,
            DateTime createdAt)
        {
            Id = id;
            OrganizationId = organizationId;
            Name = name;
            Description = description;
            CreatedAt = createdAt;
        }
        public static Team Create(
           Guid organizationId,
           string name,
           string? description)
        {
            return new Team(
                Guid.NewGuid(),
                organizationId,
                name,
                description,
                DateTime.UtcNow);
        }
    }
}
