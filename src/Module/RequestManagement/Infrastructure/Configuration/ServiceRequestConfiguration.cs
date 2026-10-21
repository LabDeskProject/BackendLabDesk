using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RequestManagement.Domain;

namespace RequestManagement.Infrastructure.Configuration;

public sealed class ServiceRequestConfiguration : IEntityTypeConfiguration<ServiceRequest>
{
    public void Configure(EntityTypeBuilder<ServiceRequest> builder)
    {
        builder.ToTable("Requests", "request_management");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.OrganizationId).IsRequired();
        builder.Property(x => x.ServiceId).IsRequired();
        builder.Property(x => x.SubmittedByUserId).IsRequired();
        builder.Property(x => x.RequestNumber).HasMaxLength(64).IsRequired();
        builder.Property(x => x.Title).HasMaxLength(300).IsRequired();
        builder.Property(x => x.Description).HasMaxLength(4000);
        builder.Property(x => x.FormDataJson).HasColumnType("nvarchar(max)").IsRequired();
        builder.Property(x => x.Status).HasConversion<int>().IsRequired();
        builder.Property(x => x.CreatedAtUtc).IsRequired();
        builder.Property(x => x.UpdatedAtUtc).IsRequired();
        builder.Property(x => x.RowVersion).IsRowVersion();
        builder.HasIndex(x => new { x.OrganizationId, x.RequestNumber }).IsUnique();
        builder.HasIndex(x => new { x.OrganizationId, x.CreatedAtUtc });
    }
}
