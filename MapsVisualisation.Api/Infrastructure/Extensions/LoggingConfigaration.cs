namespace MapsVisualisation.Api.Infrastructure.Extensions;

public static class LoggingConfigaration
{
    public static void ConfigureLogging(this ILoggingBuilder logging)
    {
        logging.ClearProviders();
        logging.AddConsole();
        logging.AddFilter("Microsoft.EntityFrameworkCore.Database.Command", LogLevel.Warning);
    }
}
