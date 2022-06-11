using Microsoft.EntityFrameworkCore;
using Database.Models;

namespace Database.Data;

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

    }

    public DbSet<Region> regions { get; set; }
    public DbSet<Map> maps { get; set; }

}
