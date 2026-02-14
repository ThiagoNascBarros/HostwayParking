using HostwayParking.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace HostwayParking.Infrastructure.DataAcess.Config
{
    public class HostwaayParkingDbContext : DbContext
    {

        public HostwaayParkingDbContext(DbContextOptions<HostwaayParkingDbContext> options) : base(options) { }

        public DbSet<Parking> Parkings { get; set; }
        public DbSet<Vehicle> Vehicles { get; set; }


    }
}
