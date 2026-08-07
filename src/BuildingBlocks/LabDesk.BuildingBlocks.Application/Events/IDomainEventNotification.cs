using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace LabDesk.BuildingBlocks.Application.Events
{
    public interface IDomainEventNotification<out TEventType> : IDomainEventNotification
    {
        TEventType DomainEvent { get; }
    }

    public interface IDomainEventNotification : INotification
    {
        Guid Id { get; }
    }
}
