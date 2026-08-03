using LabDesk.Modules.Identity.Domain.Events;
using LabDesk.SeedWork.Domain;
using Microsoft.AspNetCore.Http.HttpResults;
using System;
using System.Collections.Generic;
using System.Text;

namespace LabDesk.Modules.Identity.Domain.Aggregates
{
    public class User : AggregateRoot<Guid>
    {
        public Guid OrganizationId { get; private set; }
        public string Email { get; private set; } = default!;
        public string FullName { get; private set; } = default!;
        public Role Role { get; private set; }
        public Guid? TeamId { get; private set; }
        public bool IsActive { get; private set; }

        private User() { } // EF Core

        public User(
            Guid id,
            Guid organizationId,
            string email,
            string fullName,
            Role role,
            Guid? teamId = null
            ) : base(id)
        {
            if (organizationId == Guid.Empty)
                throw new ArgumentException("OrganizationId không hợp lệ.", nameof(organizationId));

            if (string.IsNullOrWhiteSpace(email) || !email.Contains('@'))
                throw new ArgumentException("Email không đúng định dạng.", nameof(email));

            if (string.IsNullOrWhiteSpace(fullName))
                throw new ArgumentException("FullName không được để trống.", nameof(fullName));

            OrganizationId = organizationId;
            Email = email.ToLowerInvariant().Trim();
            FullName = fullName.Trim();
            Role = role;
            TeamId = teamId;
            IsActive = true;
            CreatedAt = DateTime.UtcNow;

            AddDomainEvent(new UserCreatedDomainEvent(Id, OrganizationId, Email, Role.ToString()));
        }

        public void AssignToTeam(Guid teamId)
        {
            if (teamId == Guid.Empty)
                throw new ArgumentException("TeamId không hợp lệ.", nameof(teamId));

            TeamId = teamId;
            UpdatedAt = DateTimeOffset.UtcNow;
        }

        public void ChangeRole(Role newRole)
        {
            Role = newRole;
            UpdatedAt = DateTimeOffset.UtcNow;
        }

        public void UpdateProfile(string fullName)
        {
            if (string.IsNullOrWhiteSpace(fullName))
                throw new ArgumentException("FullName không được để trống.", nameof(fullName));

            FullName = fullName.Trim();
            UpdatedAt = DateTimeOffset.UtcNow;
        }

        public void Deactivate()
        {
            IsActive = false;
            UpdatedAt = DateTimeOffset.UtcNow;
        }
    }
}
