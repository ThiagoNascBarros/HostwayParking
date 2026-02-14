using Microsoft.EntityFrameworkCore;

namespace HostwayParking.Infrastructure.DataAcess
{
    public class HostwaayParkingDbContext : DbContext
    {

        internal HostwaayParkingDbContext(DbContextOptions<HostwaayParkingDbContext> options) : base(options) { }

        public DbSet<>

    }
}
