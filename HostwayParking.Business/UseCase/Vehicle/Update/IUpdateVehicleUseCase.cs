using HostwayParking.Communication.Request;

namespace HostwayParking.Business.UseCase.Vehicle.Update
{
    public interface IUpdateVehicleUseCase
    {
        Task Execute(string plate, RequestUpdateVehicleJson request);
    }
}
