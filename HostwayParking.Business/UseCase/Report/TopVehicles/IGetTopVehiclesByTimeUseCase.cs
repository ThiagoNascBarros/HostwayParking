using HostwayParking.Communication.Response;

namespace HostwayParking.Business.UseCase.Report.TopVehicles
{
    public interface IGetTopVehiclesByTimeUseCase
    {
        Task<ResponseTopVehiclesByTimeJson> Execute(DateTime start, DateTime end);
    }
}
