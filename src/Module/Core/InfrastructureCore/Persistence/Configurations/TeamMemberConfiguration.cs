using DomainCore.Teams;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace InfrastructureCore.Persistence.Configurations
{
    public sealed class TeamMemberConfiguration
    : IEntityTypeConfiguration<TeamMember>
    {
        public void Configure(
            EntityTypeBuilder<TeamMember> builder)
        {
            builder.ToTable("TeamMembers");

            builder.HasKey(x => new
            {
                x.TeamId,
                x.UserId
            });

            builder.Property(x => x.JoinedAt)
                .IsRequired();

            builder.HasOne(x => x.Team)
                .WithMany(x => x.Members)
                .HasForeignKey(x => x.TeamId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
