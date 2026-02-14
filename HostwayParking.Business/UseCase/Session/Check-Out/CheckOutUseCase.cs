using HostwayParking.Communication.Response;
using HostwayParking.Domain.Interface;

namespace HostwayParking.Business.UseCase.Session.Check_Out
{
    public class CheckOutUseCase : ICheckOutUseCase
    {
        private readonly ISessionParkingRepository _sessionRepo;
        private readonly IUnitOfWork _unitOfWork;

        private const decimal PRICE_FIRST_HOUR = 10.0m;
        private const decimal PRICE_EXTRA_HOUR = 5.0m;

        public CheckOutUseCase(ISessionParkingRepository sessionRepo, IUnitOfWork unitOfWork)
        {
            _sessionRepo = sessionRepo;
            _unitOfWork = unitOfWork;
        }

        public async Task<ResponseCheckoutJson> Execute(string plate)
        {
            // 1. Busca sessão ativa
            var session = await _sessionRepo.GetActiveSessionByPlateAsync(plate);
            if (session == null)
                throw new Exception("Veículo não encontrado no pátio ou já saiu.");

            // 2. Calcula Tempo
            TimeSpan duration = DateTime.Now - session.EntryTime;
            double totalHours = Math.Ceiling(duration.TotalHours); // Arredonda para cima conforme prática comum

            // 3. Calcula Valor
            decimal totalPrice = 0;
            if (totalHours <= 1)
            {
                totalPrice = PRICE_FIRST_HOUR;
            }
            else
            {
                totalPrice = PRICE_FIRST_HOUR + ((decimal)(totalHours - 1) * PRICE_EXTRA_HOUR);
            }

            // 4. Fecha Sessão
            session.CloseSession(totalPrice);
            await _sessionRepo.UpdateAsync(session);
            await _unitOfWork.Commit();

            return new ResponseCheckoutJson
            {
                EntryTime = session.EntryTime,
                ExitTime = session.ExitTime.Value,
                TotalPrice = totalPrice,
                Plate = session.Vehicle.Plate
            };
        }
    }
}
