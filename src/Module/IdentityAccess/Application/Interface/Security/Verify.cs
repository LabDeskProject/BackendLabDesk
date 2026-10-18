using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Interface.Security
{
    public interface Verify 
    {
        bool Verify(
        string password,
        string passwordHash);
    }
}
