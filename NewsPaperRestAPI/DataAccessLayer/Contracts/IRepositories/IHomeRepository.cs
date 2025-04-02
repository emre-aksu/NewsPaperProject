using InfrastructorLayer.DataAccess.Contracts;
using ModelLayer.Entities;

namespace DataAccessLayer.Contracts.IRepositories
{
    public interface IHomeRepository:IStorageRepository<Home,int>
    {
    }
}
