using Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.EntityTypeConfigurations;

public class RecognitionRequestConfiguration : IEntityTypeConfiguration<RecognitionRequest>
{
    public void Configure(EntityTypeBuilder<RecognitionRequest> builder)
    {
        builder.Property(x => x.Status)
            .HasConversion<string>();
    }
}