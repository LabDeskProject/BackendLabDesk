using MediatR;
using Notification.Domain.Enum;
using System;
using System.Collections.Generic;
using System.Text;

namespace Notification.Application.Slices.CreateNotification
{
    public sealed record CreateNotificationCommand(Guid OrganizationId, Guid RecipientUserId, string Type, string Title, string Body, string IdempotencyKey, string? PayloadJson, IReadOnlyList<DeliveryChannel> Channels) : IRequest<Guid>;

}
