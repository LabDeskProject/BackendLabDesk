using LabDesk.Modules.Identity.Application.Interfaces;
using LabDesk.Modules.Identity.Domain.Aggregates;
using LabDesk.SeedWork.Domain;
using LabDesk.SeedWork.Domain.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Security.Principal;
using System.Text;

namespace LabDesk.Modules.Identity.Infrastructure.Persistence
{
    public class IdentityDbContext : DbContext,IIdentityDbContext, IUnitOfWork
    {
        private readonly IPublisher _publisher;

        public DbSet<Organization> Organizations => Set<Organization>();
        public DbSet<Team> Teams => Set<Team>();
        public DbSet<User> Users => Set<User>();

        public IdentityDbContext(DbContextOptions<IdentityDbContext> options, IPublisher publisher)
        : base(options)
        {
            _publisher = publisher;
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(IdentityDbContext).Assembly);
        }

        //Important
        private async Task DispatchDomainEventsAsync()
        {
            // Bước 1: Dùng ChangeTracker của EF Core để tìm tất cả các Entity đang nằm trong bộ nhớ
            var domainEntities = ChangeTracker
                .Entries<AggregateRoot<System.Guid>>()
                .Where(x => x.Entity.DomainEvents != null && x.Entity.DomainEvents.Any())
                .ToList();

            // Bước 2: Gom tất cả các Domain Events của các Entity đó lại thành 1 danh sách duy nhất
            var domainEvents = domainEntities
                .SelectMany(x => x.Entity.DomainEvents)
                .ToList();

            // Bước 3: XÓA SẠCH danh sách sự kiện trong Entity
            domainEntities.ForEach(entity => entity.Entity.ClearDomainEvents());

            // Bước 4: Dùng MediatR bắn từng sự kiện đi
            foreach (var domainEvent in domainEvents)
            {
                await _publisher.Publish(domainEvent);
            }
        }

        public async Task<bool> SaveEntitiesAsync(CancellationToken cancellationToken = default)
        {
            // 1. Dispatch các Domain Events trước khi SaveChanges
            await DispatchDomainEventsAsync();

            // 2. Lưu thay đổi vào DB
            await base.SaveChangesAsync(cancellationToken);
            return true;
        }
    }


}
