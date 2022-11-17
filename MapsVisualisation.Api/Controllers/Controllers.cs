namespace MapsVisualisation.Api.Controllers;

public static class Controllers
{
    public static void AddControllers(this WebApplication app)
    {
        app.AddRegionController();
        app.AddScraperControllers();
    }
}
