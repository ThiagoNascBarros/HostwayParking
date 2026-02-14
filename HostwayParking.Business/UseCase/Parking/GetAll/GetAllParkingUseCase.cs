using HostwayParking.Business.UseCase.Parking.Register;
using HostwayParking.Domain.Interface;

namespace HostwayParking.Business.UseCase.Parking.GetAll
{
    public class GetAllParkingUseCase : IGetAllParkingUseCase
    {
        private readonly IParkingRepository _repository;
        private readonly IUnitOfWork _unitOfWork;

        public GetAllParkingUseCase(IParkingRepository repository, IUnitOfWork unitOfWork)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<ResponseGetParkingJson>> Execute()
        {
            var parkings = await _repository.GetAll();
            var response = parkings.Select(p => new ResponseGetParkingJson
            {
                Code = p.Code,
                City = p.Address.City
            });
            return response;
        }
    }
}
