using HostwayParking.Domain.Entities;
using HostwayParking.Domain.Interface;
using HostwayParking.Infrastructure.DataAcess.Config;
using Microsoft.EntityFrameworkCore;

namespace HostwayParking.Infrastructure.DataAcess.Repositories
{
    public class VehicleRepository : IVehiclesRepository
    {

        private readonly HostwaayParkingDbContext dbContext;

        public VehicleRepository(HostwaayParkingDbContext context)
        {
            this.dbContext = context;
        }

        public async Task Post(Vehicle vehicle)
        {
            await dbContext.Vehicles.AddAsync(vehicle);
        }

        public async Task<IEnumerable<Vehicle>> GetAll()
        {
            return await dbContext.Vehicles.ToListAsync();
        }

        public Task<Vehicle?> GetByPlateAsync(string plate)
        {
            return dbContext.Vehicles.FirstOrDefaultAsync(v => v.Plate == plate);
        }

        public void Update(Vehicle vehicle)
        {
            dbContext.Vehicles.Update(vehicle);
        }
    }
}
