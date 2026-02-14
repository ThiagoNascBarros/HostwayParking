using HostwayParking.Domain.Entities;
using HostwayParking.Domain.Interface;
using HostwayParking.Infrastructure.DataAcess.Config;
using Microsoft.EntityFrameworkCore;

namespace HostwayParking.Infrastructure.DataAcess.Repositories
{
    public class ParkingRepository : IParkingRepository
    {

        private readonly HostwaayParkingDbContext dbContext;

        public ParkingRepository(HostwaayParkingDbContext context)
        {
            this.dbContext = context;
        }

        public async Task<IEnumerable<Parking>> GetAll()
        {
            return await dbContext.Parkings.ToListAsync();
        }

        public async Task Post(Parking request)
        {
            await dbContext.Parkings.AddAsync(request);
        }

    }
}
