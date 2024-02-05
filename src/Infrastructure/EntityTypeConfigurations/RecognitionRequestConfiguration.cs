using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.EntityTypeConfigurations;

public class RecognitionRequestConfiguration : IEntityTypeConfiguration<MovieRecognition>
{
    public void Configure(EntityTypeBuilder<MovieRecognition> builder)
    {
        builder.Property(x => x.Status)
            .HasConversion<string>();
    }
}