using HostwayParking.Communication.Response;

namespace HostwayParking.Business.UseCase.Report.Occupancy
{
    public interface IGetOccupancyByHourUseCase
    {
        Task<ResponseOccupancyByHourJson> Execute(DateTime start, DateTime end);
    }
}
