using MediatR;
using MapsVisualisation.Service.Features.Regions.Queries;

namespace MapsVisualisation.Api.Controllers;

public static class RegionsController
{
    public static void AddRegionController(this WebApplication app)
    {
        app.MapGet("/regions", async (IMediator mediator) =>
            await mediator.Send(new AllRegionsQuery()));
    }
}
