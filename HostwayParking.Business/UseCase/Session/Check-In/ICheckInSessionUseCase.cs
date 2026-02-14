namespace HostwayParking.Business.UseCase.Session
{
    public interface ICheckInSessionUseCase
    {
        Task Execute(string plate, string model, string color, string type);
    }
}
