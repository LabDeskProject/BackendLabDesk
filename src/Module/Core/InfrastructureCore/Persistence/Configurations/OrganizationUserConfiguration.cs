using DomainCore.Organizations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace InfrastructureCore.Persistence.Configurations
{
    public sealed class OrganizationUserConfiguration
    : IEntityTypeConfiguration<OrganizationUser>
    {
        public void Configure(
            EntityTypeBuilder<OrganizationUser> builder)
        {
            builder.ToTable("OrganizationUsers");

            builder.HasKey(
                x => new
                {
                    x.OrganizationId,
                    x.UserId
                });

            builder.Property(x => x.Role)
                .IsRequired();

            builder.Property(x => x.JoinedAt)
                .IsRequired();
        }
    }
}
