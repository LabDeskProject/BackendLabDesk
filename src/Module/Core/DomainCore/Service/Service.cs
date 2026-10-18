using System;
using System.Collections.Generic;
using System.Text;

namespace DomainCore.Service
{
    public sealed class Service
    {
        public Guid Id { get; private set; }

        public Guid OrganizationId { get; private set; }

        public Guid ResponsibleTeamId { get; private set; }

        public string Name { get; private set; }

        public string? Description { get; private set; }

        public bool IsActive { get; private set; }

        public DateTime CreatedAt { get; private set; }


        private Service()
        {
        }


        public Service(
            Guid id,
            Guid organizationId,
            Guid responsibleTeamId,
            string name,
            string? description,
            DateTime createdAt)
        {
            Id = id;
            OrganizationId = organizationId;
            ResponsibleTeamId = responsibleTeamId;
            Name = name;
            Description = description;
            IsActive = true;
            CreatedAt = createdAt;
        }


        public void Deactivate()
        {
            IsActive = false;
        }


        public void Activate()
        {
            IsActive = true;
        }
    }
}
