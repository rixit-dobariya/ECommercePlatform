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

        public IQueryable<User> GetAll()
        {
            return base.GetAll().Where(u => !u.IsDeleted);
        }

        public IQueryable<User> GetAllDeletedUsers()
        {
            return base.GetAll().Where(u => u.IsDeleted);
        }
    }
}
