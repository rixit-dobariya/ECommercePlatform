using ECommercePlatform.Models;

namespace ECommercePlatform.Repository.IRepository
{
    public interface IUnitOfWork
    {
        //ICategoryRepository CategoryRepository { get; }
        //IProductRepository ProductRepository { get; }
        IRepository<Category> Categories {  get; }
        void Save();
    }
}
