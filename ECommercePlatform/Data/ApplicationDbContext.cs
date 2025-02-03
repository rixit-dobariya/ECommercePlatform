using ECommercePlatform.Models;
using Microsoft.EntityFrameworkCore;
using System;

namespace ECommercePlatform.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions options) : base(options)
        {
        }

        DbSet<Category> Categories { get; set; }
        DbSet<Product> Products { get; set; }
        DbSet<ProductAttribute> ProductAttributes { get; set; }
        DbSet<Address> Addresses { get; set; }
        DbSet<CartItem> CartItems { get; set; }
        DbSet<Offer> Offers { get; set; }
        DbSet<OrderDetail> OrderDetails { get; set; }
        DbSet<OrderHeader> orderHeaders { get; set; }
        DbSet<User> Users { get; set; }
        DbSet<WishlistItem> WishlistItems { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<WishlistItem>()
                .HasKey(wi => new { wi.UserId, wi.ProductId });
            modelBuilder.Entity<CartItem>()
                .HasKey(ci => new { ci.UserId, ci.ProductId });
            modelBuilder.Entity<OrderDetail>()
                .HasKey(od => new { od.OrderId, od.ProductId });

            modelBuilder.Entity<OrderHeader>()
                .HasOne(o => o.ShippingAddress)
                .WithMany()
                .HasForeignKey(o => o.ShippingAddressId)
                .OnDelete(DeleteBehavior.NoAction);  // 👈 Prevents cascade delete

            modelBuilder.Entity<OrderHeader>()
                .HasOne(o => o.BillingAddress)
                .WithMany()
                .HasForeignKey(o => o.BillingAddressId)
                .OnDelete(DeleteBehavior.NoAction);  // 👈 Prevents cascade delete

            //seed tables

            //category
            modelBuilder.Entity<Category>().HasData(
                new Category { CategoryId = 1, Name = "Electronics", ParentCategoryId = null },
                new Category { CategoryId = 2, Name = "Laptops", ParentCategoryId = 1 },
                new Category { CategoryId = 3, Name = "Smartphones", ParentCategoryId = 1 },
                new Category { CategoryId = 4, Name = "Fashion", ParentCategoryId = null },
                new Category { CategoryId = 5, Name = "Men's Clothing", ParentCategoryId = 4 },
                new Category { CategoryId = 6, Name = "Women's Clothing", ParentCategoryId = 4 },
                new Category { CategoryId = 7, Name = "Home Appliances", ParentCategoryId = null },
                new Category { CategoryId = 8, Name = "Refrigerators", ParentCategoryId = 7 },
                new Category { CategoryId = 9, Name = "Washing Machines", ParentCategoryId = 7 }
            );

            //products
            modelBuilder.Entity<Product>().HasData(
                new Product
                {
                    ProductId=1,
                    Name = "Laptop",
                    Description = "High-end gaming laptop",
                    CategoryId = 2,
                    Stock = 10,
                    SellPrice = 1500.00m,
                    CostPrice = 1200.00m,
                    Discount = 10.0m,
                    ImageUrl = "th\\Images\\profile.jpg",
                    IsActive = 1
                },
                new Product
                {
                    ProductId = 2,
                    Name = "Smartphone",
                    Description = "Latest Android smartphone",
                    CategoryId = 3,
                    Stock = 20,
                    SellPrice = 800.00m,
                    CostPrice = 600.00m,
                    Discount = 5.0m,
                    ImageUrl = "th\\Images\\profile.jpg",
                    IsActive = 1
                },

                // Products for Fashion category
                new Product
                {
                    ProductId = 3,
                    Name = "Men's T-shirt",
                    Description = "Comfortable cotton t-shirt",
                    CategoryId = 5,
                    Stock = 50,
                    SellPrice = 20.00m,
                    CostPrice = 12.00m,
                    Discount = 0.0m,
                    ImageUrl = "th\\Images\\profile.jpg",
                    IsActive = 1
                },
                new Product
                {
                    ProductId = 4,
                    Name = "Women's Dress",
                    Description = "Elegant summer dress",
                    CategoryId = 6,
                    Stock = 30,
                    SellPrice = 35.00m,
                    CostPrice = 25.00m,
                    Discount = 10.0m,
                    ImageUrl = "th\\Images\\profile.jpg",
                    IsActive = 1
                },

                // Products for Home Appliances category
                new Product
                {
                    ProductId = 5,
                    Name = "Refrigerator",
                    Description = "Energy-efficient refrigerator",
                    CategoryId = 8,
                    Stock = 15,
                    SellPrice = 1200.00m,
                    CostPrice = 950.00m,
                    Discount = 15.0m,
                    ImageUrl = "th\\Images\\profile.jpg",
                    IsActive = 1
                },
                new Product
                {
                    ProductId = 6,
                    Name = "Washing Machine",
                    Description = "Front-load washing machine",
                    CategoryId = 9,
                    Stock = 25,
                    SellPrice = 500.00m,
                    CostPrice = 400.00m,
                    Discount = 5.0m,
                    ImageUrl = "th\\Images\\profile.jpg",
                    IsActive = 1
                }
            );
            //product attributes
            modelBuilder.Entity<ProductAttribute>().HasData(
                new ProductAttribute { AttributeID=1, ProductID = 1, Name = "Brand", Value = "BrandX" },
                new ProductAttribute { AttributeID = 2, ProductID = 1, Name = "Warranty", Value = "2 years" },
                new ProductAttribute { AttributeID = 3, ProductID = 2, Name = "Brand", Value = "BrandY" },
                new ProductAttribute { AttributeID = 4, ProductID = 2, Name = "Battery Life", Value = "12 hours" },
                new ProductAttribute { AttributeID = 5, ProductID = 3, Name = "Material", Value = "Cotton" },
                new ProductAttribute { AttributeID = 6, ProductID = 4, Name = "Material", Value = "Polyester" },
                new ProductAttribute { AttributeID = 7, ProductID = 5, Name = "Energy Rating", Value = "A+" },
                new ProductAttribute { AttributeID = 8, ProductID = 6, Name = "Load Capacity", Value = "8 kg" }
            );
        }

    }
}
