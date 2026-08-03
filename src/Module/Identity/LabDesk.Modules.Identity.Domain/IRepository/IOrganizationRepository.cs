using LabDesk.Modules.Identity.Domain.Aggregates;
using LabDesk.SeedWork.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace LabDesk.Modules.Identity.Domain.IRepository
{
    public interface IOrganizationRepository : IRepository<Organization>
    {
        Task<Organization?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
        Task<Organization?> GetBySlugAsync(string slug, CancellationToken cancellationToken = default);
        Task<bool> ExistsBySlugAsync(string slug, CancellationToken cancellationToken = default);
        void Add(Organization organization);
        void Update(Organization organization);
    }
}
