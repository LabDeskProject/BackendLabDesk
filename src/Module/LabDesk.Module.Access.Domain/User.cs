using LabDesk.SeedWork.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace LabDesk.Module.Access.Domain
{
    public class User : AggregateRoot<UserId>
    {
        public string Email { get; private set;  }
        public string PasswordHash { get; private set; }
        public bool IsActive { get; private set; }
        public DateTime CreatedAt {  get; private set; }

        private User() { }

        private User(UserId id, string email, string passwordHash)
        {
            Id = id;
            Email = email;
            PasswordHash = passwordHash;
            IsActive = true;
            CreatedAt = DateTime.UtcNow;
        }
        public static User CreateNew(UserId id, string email, string passwordHash)
        {
            return new User(id, email, passwordHash);
        }

    }
}
