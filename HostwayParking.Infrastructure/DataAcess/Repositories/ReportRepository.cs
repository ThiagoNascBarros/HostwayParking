using HostwayParking.Domain.Entities;
using HostwayParking.Domain.Interface;
using HostwayParking.Infrastructure.DataAcess.Config;
using Microsoft.EntityFrameworkCore;

namespace HostwayParking.Infrastructure.DataAcess.Repositories
{
    public class ReportRepository : IReportRepository
    {
        private readonly HostwaayParkingDbContext _context;

        public ReportRepository(HostwaayParkingDbContext context)
        {
            _context = context;
        }

        public async Task<List<SessionParking>> GetFinishedSessionsInPeriodAsync(DateTime start, DateTime end)
        {
            return await _context.SessionParkings
                .Include(s => s.Vehicle)
                .Where(s => s.ExitTime != null
                         && s.ExitTime >= start
                         && s.ExitTime <= end)
                .ToListAsync();
        }

        public async Task<List<SessionParking>> GetSessionsOverlappingPeriodAsync(DateTime start, DateTime end)
        {
            return await _context.SessionParkings
                .Include(s => s.Vehicle)
                .Where(s => s.EntryTime < end
                         && (s.ExitTime == null || s.ExitTime > start))
                .ToListAsync();
        }
    }
}
