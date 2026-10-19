using DomainCore.Organizations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace OrganizationManagement.Infrastructure.Configurations;

public sealed class OrganizationUserConfiguration : IEntityTypeConfiguration<OrganizationUser>
{
    public void Configure(EntityTypeBuilder<OrganizationUser> builder)
    {
        builder.ToTable("OrganizationUsers", "organization_management");
        builder.HasKey(x => new { x.OrganizationId, x.UserId });
        builder.Property(x => x.Role).HasConversion<int>().IsRequired();
        builder.Property(x => x.RemovedAtUtc);
        builder.HasIndex(x => new { x.OrganizationId, x.RemovedAtUtc });
        builder.Property(x => x.JoinedAt).IsRequired();
    }
}
