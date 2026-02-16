using HostwayParking.Business.Exceptions;
using HostwayParking.Business.Validators;
using HostwayParking.Communication.Request;
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

        public async Task Execute(RequestRegisterCheckInJson request)
        {
            // 0. Validação de entrada
            var validation = new RegisterCheckInValidator().Validate(request);
            if (!validation.IsValid)
                throw new ValidationErrorsException(validation.Errors.Select(e => e.ErrorMessage).ToList());

            // 1. Valida se já existe sessão aberta
            var activeSession = await _repository.GetActiveSessionByPlateAsync(request.Plate);

            if (activeSession != null)
                throw new Exception("Veículo já está no pátio!");

            // 2. Busca ou Cria o Veículo (Simplificação para agilizar o front)
            var vehicle = await _vehicleRepo.GetByPlateAsync(request.Plate);

            if (vehicle == null)
            {
                vehicle = new Domain.Entities.Vehicle
                {
                    Plate = request.Plate,
                    Model = request.Model,
                    Color = request.Color,
                    Type = request.Type
                };
                await _vehicleRepo.Post(vehicle);
                await _unitOfWork.Commit();
            }

            // 3. Cria Sessão
            var session = new SessionParking(vehicle.Id);
            await _repository.AddAsync(session);
            await _unitOfWork.Commit();
        }
    }
}
