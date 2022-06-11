using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using Database.Data;

namespace Database;

public class MapsVisualisationDbContextFactory : IDesignTimeDbContextFactory<MapsVisualisationContext>
{
    public MapsVisualisationContext CreateDbContext(string[] args)
    {
        IConfigurationRoot configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();
        var builder = new DbContextOptionsBuilder<MapsVisualisationContext>();
        var connectionString = configuration.GetConnectionString("DefaultConnection");
        builder.UseSqlServer(connectionString);

        return new MapsVisualisationContext(builder.Options);
    }
}
