using HostwayParking.Business.UseCase.Parking.Register;

namespace HostwayParking.Business.UseCase.Parking.GetAll
{
    public interface IGetAllParkingUseCase
    {
        Task<IEnumerable<ResponseGetParkingJson>> Execute();
    }
}
