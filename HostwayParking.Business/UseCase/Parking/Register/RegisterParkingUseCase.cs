using HostwayParking.Business.Exceptions;
using HostwayParking.Business.Validators;
using HostwayParking.Communication.Request;
using HostwayParking.Communication.Response;
using HostwayParking.Domain.Interface;

namespace HostwayParking.Business.UseCase.Parking.Register
{
    public class RegisterParkingUseCase : IRegisterParkingUseCase
    {

        private readonly IParkingRepository _repository;
        private readonly IUnitOfWork _unitOfWork;

        public RegisterParkingUseCase(IParkingRepository parkingRepository, IUnitOfWork unitOfWork)
        {
            this._repository = parkingRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<ResponseRegisterParkingJson> Execute(RequestRegisterParkingJson request)
        {
            var validation = new RegisterParkingValidator().Validate(request);
            if (!validation.IsValid)
                throw new ValidationErrorsException(validation.Errors.Select(e => e.ErrorMessage).ToList());

            var entity = new Domain.Entities.Parking()
            {
                Code = request.Code,
                Address = new HostwayParking.Domain.Entities.Embedded.Address()
                {
                    City = request.Address.City,
                    State = request.Address.State,
                    Number = request.Address.Number,
                    Supplement = request.Address.Supplement
                }
            };

            await _repository.Post(entity);
            await _unitOfWork.Commit();

            return new ResponseRegisterParkingJson()
            {
                Code = entity.Code
            };
        }


    }
}
