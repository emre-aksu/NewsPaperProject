using InfrastructorLayer.DataAccess.Contracts;
using ModelLayer.Entities;

namespace DataAccessLayer.Contracts.IRepositories
{
    public interface IAuthorRepository:IStorageRepository<Author,int>
    {
    }
}
