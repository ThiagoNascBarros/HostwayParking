using HostwayParking.Communication.Request;
using HostwayParking.Communication.Response;

namespace HostwayParking.Business.UseCase.Parking.Register
{
    public interface IRegisterParkingUseCase
    {

        Task<ResponseRegisterParkingJson> Register(RequestRegisterParkingJson request);
        Task<IEnumerable<ResponseGetParkingJson>> GetAll();

    }
}