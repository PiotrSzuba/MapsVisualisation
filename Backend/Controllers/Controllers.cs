namespace Backend.Controllers;

public static class Controllers
{
    public static void AddControllers(this WebApplication app)
    {
        app.AddMapsController();
        app.AddRegionController();
    }
}
