using BuildingBlocks.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.User
{
    public sealed class User : AggregateRoot
    {
        private User()
        {
        }

        private User(
            string email,
            string fullName,
            string passwordHash)
        {
            Email = email;
            FullName = fullName;
            PasswordHash = passwordHash;
            IsActive = true;
        }

        public string Email { get; private set; } = null!;

        public string FullName { get; private set; } = null!;

        public string PasswordHash { get; private set; } = null!;

        public bool IsActive { get; private set; }

        public static User Create(
            string email,
            string fullName,
            string passwordHash)
        {
            if (string.IsNullOrWhiteSpace(email))
                throw new ArgumentException(
                    "Email is required.",
                    nameof(email));

            if (string.IsNullOrWhiteSpace(fullName))
                throw new ArgumentException(
                    "Full name is required.",
                    nameof(fullName));

            if (string.IsNullOrWhiteSpace(passwordHash))
                throw new ArgumentException(
                    "Password hash is required.",
                    nameof(passwordHash));

            return new User(
                email.Trim().ToLowerInvariant(),
                fullName.Trim(),
                passwordHash);
        }
    }
}
