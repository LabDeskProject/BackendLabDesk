using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Interface.Security
{
    public interface IPasswordHasher
    {
        string Hash(string password);
    }
}
