using HostwayParking.Communication.Response;

namespace HostwayParking.Business.UseCase.Report.Revenue
{
    public interface IGetRevenueByDayUseCase
    {
        Task<ResponseRevenueByDayJson> Execute(int days);
    }
}
