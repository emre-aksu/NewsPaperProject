using DataAccessLayer.Contracts.IRepositories;
using DataAccessLayer.EntityFrameWork.Context;
using InfrastructorLayer.DataAccess.Implementations;
using ModelLayer.Entities;

namespace DataAccessLayer.EntityFrameWork.Repositories
{
    public class HomeRepository:StorageRepository<Home,int,ENewsPaperDbContext>,IHomeRepository
    {

    }
}
