using HostwayParking.Business.UseCase.Parking.Register;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace HostwayParking.Business
{
    public static class Injection
    {
        public static IServiceCollection AddUseCases(this IServiceCollection service, IConfiguration configuration)
        {
            AddInterfaces(service);

            return service;
        }

        private static void AddInterfaces(IServiceCollection service)
        {
            service.AddScoped<IRegisterParkingUseCase, RegisterParkingUseCase>();
        }
    }
}
