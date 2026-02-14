using HostwayParking.Communication.Response;
using HostwayParking.Domain.Interface;

namespace HostwayParking.Business.UseCase.Report.Revenue
{
    public class GetRevenueByDayUseCase : IGetRevenueByDayUseCase
    {
        private readonly IReportRepository _reportRepo;

        public GetRevenueByDayUseCase(IReportRepository reportRepo)
        {
            _reportRepo = reportRepo;
        }

        public async Task<ResponseRevenueByDayJson> Execute(int days)
        {
            if (days is not 7 and not 30)
                throw new Exception("Informe 7 ou 30 para o período de dias.");

            var end = DateTime.Now;
            var start = end.AddDays(-days).Date;

            var sessions = await _reportRepo.GetFinishedSessionsInPeriodAsync(start, end);

            var grouped = sessions
                .GroupBy(s => DateOnly.FromDateTime(s.ExitTime!.Value))
                .Select(g => new RevenueByDayItem
                {
                    Date = g.Key,
                    TotalSessions = g.Count(),
                    Revenue = g.Sum(s => s.AmountCharged ?? 0)
                })
                .OrderBy(x => x.Date)
                .ToList();

            return new ResponseRevenueByDayJson
            {
                Items = grouped,
                TotalRevenue = grouped.Sum(x => x.Revenue)
            };
        }
    }
}
