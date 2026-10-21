using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Notification.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace Notification.Infrastructure.Configuration
{
    public sealed class NotificationMessageConfiguration : IEntityTypeConfiguration<NotificationMessage>
    {
        public void Configure(EntityTypeBuilder<NotificationMessage> builder)
        {
            builder.ToTable("Notifications", "notification");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Type).HasMaxLength(100).IsRequired();
            builder.Property(x => x.Title).HasMaxLength(200).IsRequired();
            builder.Property(x => x.Body).HasMaxLength(4000).IsRequired();
            builder.Property(x => x.IdempotencyKey).HasMaxLength(200).IsRequired();
            builder.Property(x => x.ReadAtUtc);
            builder.Property(x => x.PayloadJson).HasColumnType("nvarchar(max)");
            builder.HasIndex(x => new { x.OrganizationId, x.IdempotencyKey }).IsUnique();
            builder.HasIndex(x => new { x.OrganizationId, x.RecipientUserId, x.CreatedAtUtc });
            builder.HasMany(x => x.Deliveries).WithOne(x => x.Notification).HasForeignKey(x => x.NotificationId).OnDelete(DeleteBehavior.Restrict);
        }
    }
}
