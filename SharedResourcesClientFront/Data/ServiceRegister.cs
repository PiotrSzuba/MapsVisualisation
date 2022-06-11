using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using BlazorPanzoom;

namespace SharedResourcesClientFront.Data
{
    public static class ServiceRegister
    {
        public static IServiceCollection RegisterServices(this IServiceCollection services)
        {
            services.AddSingleton<RegionService>();
            services.AddBlazorPanzoomServices();
            return services;
        }
    }
}
