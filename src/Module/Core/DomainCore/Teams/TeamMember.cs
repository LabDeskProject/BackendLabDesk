using System;
using System.Collections.Generic;
using System.Text;

namespace DomainCore.Teams
{
    public sealed class TeamMember
    {
        private TeamMember()
        {
        }

        public Guid TeamId { get; private set; }

        public Guid UserId { get; private set; }

        public DateTime JoinedAt { get; private set; }

        public Team Team { get; private set; } = null!;

        public static TeamMember Create(
            Guid teamId,
            Guid userId)
        {
            return new TeamMember
            {
                TeamId = teamId,
                UserId = userId,
                JoinedAt = DateTime.UtcNow
            };
        }
    }
}
