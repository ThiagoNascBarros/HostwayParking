using HostwayParking.Communication.Response;
using HostwayParking.Domain.Interface;

namespace HostwayParking.Business.UseCase.Report.Occupancy
{
    public class GetOccupancyByHourUseCase : IGetOccupancyByHourUseCase
    {
        private readonly IReportRepository _reportRepo;

        public GetOccupancyByHourUseCase(IReportRepository reportRepo)
        {
            _reportRepo = reportRepo;
        }

        public async Task<ResponseOccupancyByHourJson> Execute(DateTime start, DateTime end)
        {
            if (start >= end)
                throw new Exception("A data de início deve ser anterior à data de fim.");

            var sessions = await _reportRepo.GetSessionsOverlappingPeriodAsync(start, end);

            var items = new List<OccupancyByHourItem>();

            for (int hour = 0; hour < 24; hour++)
            {
                var countsByDay = new List<int>();

                for (var day = start.Date; day < end.Date.AddDays(1); day = day.AddDays(1))
                {
                    var slotStart = day.AddHours(hour);
                    var slotEnd = slotStart.AddHours(1);

                    if (slotEnd <= start || slotStart >= end)
                        continue;

                    int count = sessions.Count(s =>
                        s.EntryTime < slotEnd &&
                        (s.ExitTime == null || s.ExitTime > slotStart));

                    countsByDay.Add(count);
                }

                if (countsByDay.Count == 0)
                    continue;

                items.Add(new OccupancyByHourItem
                {
                    Hour = hour,
                    HourRange = $"{hour:00}:00 - {(hour + 1) % 24:00}:00",
                    AverageVehicles = Math.Round(countsByDay.Average(), 2),
                    MaxVehicles = countsByDay.Max()
                });
            }

            return new ResponseOccupancyByHourJson { Items = items };
        }
    }
}
