using HostwayParking.Domain.Interface;
using HostwayParking.Infrastructure.DataAcess.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace HostwayParking.Infrastructure.DataAcess.Config
{
    public static class Injection
    {
        public static IServiceCollection AddInfraestructure(this IServiceCollection service, IConfiguration configuration)
        {
            service.AddDbContext<HostwaayParkingDbContext>(options =>
                options.UseSqlite(configuration.GetConnectionString("DefaultConnection"),
                b => b.MigrationsAssembly(typeof(HostwaayParkingDbContext).Assembly.FullName)));

            AddInterfaces(service);

            return service;
        }

        private static void AddInterfaces(IServiceCollection service)
        {
            service.AddScoped<IUnitOfWork, UnitOfWork>();
            service.AddScoped<IParkingRepository, ParkingRepository>();
            service.AddScoped<IVehiclesRepository, VehicleRepository>();
            service.AddScoped<ISessionParkingRepository, SessionParkingRepository>();
        }

    }
}
