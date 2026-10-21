using MediatR;
using Notification.Application.Interfaces;
using Notification.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace Notification.Application.Slices.CreateNotification
{
    public sealed class CreateNotificationHandler(INotificationRepository repo, INotificationUserContext user) : IRequestHandler<CreateNotificationCommand, Guid>
    {
        public async Task<Guid> Handle(CreateNotificationCommand c, CancellationToken ct)
        {
            if (!await repo.IsMemberAsync(c.OrganizationId, user.UserId, ct)) 
                throw new UnauthorizedAccessException("Active organization membership is required to create a notification.");
            if (c.RecipientUserId != user.UserId) throw new UnauthorizedAccessException("Clients may only create notifications for themselves.");
            var found = await repo.FindByIdempotencyKeyAsync(c.OrganizationId, c.IdempotencyKey, ct); 
            if (found is not null) 
                return found.Id;
            var msg = NotificationMessage.Create(c.OrganizationId, c.RecipientUserId, c.Type, c.Title, c.Body, c.IdempotencyKey, c.PayloadJson, c.Channels, DateTime.UtcNow);
            await repo.AddAsync(msg, ct); 
            await repo.SaveChangesAsync(ct); 
            return msg.Id;
        }
    }
}
