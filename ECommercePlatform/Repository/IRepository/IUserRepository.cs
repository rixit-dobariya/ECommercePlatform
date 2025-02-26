using ECommercePlatform.Models;

namespace ECommercePlatform.Repository.IRepository
{
    public interface IUserRepository: IRepository<User>
    {
        IQueryable<User> GetAll();
        IQueryable<User> GetAllDeletedUsers();
    }
}
