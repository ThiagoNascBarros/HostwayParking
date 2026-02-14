using HostwayParking.Communication.Request;
using HostwayParking.Domain.Interface;

namespace HostwayParking.Business.UseCase.Vehicle.Update
{
    public class UpdateVehicleUseCase : IUpdateVehicleUseCase
    {
        private readonly IVehiclesRepository _repository;
        private readonly IUnitOfWork _unitOfWork;

        public UpdateVehicleUseCase(IVehiclesRepository repository, IUnitOfWork unitOfWork)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
        }

        public async Task Execute(string plate, RequestUpdateVehicleJson request)
        {
            var vehicle = await _repository.GetByPlateAsync(plate);
            if (vehicle == null)
                throw new Exception("Veículo não encontrado!");

            vehicle.Model = request.Model;
            vehicle.Color = request.Color;
            vehicle.Type = request.Type;

            _repository.Update(vehicle);
            await _unitOfWork.Commit();
        }
    }
}
