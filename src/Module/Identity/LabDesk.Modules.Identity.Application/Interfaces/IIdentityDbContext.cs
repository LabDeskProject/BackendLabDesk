using LabDesk.Modules.Identity.Domain.Aggregates;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace LabDesk.Modules.Identity.Application.Interfaces
{
    public interface IIdentityDbContext
    {
        DbSet<Organization> Organizations { get; }
        DbSet<Team> Teams { get; }
        DbSet<User> Users { get; }
    }
}
