using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Register.Error
{
    public sealed class UserAlreadyExistsException : Exception
    {
        public UserAlreadyExistsException()
            : base("A user with this email already exists.")
        {
        }
    }
}
