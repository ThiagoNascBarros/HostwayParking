using HostwayParking.Communication.Response;
using HostwayParking.Domain.Interface;

namespace HostwayParking.Business.UseCase.Session.List_Active
{
    public class GetAllActiveSessionsUseCase : IGetAllActiveSessionsUseCase
    {
        private readonly ISessionParkingRepository _repository;

        public GetAllActiveSessionsUseCase(ISessionParkingRepository repository)
        {
            _repository = repository;
        }

        public async Task<List<ResponseVehicleDisplayJson>> Execute()
        {
            var sessions = await _repository.GetAllActiveSessionsAsync();

            return sessions.Select(s => new ResponseVehicleDisplayJson
            {
                Plate = s.Vehicle?.Plate ?? string.Empty,
                Model = s.Vehicle?.Model ?? string.Empty,
                Color = s.Vehicle?.Color ?? string.Empty,
                EntryTime = s.EntryTime
            }).ToList();
        }
    }
}
