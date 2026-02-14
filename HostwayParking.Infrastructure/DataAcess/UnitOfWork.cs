using HostwayParking.Domain.Interface;
using HostwayParking.Infrastructure.DataAcess.Config;

namespace HostwayParking.Infrastructure.DataAcess
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly HostwaayParkingDbContext _context;

        public UnitOfWork(HostwaayParkingDbContext _context)
        {
            this._context = _context;
        }

        public async Task Commit() => await _context.SaveChangesAsync();
    }
}
