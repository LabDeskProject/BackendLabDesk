using Application.Interface.Security;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Authentication
{
    public sealed class PasswordHasher : IPasswordHasher
    {
        public string Hash(string password)
        {
            return BCrypt.Net.BCrypt.HashPassword(password);
        }
    }
}
