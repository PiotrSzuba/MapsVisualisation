using MapsVisualisation.Service.Features.Scrapers.Shared;
using MapsVisualisation.Service.Infrastructure.Mediator;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace MapsVisualisation.Service;

public static class Dependencies
{
    public static void AddServicesDependency(this IServiceCollection services)
    {
        services.AddMediatR(Assembly.GetExecutingAssembly());
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(WorkBehavior<,>));

        services.AddDomainServices();
    }

    private static IServiceCollection AddDomainServices(this IServiceCollection services)
    {
        services.AddScoped<IScrapedInfoHandler, ScrapedInfoHandler>();

        return services;
    }
}
