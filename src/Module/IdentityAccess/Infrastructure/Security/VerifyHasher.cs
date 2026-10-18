using Application.Interface.Security;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Security
{
    public class VerifyHasher : Verify
    {
        public bool Verify(
        string password,
        string passwordHash)
        {
            return BCrypt.Net.BCrypt.Verify(
                password,
                passwordHash);
        }
    }
}
