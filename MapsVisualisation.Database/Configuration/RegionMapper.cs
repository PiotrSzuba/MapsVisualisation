using MapsVisualisation.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MapsVisualisation.Database.Configuration;

public class RegionMapper : IEntityTypeConfiguration<Region>
{
    public void Configure(EntityTypeBuilder<Region> builder)
    {
        builder
            .HasKey(region => region.Id);

        builder
            .Property(region => region.Id)
            .HasDefaultValueSql("NEWID()");

        builder
            .Property(region => region.RegionType)
            .HasConversion<string>();
    }
}