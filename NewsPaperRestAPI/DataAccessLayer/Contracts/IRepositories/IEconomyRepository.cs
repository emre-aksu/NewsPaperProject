using InfrastructorLayer.DataAccess.Contracts;
using ModelLayer.Entities;

namespace DataAccessLayer.Contracts.IRepositories
{
    public interface IEconomyRepository : IStorageRepository<Economy, int>
    {
    }
}
