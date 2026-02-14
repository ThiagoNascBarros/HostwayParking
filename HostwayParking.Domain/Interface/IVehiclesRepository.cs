using HostwayParking.Domain.Entities;

namespace HostwayParking.Domain.Interface
{
    public interface IVehiclesRepository
    {
        Task Post(Vehicle vehicle);
        Task<IEnumerable<Vehicle>> GetAll();
        Task<Vehicle> GetByPlateAsync(string plate);
    }
}
