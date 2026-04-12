using application.Service;
using application.Service.Interfaces;
using Application.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;


namespace application;

public static class DI
{
    public static IServiceCollection AddApplication(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<ILineService, LineService>();
        services.AddScoped<INodeService, NodeService>();
        services.AddScoped<IFloorService, FloorService>();
        services.AddScoped<IVenueService, VenueService>();

        return services;
    }
}
