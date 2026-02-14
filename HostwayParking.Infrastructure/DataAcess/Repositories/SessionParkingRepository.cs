using HostwayParking.Domain.Entities;
using HostwayParking.Domain.Interface;
using HostwayParking.Infrastructure.DataAcess.Config;
using Microsoft.EntityFrameworkCore;

namespace HostwayParking.Infrastructure.DataAcess.Repositories
{
    public class SessionParkingRepository : ISessionParkingRepository
    {
        private readonly HostwaayParkingDbContext _context;

        public SessionParkingRepository(HostwaayParkingDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(SessionParking session)
        {
            await _context.SessionParkings.AddAsync(session);
        }

        public async Task UpdateAsync(SessionParking session)
        {
            _context.SessionParkings.Update(session);
        }

        public async Task<SessionParking?> GetActiveSessionByPlateAsync(string plate)
        {
            // EF Core can't translate the computed property IsActive, so we filter by ExitTime instead
            return await _context.SessionParkings
                .Include(s => s.Vehicle)
                .Where(s => s.Vehicle.Plate == plate && s.ExitTime == null)
                .FirstOrDefaultAsync();
        }

        public async Task<List<SessionParking>> GetAllActiveSessionsAsync()
        {
            return await _context.SessionParkings
                .Include(x => x.Vehicle)
                .Where(x => x.ExitTime == null)
                .ToListAsync();
        }

        public async Task<List<SessionParking>> GetAllFinishedSessionsAsync()
        {
            return await _context.SessionParkings
                .Where(x => x.ExitTime != null)
                .ToListAsync();
        }

    }
}
