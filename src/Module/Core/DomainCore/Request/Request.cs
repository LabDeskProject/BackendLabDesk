using System;
using System.Collections.Generic;
using System.Text;

namespace DomainCore.Request
{
    public sealed class Request
    {
        public Guid Id { get; private set; }

        public Guid OrganizationId { get; private set; }

        public Guid ServiceId { get; private set; }

        public Guid RequesterUserId { get; private set; }

        public Guid? AssignedTeamId { get; private set; }

        public string Title { get; private set; }

        public string? Description { get; private set; }

        public RequestStatus Status { get; private set; }

        public DateTime CreatedAt { get; private set; }

        public DateTime? UpdatedAt { get; private set; }


        private Request()
        {
        }


        public Request(
            Guid id,
            Guid organizationId,
            Guid serviceId,
            Guid requesterUserId,
            string title,
            string? description,
            DateTime createdAt)
        {
            Id = id;

            OrganizationId = organizationId;

            ServiceId = serviceId;

            RequesterUserId = requesterUserId;

            Title = title;

            Description = description;

            Status = RequestStatus.Submitted;

            CreatedAt = createdAt;
        }


        public void AssignToTeam(
            Guid teamId,
            DateTime updatedAt)
        {
            AssignedTeamId = teamId;

            Status = RequestStatus.Assigned;

            UpdatedAt = updatedAt;
        }
    }
}
