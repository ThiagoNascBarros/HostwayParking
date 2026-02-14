using HostwayParking.Domain.Entities;
using HostwayParking.Domain.Interface;

namespace HostwayParking.Business.UseCase.Session.Check_In
{
    public class CheckInSessionUseCase : ICheckInSessionUseCase
    {

        private readonly ISessionParkingRepository _repository;
        private readonly IVehiclesRepository _vehicleRepo;
        private readonly IUnitOfWork _unitOfWork;

        public CheckInSessionUseCase(ISessionParkingRepository repository, IVehiclesRepository vehicleRepo, IUnitOfWork unitOfWork)
        {
            _repository = repository;
            _vehicleRepo = vehicleRepo;
            _unitOfWork = unitOfWork;
        }

        public async Task Execute(string plate, string model, string color)
        {
            // 1. Valida se já existe sessão aberta
            var activeSession = await _repository.GetActiveSessionByPlateAsync(plate);
            if (activeSession != null)
                throw new Exception("Veículo já está no pátio!");

            // 2. Busca ou Cria o Veículo (Simplificação para agilizar o front)
            // Você precisará de um método GetByPlate no seu VehicleRepository
            var vehicle = await _vehicleRepo.GetByPlateAsync(plate);

            if (vehicle == null)
            {
                vehicle = new Vehicle { Plate = plate, Model = model, Color = color };
                await _vehicleRepo.Post(vehicle);
                await _unitOfWork.Commit(); // Salva o veículo primeiro para ter o ID
            }

            // 3. Cria Sessão
            var session = new SessionParking(vehicle.Id);
            await _repository.AddAsync(session);
            await _unitOfWork.Commit();
        }
    }
}
