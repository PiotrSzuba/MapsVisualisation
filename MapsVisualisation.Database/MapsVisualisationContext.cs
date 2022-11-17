using Microsoft.EntityFrameworkCore;
using MapsVisualisation.Database.Configuration;
using MapsVisualisation.Domain.Entities;

namespace MapsVisualisation.Database;

public class MapsVisualisationContext : DbContext
{
    public MapsVisualisationContext()
    {

    }

    public MapsVisualisationContext(DbContextOptions<MapsVisualisationContext> options) : base(options)
    {

    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new RegionMapper());
        modelBuilder.ApplyConfiguration(new MapMapper());
    }

    public DbSet<Region> Regions { get; set; }
    public DbSet<Map> Maps { get; set; }

}
