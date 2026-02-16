using HostwayParking.Business.Exceptions;
using HostwayParking.Business.Validators;
using HostwayParking.Communication.Request;
using HostwayParking.Domain.Entities;
using HostwayParking.Domain.Interface;

namespace HostwayParking.Business.UseCase.Vehicle.Create
{
    public class CreateVehicleUseCase : ICreateVehicleUseCase
    {
        private readonly IVehiclesRepository _repository;
        private readonly IUnitOfWork _unitOfWork;

        public CreateVehicleUseCase(IVehiclesRepository repository, IUnitOfWork unitOfWork)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
        }

        public async Task Execute(RequestRegisterVehicleJson request)
        {
            var validation = new RegisterVehicleValidator().Validate(request);
            if (!validation.IsValid)
                throw new ValidationErrorsException(validation.Errors.Select(e => e.ErrorMessage).ToList());

            var exists = await _repository.GetByPlateAsync(request.Plate);
            if (exists != null)
                throw new Exception("Veículo já cadastrado!");

            var vehicle = new HostwayParking.Domain.Entities.Vehicle
            {
                Plate = request.Plate,
                Model = request.Model,
                Color = request.Color,
                Type = request.Type
            };

            await _repository.Post(vehicle);
            await _unitOfWork.Commit();
        }
    }
}
