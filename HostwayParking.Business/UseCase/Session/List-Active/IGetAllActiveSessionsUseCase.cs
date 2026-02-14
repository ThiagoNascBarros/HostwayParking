using HostwayParking.Communication.Response;

namespace HostwayParking.Business.UseCase.Session.List_Active
{
    public interface IGetAllActiveSessionsUseCase
    {
        Task<List<ResponseVehicleDisplayJson>> Execute();
    }
}
