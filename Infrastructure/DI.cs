using Domain.Interfaces;
using Infrastructure;
using Infrastructure.Repository;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;


namespace application;

public static class DI
{
    public static IServiceCollection AddInfrastructure (this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
        services.AddScoped<ILineRepository, LineRepository>();
        services.AddScoped<INodeRepository, NodeRepository>();
        services.AddScoped<IFloorRepository, FloorRepository>();
        services.AddScoped<IVenueRepository, VenueRepository>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();

        return services;
    }
}
