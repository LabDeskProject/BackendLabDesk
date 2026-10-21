using System;
using System.Collections.Generic;
using System.Text;

namespace RequestManagement.Application.Interfaces
{
    public interface IRequestSubmissionTransaction
    { 
        Task ExecuteAsync(Func<CancellationToken, Task> action, CancellationToken ct); 
    }
}

