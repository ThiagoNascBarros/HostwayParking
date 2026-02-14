using HostwayParking.Communication.Response;
using HostwayParking.Domain.Interface;

namespace HostwayParking.Business.UseCase.Report.TopVehicles
{
    public class GetTopVehiclesByTimeUseCase : IGetTopVehiclesByTimeUseCase
    {
        private readonly IReportRepository _reportRepo;

        public GetTopVehiclesByTimeUseCase(IReportRepository reportRepo)
        {
            _reportRepo = reportRepo;
        }

        public async Task<ResponseTopVehiclesByTimeJson> Execute(DateTime start, DateTime end)
        {
            if (start >= end)
                throw new Exception("A data de início deve ser anterior à data de fim.");

            var sessions = await _reportRepo.GetSessionsOverlappingPeriodAsync(start, end);

            var ranked = sessions
                .GroupBy(s => s.VehicleId)
                .Select(g =>
                {
                    var vehicle = g.First().Vehicle;
                    var totalMinutes = g.Sum(s =>
                    {
                        var entry = s.EntryTime < start ? start : s.EntryTime;
                        var exit = s.ExitTime == null || s.ExitTime > end ? end : s.ExitTime.Value;
                        return (exit - entry).TotalMinutes;
                    });

                    var timeSpan = TimeSpan.FromMinutes(totalMinutes);

                    return new TopVehicleByTimeItem
                    {
                        Plate = vehicle.Plate,
                        Model = vehicle.Model,
                        TotalSessions = g.Count(),
                        TotalMinutes = Math.Round(totalMinutes, 2),
                        TotalTimeParked = $"{(int)timeSpan.TotalHours:00}:{timeSpan.Minutes:00}:{timeSpan.Seconds:00}"
                    };
                })
                .OrderByDescending(x => x.TotalMinutes)
                .Take(10)
                .ToList();

            return new ResponseTopVehiclesByTimeJson { Items = ranked };
        }
    }
}
