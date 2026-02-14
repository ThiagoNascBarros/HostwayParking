using HostwayParking.Domain.Entities;

namespace HostwayParking.Domain.Interface
{
    public interface IReportRepository
    {
        Task<List<SessionParking>> GetFinishedSessionsInPeriodAsync(DateTime start, DateTime end);
        Task<List<SessionParking>> GetSessionsOverlappingPeriodAsync(DateTime start, DateTime end);
    }
}
