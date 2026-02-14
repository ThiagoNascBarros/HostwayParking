using HostwayParking.Communication.Response;
using HostwayParking.Domain.Interface;

namespace HostwayParking.Business.UseCase.Session.Check_Out
{
    public class GetCheckOutPreviewUseCase : IGetCheckOutPreviewUseCase
    {
        private readonly ISessionParkingRepository _sessionRepo;
        private const decimal PRICE_FIRST_HOUR = 10.0m;
        private const decimal PRICE_EXTRA_HOUR = 5.0m;

        public GetCheckOutPreviewUseCase(ISessionParkingRepository sessionRepo)
        {
            _sessionRepo = sessionRepo;
        }

        public async Task<ResponseCheckoutJson> Execute(string plate)
        {
            var session = await _sessionRepo.GetActiveSessionByPlateAsync(plate);
            if (session == null)
                throw new Exception("Veículo não encontrado no pátio ou já saiu.");

            TimeSpan duration = DateTime.Now - session.EntryTime;
            double totalHours = Math.Ceiling(duration.TotalHours);

            decimal totalPrice = 0;
            if (totalHours <= 1)
            {
                totalPrice = PRICE_FIRST_HOUR;
            }
            else
            {
                totalPrice = PRICE_FIRST_HOUR + ((decimal)(totalHours - 1) * PRICE_EXTRA_HOUR);
            }

            return new ResponseCheckoutJson
            {
                EntryTime = session.EntryTime,
                ExitTime = DateTime.Now,
                TimeSpent = $"{duration.Hours:00}:{duration.Minutes:00}",
                TotalPrice = totalPrice,
                Plate = session.Vehicle.Plate,
                Model = session.Vehicle.Model
            };
        }
    }
}
