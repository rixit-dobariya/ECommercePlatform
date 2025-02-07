﻿using ECommercePlatform.Models;

namespace ECommercePlatform.Repository.IRepository
{
    public interface IUnitOfWork
    {
        IRepository<Category> Categories {  get; }
        IProductRepository Products { get; }
        IRepository<ProductAttribute> ProductAttributes { get; }
        IUserRepository Users { get; }
        IRepository<Offer> Offers { get; }
        IRepository<UserOTP> UserOTPs { get; }

        void Save();
    }
}
