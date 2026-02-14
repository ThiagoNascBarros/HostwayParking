using HostwayParking.Communication.Request;
using HostwayParking.Communication.Response;

namespace HostwayParking.Business.UseCase.Vehicle.Create
{
    public interface ICreateVehicleUseCase
    {
        Task Execute(RequestRegisterVehicleJson request);
    }
}
