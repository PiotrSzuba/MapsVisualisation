using MapsVisualisation.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MapsVisualisation.Database.Configuration;

public class MapMapper : IEntityTypeConfiguration<Map>
{
    public void Configure(EntityTypeBuilder<Map> builder)
    {
        builder
            .HasKey(map => map.Id);

        builder
            .Property(map => map.Id)
            .HasDefaultValueSql("NEWID()");

        builder
            .HasOne(map => map.Region)
            .WithMany(region => region.Maps);
    }
}
