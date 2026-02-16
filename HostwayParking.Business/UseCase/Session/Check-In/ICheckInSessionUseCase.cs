using HostwayParking.Communication.Request;

namespace HostwayParking.Business.UseCase.Session
{
    public interface ICheckInSessionUseCase
    {
        Task Execute(RequestRegisterCheckInJson request);
    }
}
