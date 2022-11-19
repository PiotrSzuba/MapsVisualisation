using MapsVisualisation.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MapsVisualisation.Database.Configuration;

public class OtherSourceMapper : IEntityTypeConfiguration<OtherSource>
{
    public void Configure(EntityTypeBuilder<OtherSource> builder)
    {
        builder
            .HasKey(otherSource => otherSource.Id);

        builder
            .Property(otherSource => otherSource.Id)
            .HasDefaultValueSql("NEWID()");

        builder
            .HasOne(otherSource => otherSource.Region)
            .WithMany(region => region.OtherSources);
    }
}
