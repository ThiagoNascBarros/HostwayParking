using HostwayParking.Communication.Response;
using HostwayParking.Domain.Interface;

namespace HostwayParking.Business.UseCase.Vehicle.GetAll
{
    public class GetAllVehiclesUseCase : IGetAllVehiclesUseCase
    {
        private readonly IVehiclesRepository _repository;

        public GetAllVehiclesUseCase(IVehiclesRepository repository)
        {
            _repository = repository;
        }

        public async Task<List<ResponseVehicleJson>> Execute()
        {
            var vehicles = await _repository.GetAll();
            return vehicles.Select(v => new ResponseVehicleJson
            {
                Id = v.Id,
                Plate = v.Plate,
                Model = v.Model,
                Color = v.Color,
                Type = v.Type
            }).ToList();
        }
    }
}
