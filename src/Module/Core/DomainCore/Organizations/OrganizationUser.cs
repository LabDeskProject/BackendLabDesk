using System;
using System.Collections.Generic;
using System.Text;

namespace DomainCore.Organizations
{
    public sealed class OrganizationUser
    {
        public Guid OrganizationId { get; private set; }

        public Guid UserId { get; private set; }

        public OrganizationRole Role { get; private set; }

        public DateTime JoinedAt { get; private set; }


        private OrganizationUser()
        {
        }


        public OrganizationUser(
            Guid organizationId,
            Guid userId,
            OrganizationRole role,
            DateTime joinedAt)
        {
            OrganizationId = organizationId;

            UserId = userId;

            Role = role;

            JoinedAt = joinedAt;
        }
    }
}
