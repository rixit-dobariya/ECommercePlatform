using ECommercePlatform.Data;
using ECommercePlatform.Models;
using ECommercePlatform.Repository.IRepository;

namespace ECommercePlatform.Repository
{
    public class UserRepository : Repository<User>, IUserRepository
    {
        public UserRepository(ApplicationDbContext db) : base(db)
        {
        }

        public IEnumerable<User> GetAll()
        {
            return base.GetAll().Where(u => !u.IsDeleted);
        }

        public IEnumerable<User> GetAllDeletedUsers()
        {
            return base.GetAll().Where(u => u.IsDeleted);
        }
    }
}
