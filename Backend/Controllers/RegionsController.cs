using Database.Data;
using Database.Models;
using Microsoft.EntityFrameworkCore;

namespace Backend.Controllers;

public static class RegionsController
{
    public static void AddRegionController(this WebApplication app)
    {
        app.MapGet("/regions", async (MapsVisualisationContext context) =>
        {
            var data = await context.regions.ToListAsync();

            return data != null ? Results.Ok(data) : Results.NotFound();
        });

        app.MapGet("/regions/{id}", async (int id, MapsVisualisationContext context) =>
        {
            var data = await context.regions.FindAsync(id);

            return data != null ? Results.Ok(data) : Results.NotFound();
        });

        app.MapPost("/regions", async (Region region, MapsVisualisationContext context) =>
        {
            context.regions.Add(region);
            await context.SaveChangesAsync();

            return Results.Created("Region",region);
        });

        app.MapPut("/regions/{id}", async (int id, Region region, MapsVisualisationContext context) =>
        {
            if (id != region.RegionId)
            {
                return Results.BadRequest();
            }
            context.Entry(region).State = EntityState.Modified;

            await context.SaveChangesAsync();

            return Results.NoContent();
        });

        app.MapDelete("/regions/{id}", async (int id, MapsVisualisationContext context) =>
        {
            var data = await context.regions.FindAsync(id);

            if (data == null)
            {
                return Results.NotFound();
            }

            context.regions.Remove(data);

            await context.SaveChangesAsync();

            return Results.NoContent();
        });
    }
}
