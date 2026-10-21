using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Notification.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace Notification.Infrastructure.Configuration
{
    public sealed class NotificationDeliveryConfiguration : IEntityTypeConfiguration<NotificationDelivery>  
    {
        public void Configure(EntityTypeBuilder<NotificationDelivery> builder)
        {
            builder.ToTable("Deliveries", "notification");
            builder.HasKey(nd => nd.Id);
            builder.Property(nd => nd.Id).ValueGeneratedOnAdd();
            builder.Property(nd => nd.NotificationId).IsRequired();
            builder.Property(nd => nd.Channel).HasConversion<int>().IsRequired();
            builder.Property(nd => nd.Status).HasConversion<int>().IsRequired();
            builder.Property(nd => nd.AttemptCount).IsRequired();
            builder.Property(nd => nd.NextAttemptAtUtc).IsRequired();
            builder.Property(nd => nd.SentAtUtc).IsRequired(false);
            builder.Property(nd => nd.ClaimedAtUtc).IsRequired(false);
            builder.Property(nd => nd.LastError).HasMaxLength(1000).IsRequired(false);
            builder.HasIndex(nd => new { nd.NotificationId, nd.Channel }).IsUnique();
            builder.HasIndex(nd => new { nd.Status, nd.NextAttemptAtUtc });
        }
    }
}
