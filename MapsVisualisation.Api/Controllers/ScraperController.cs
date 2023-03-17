using MapsVisualisation.Service.Features.Scrapers.Commands;
using MediatR;

namespace MapsVisualisation.Api.Controllers;

public static class ScraperController
{
    public static void AddScraperControllers(this WebApplication app)
    {
        app.MapGet("/api/scrap/Wig100", async (IMediator mediator) =>
            await mediator.Send(new ScrapWig100Command()));

        app.MapGet("/api/scrap/Wig25", async (IMediator mediator) =>
            await mediator.Send(new ScrapWig25Command()));

        app.MapGet("/api/scrap/Messtischblatt", async (IMediator mediator) =>
            await mediator.Send(new ScrapMesstischblattCommand()));

        app.MapGet("/api/scrap/Amzp", async (IMediator mediator) =>
            await mediator.Send(new ScrapMapyAmzpCommand()));

        app.MapGet("/api/scrap/AusHun75", async (IMediator mediator) =>
            await mediator.Send(new ScrapAusHun75Command()));

        app.MapGet("/api/scrap/AllImages", async (IMediator mediator) =>
            await mediator.Send(new SrapAllImagesCommand()));

        app.MapGet("/api/scrap/Sbc", async (IMediator mediator) =>
            await mediator.Send(new ScrapSbcCommand()));

        app.MapGet("/api/scrap/Bcuw", async (IMediator mediator) =>
            await mediator.Send(new ScrapBcuwCommand()));

        app.MapGet("/api/scrap/Kpbc", async (IMediator mediator) =>
            await mediator.Send(new ScrapKpbcCommand()));
    }
}
