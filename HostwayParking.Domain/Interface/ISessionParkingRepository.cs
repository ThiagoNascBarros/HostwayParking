using HostwayParking.Domain.Entities;

namespace HostwayParking.Domain.Interface
{
    public interface ISessionParkingRepository
    {
        Task AddAsync(SessionParking session);
        Task UpdateAsync(SessionParking session);
        Task<SessionParking?> GetActiveSessionByPlateAsync(string plate);
        Task<List<SessionParking>> GetAllActiveSessionsAsync();
        Task<List<SessionParking>> GetAllFinishedSessionsAsync();
    }
}
