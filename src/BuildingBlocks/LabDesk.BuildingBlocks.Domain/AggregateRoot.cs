using LabDesk.BuildingBlocks.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace LabDesk.BuildingBlocks.Domain
{
    public abstract class AggregateRoot<TKey> : Entity<TKey>, IAggregateRoot, IHasDomainEvents
    {
        // Quản lý Domain Events tại ranh giới Aggregate Root
        private readonly List<IDomainEvent> _domainEvents = new();
        public IReadOnlyCollection<IDomainEvent> DomainEvents => _domainEvents.AsReadOnly();

        protected AggregateRoot(TKey id) : base(id) { }
        protected AggregateRoot() { }

        protected void AddDomainEvent(IDomainEvent domainEvent)
        {
            _domainEvents.Add(domainEvent);
        }

        public void ClearDomainEvents()
        {
            _domainEvents.Clear();
        }

        // Sau này sẽ bổ sung Version / Concurrency Token (Mục 19 trong spec)
    }
}
