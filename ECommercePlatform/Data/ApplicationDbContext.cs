using ECommercePlatform.Models;
using Microsoft.EntityFrameworkCore;

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
        }

    }
}
