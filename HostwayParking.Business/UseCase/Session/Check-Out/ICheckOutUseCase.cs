using HostwayParking.Communication.Response;

namespace HostwayParking.Business.UseCase.Session.Check_Out
{
    public interface ICheckOutUseCase
    {
        Task<ResponseCheckoutJson> Execute(string plate);
    }
}