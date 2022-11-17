using MapsVisualisation.Service.Features.Scrapers.Commands;
using MediatR;

namespace MapsVisualisation.Api.Controllers;

public static class ScraperController
{
    public static void AddScraperControllers(this WebApplication app)
    {
        app.MapGet("/scrapIgrekAmzp", async (IMediator mediator) =>
            await mediator.Send(new ScrapIgrekAmzpCommand()));

        app.MapGet("/scrapMapyAmzp", async (IMediator mediator) =>
            await mediator.Send(new ScrapMapyAmzpCommand()));

        app.MapGet("/scrapAllImages", async (IMediator mediator) =>
            await mediator.Send(new SrapAllImagesCommand()));
    }
}
