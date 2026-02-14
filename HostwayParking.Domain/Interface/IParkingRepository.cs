using HostwayParking.Domain.Entities;

namespace HostwayParking.Domain.Interface
{
    public interface IParkingRepository
    {
        Task Post(Parking request);
        Task<IEnumerable<Parking>> GetAll();

    }
}
