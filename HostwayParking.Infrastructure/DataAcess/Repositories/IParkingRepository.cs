using HostwayParking.Domain.Entities;

namespace HostwayParking.Infrastructure.DataAcess.Repositories
{
    public interface IParkingRepository
    {

        Task<Parking> Get();

    }
}
