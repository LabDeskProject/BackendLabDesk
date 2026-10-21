using System;
using System.Collections.Generic;
using System.Text;

namespace RequestManagement.Application.Interfaces
{
    public interface IRequestNumberAllocator 
    { 
        Task<string> NextAsync(Guid organizationId, CancellationToken ct); 
    }
}
