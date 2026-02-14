using HostwayParking.Business.UseCase.Parking.GetAll;
using HostwayParking.Business.UseCase.Parking.Register;
using HostwayParking.Business.UseCase.Session;
using HostwayParking.Business.UseCase.Session.Check_In;
using HostwayParking.Business.UseCase.Session.Check_Out;
using HostwayParking.Business.UseCase.Session.List_Active;
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
            service.AddScoped<IGetAllParkingUseCase, GetAllParkingUseCase>();
            service.AddScoped<ICheckInSessionUseCase, CheckInSessionUseCase>();
            service.AddScoped<ICheckOutUseCase, CheckOutUseCase>();
            service.AddScoped<IGetAllActiveSessionsUseCase, GetAllActiveSessionsUseCase>();
        }
    }
}
