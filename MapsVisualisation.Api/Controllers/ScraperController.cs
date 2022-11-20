using MapsVisualisation.Service.Features.Scrapers.Commands;
using MediatR;

namespace MapsVisualisation.Api.Controllers;

public static class ScraperController
{
    public static void AddScraperControllers(this WebApplication app)
    {
        app.MapGet("/scrap/IgrekAmzp", async (IMediator mediator) =>
            await mediator.Send(new ScrapIgrekAmzpCommand()));

        app.MapGet("/scrap/MapyAmzp", async (IMediator mediator) =>
            await mediator.Send(new ScrapMapyAmzpCommand()));

        app.MapGet("/scrap/AllImages", async (IMediator mediator) =>
            await mediator.Send(new SrapAllImagesCommand()));

        app.MapGet("/scrap/Sbc", async (IMediator mediator) =>
            await mediator.Send(new ScrapSbcCommand()));

        app.MapGet("/scrap/Bcuw", async (IMediator mediator) =>
            await mediator.Send(new ScrapBcuwCommand()));

        app.MapGet("/scrap/Kpbc", async (IMediator mediator) =>
            await mediator.Send(new ScrapKpbcCommand()));
    }
}
