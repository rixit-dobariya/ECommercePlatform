using ECommercePlatform.Models;

namespace ECommercePlatform.Repository.IRepository
{
    public interface IUserRepository: IRepository<User>
    {
        IEnumerable<User> GetAll();
        IEnumerable<User> GetAllDeletedUsers();
    }
}
