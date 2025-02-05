using ECommercePlatform.Models;

namespace ECommercePlatform.Repository.IRepository
{
    public interface IUserRepository: IRepository<User>
    {
        IEnumerable<User> GetAllDeletedUsers();
    }
}
