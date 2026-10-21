using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RequestManagement.Domain;

namespace RequestManagement.Infrastructure.Configuration;

public sealed class RequestNumberSequenceConfiguration : IEntityTypeConfiguration<RequestNumberSequence>
{
    public void Configure(EntityTypeBuilder<RequestNumberSequence> builder)
    {
        builder.ToTable("RequestNumberSequences", "request_management");
        builder.HasKey(x => x.OrganizationId);
        builder.Property(x => x.NextValue).IsRequired();
        builder.Property(x => x.RowVersion).IsRowVersion();
    }
}
