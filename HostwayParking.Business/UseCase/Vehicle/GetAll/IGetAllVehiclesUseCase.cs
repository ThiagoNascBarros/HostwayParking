using HostwayParking.Communication.Response;

namespace HostwayParking.Business.UseCase.Vehicle.GetAll
{
    public interface IGetAllVehiclesUseCase
    {
        Task<List<ResponseVehicleJson>> Execute();
    }
}
