using HostwayParking.Communication.Request;
using HostwayParking.Communication.Response;

namespace HostwayParking.Business.UseCase.Parking.Register
{
    public interface IRegisterParkingUseCase
    {

        Task<ResponseRegisterParkingJson> Execute(RequestRegisterParkingJson request);

    }
}