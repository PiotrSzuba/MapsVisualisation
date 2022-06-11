using Database.Data;
using Database.Models;
using Microsoft.EntityFrameworkCore;

namespace Backend.Controllers;

public static class MapsController
{
    public static void AddMapsController(this WebApplication app)
    {
        app.MapGet("/maps", async (MapsVisualisationContext context) =>
        {
            var data = await context.maps.ToListAsync();

            return data != null ? Results.Ok(data) : Results.NotFound();
        });

        app.MapGet("/maps/{id}", async (int id,MapsVisualisationContext context) =>
        {
            var data = await context.maps.FindAsync(id);

            return data != null ? Results.Ok(data) : Results.NotFound();
        });

        app.MapPost("/maps", async (Map map, MapsVisualisationContext context) =>
        {

            context.maps.Add(map);
            await context.SaveChangesAsync();

            return Results.Created("Map", map);
        });

        app.MapPut("/maps/{id}", async (int id, Map map, MapsVisualisationContext context) =>
        {
            if (id != map.MapId)
            {
                return Results.BadRequest();
            }
            context.Entry(map).State = EntityState.Modified;

            await context.SaveChangesAsync();

            return Results.NoContent();
        });

        app.MapDelete("/maps/{id}", async (int id, MapsVisualisationContext context) =>
        {
            var data = await context.maps.FindAsync(id);

            if (data == null)
            {
                return Results.NotFound();
            }

            context.maps.Remove(data);

            await context.SaveChangesAsync();

            return Results.NoContent();
        });
    }
}
