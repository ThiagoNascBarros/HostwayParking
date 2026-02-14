namespace HostwayParking.Domain.Interface
{
    public interface IUnitOfWork
    {
        Task Commit();
    }
}
