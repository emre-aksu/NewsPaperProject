using InfrastructorLayer.DataAccess.Contracts;
using ModelLayer.Entities;

namespace DataAccessLayer.Contracts.IRepositories
{
    public interface ICategoryRepository:IStorageRepository<Category,int>
    {
    }
}
