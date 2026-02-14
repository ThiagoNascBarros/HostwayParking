using HostwayParking.Business.UseCase.Parking.GetAll;
using HostwayParking.Business.UseCase.Parking.Register;
using HostwayParking.Business.UseCase.Report.Occupancy;
using HostwayParking.Business.UseCase.Report.Revenue;
using HostwayParking.Business.UseCase.Report.TopVehicles;
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
            service.AddScoped<IGetCheckOutPreviewUseCase, GetCheckOutPreviewUseCase>();
            service.AddScoped<IGetAllActiveSessionsUseCase, GetAllActiveSessionsUseCase>();
            // Vehicle
            service.AddScoped<UseCase.Vehicle.Create.ICreateVehicleUseCase, UseCase.Vehicle.Create.CreateVehicleUseCase>();
            service.AddScoped<UseCase.Vehicle.Update.IUpdateVehicleUseCase, UseCase.Vehicle.Update.UpdateVehicleUseCase>();
            service.AddScoped<UseCase.Vehicle.GetAll.IGetAllVehiclesUseCase, UseCase.Vehicle.GetAll.GetAllVehiclesUseCase>();
            // Reports
            service.AddScoped<IGetRevenueByDayUseCase, GetRevenueByDayUseCase>();
            service.AddScoped<IGetTopVehiclesByTimeUseCase, GetTopVehiclesByTimeUseCase>();
            service.AddScoped<IGetOccupancyByHourUseCase, GetOccupancyByHourUseCase>();
        }
    }
}
