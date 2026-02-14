using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace HostwayParking.Infrastructure.DataAcess
{
    public static class Injection
    {
        public static IServiceCollection AddInfraestructure(this IServiceCollection service, IConfiguration configuration)
        {
            service.AddDbContext<HostwaayParkingDbContext>(options =>
                options.UseSqlite(configuration.GetConnectionString("DefaultConnection"),
                b => b.MigrationsAssembly(typeof(HostwaayParkingDbContext).Assembly.FullName)));


            return service;
        }
    }
}
