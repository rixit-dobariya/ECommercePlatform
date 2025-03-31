using ECommercePlatform.Constants;
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
        DbSet<OrderHeader> OrderHeaders { get; set; }
        DbSet<User> Users { get; set; }
        DbSet<WishlistItem> WishlistItems { get; set; }
        DbSet<UserOTP> UserOTPs { get; set; }
        DbSet<Review> Reviews { get; set; }
        DbSet<Response> Responses { get; set; }
        DbSet<ContactDetails> ContactDetails { get; set; }
        DbSet<AboutPageContent> AboutPageContent { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<WishlistItem>()
                .HasKey(wi => new { wi.UserId, wi.ProductId });
            modelBuilder.Entity<CartItem>()
                .HasKey(ci => new { ci.UserId, ci.ProductId });
            modelBuilder.Entity<OrderDetail>()
                .HasKey(od => new { od.OrderHeaderId, od.ProductId });

            
            modelBuilder.Entity<OrderHeader>()
                .HasOne(o => o.ShippingAddress)
                .WithMany()
                .HasForeignKey(o => o.ShippingAddressId)
                .OnDelete(DeleteBehavior.NoAction);  // Prevents cascade delete

            modelBuilder.Entity<OrderHeader>()
                .HasOne(o => o.BillingAddress)
                .WithMany()
                .HasForeignKey(o => o.BillingAddressId)
                .OnDelete(DeleteBehavior.NoAction);  // Prevents cascade delete

            modelBuilder.Entity<OrderDetail>()
                .HasOne(od => od.OrderHeader)
                .WithMany(oh => oh.OrderDetails)
                .HasForeignKey(od => od.OrderHeaderId)
                .OnDelete(DeleteBehavior.Cascade); // If OrderHeader is deleted, OrderDetails should also be deleted.


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
                    LongDescription = "High-end gaming laptop",
                    ShortDescription = "High-end gaming laptop",
                    CategoryId = 2,
                    Stock = 10,
                    SellPrice = 1500.00m,
                    CostPrice = 1200.00m,
                    Discount = 10.0m,
                    ImageUrl = "\\Images\\profile.jpg",
                    IsActive = true
                },
                new Product
                {
                    ProductId = 2,
                    Name = "Smartphone",
                    LongDescription = "Latest Android smartphone",
                    ShortDescription = "Latest Android smartphone",
                    CategoryId = 3,
                    Stock = 20,
                    SellPrice = 800.00m,
                    CostPrice = 600.00m,
                    Discount = 5.0m,
                    ImageUrl = "\\Images\\profile.jpg",
                    IsActive = true
                },

                // Products for Fashion category
                new Product
                {
                    ProductId = 3,
                    Name = "Men's T-shirt",
                    ShortDescription = "Comfortable cotton t-shirt",
                    LongDescription = "Comfortable cotton t-shirt",
                    CategoryId = 5,
                    Stock = 50,
                    SellPrice = 20.00m,
                    CostPrice = 12.00m,
                    Discount = 0.0m,
                    ImageUrl = "\\Images\\profile.jpg",
                    IsActive = true 
                },
                new Product
                {
                    ProductId = 4,
                    Name = "Women's Dress",
                    ShortDescription = "Elegant summer dress",
                    LongDescription = "Elegant summer dress",
                    CategoryId = 6,
                    Stock = 30,
                    SellPrice = 35.00m,
                    CostPrice = 25.00m,
                    Discount = 10.0m,
                    ImageUrl = "\\Images\\profile.jpg",
                    IsActive = true
                },

                // Products for Home Appliances category
                new Product
                {
                    ProductId = 5,
                    Name = "Refrigerator",
                    ShortDescription = "Energy-efficient refrigerator",
                    LongDescription = "Energy-efficient refrigerator",
                    CategoryId = 8,
                    Stock = 15,
                    SellPrice = 1200.00m,
                    CostPrice = 950.00m,
                    Discount = 15.0m,
                    ImageUrl = "\\Images\\profile.jpg",
                    IsActive = true
                },
                new Product
                {
                    ProductId = 6,
                    Name = "Washing Machine",
                    ShortDescription = "Front-load washing machine",
                    LongDescription = "Front-load washing machine",
                    CategoryId = 9,
                    Stock = 25,
                    SellPrice = 500.00m,
                    CostPrice = 400.00m,
                    Discount = 5.0m,
                    ImageUrl = "\\Images\\profile.jpg",
                    IsActive = true
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
            modelBuilder.Entity<User>().HasData(
                new User
                {
                    UserId=1,
                    Email = "Admin@admin.com",
                    FullName = "Admin",
                    IsDeleted = false,
                    IsEmailVerified = true,
                    Password = "$2a$11$aZsiddJKDzxfpg2rdgqLZOWyxRoVrZUNsJnYFd3EORCmnpoAPKwlm",
                    Phone = "8732965892",
                    ProfilePicture = "/Images/users/d8dadcfc-e8b8-480a-94b7-ca496880db91.png",
                    Role = RoleType.Admin
                },
                new User
                {
                    UserId = 2,
                    Email = "rixitdobariya05@gmail.com",
                    FullName = "Rixit Dobariya",
                    IsDeleted = false,
                    IsEmailVerified = true,
                    Password = "$2a$11$aZsiddJKDzxfpg2rdgqLZOWyxRoVrZUNsJnYFd3EORCmnpoAPKwlm",
                    Phone = "8732965892",
                    ProfilePicture = "/Images/users/d8dadcfc-e8b8-480a-94b7-ca496880db91.png",
                    Role = RoleType.User
                }
            );
            modelBuilder.Entity<Offer>().HasData(
                new Offer
                {
                    OfferId = 1,
                    OfferCode = "DISCOUNT10",
                    OfferDescription = "Get 10% off on all orders above $50",
                    MinimumAmount = 50.00m,
                    Discount = 10.00m,
                    StartDate = new DateOnly(2025, 2, 1),
                    EndDate = new DateOnly(2025, 2, 28)
                }
            ); 
            modelBuilder.Entity<Address>().HasData(
                new Address
                {
                    AddressId = 1,
                    UserId = 2, // Assuming this user exists in the Users table
                    City = "New York",
                    Region = "Manhattan",
                    State = "NY",
                    PinCode = "10001",
                    Phone = "1234567890",
                    IsDeleted = 0,
                    FirstName = "Rixit",
                    LastName = "Dobariya"
                },
                new Address
                {
                    AddressId = 2,
                    UserId = 2, // Assuming this user exists in the Users table
                    City = "Los Angeles",
                    Region = "Downtown",
                    State = "CA",
                    PinCode = "90001",
                    Phone = "9876543210",
                    IsDeleted = 0,
                    FirstName = "Rixit",
                    LastName = "Dobariya"
                }
            );
            // Seeding OrderHeaders
            modelBuilder.Entity<OrderHeader>().HasData(
                new OrderHeader
                {
                    OrderId = 1,
                    UserId = 2, // Assuming this user exists in Users table
                    OrderStatus = OrderStatus.Pending, // Example status (e.g., Pending)
                    OrderDate = new DateTime(2025, 2, 16),
                    ShippingAddressId = 1, // Assuming this address exists
                    BillingAddressId = 2, // Assuming this address exists
                    ShippingCharge = 5.00m,
                    Subtotal = 100.00m,
                    PaymentMode = PaymentMode.UPI // Example payment mode (e.g., Credit Card)
                },
                new OrderHeader
                {
                    OrderId = 2,
                    UserId = 2, // Another user
                    OrderStatus = OrderStatus.Pending, // Example status (e.g., Shipped)
                    OrderDate = new DateTime(2025, 2, 17),
                    ShippingAddressId = 2, // Assuming this address exists
                    BillingAddressId = 2, // Same address for billing
                    ShippingCharge = 0.00m, // Free shipping
                    Subtotal = 50.00m,
                    PaymentMode = PaymentMode.UPI // Example payment mode (e.g., PayPal)
                }
            );

            // Seeding OrderDetails
            modelBuilder.Entity<OrderDetail>().HasData(
                new OrderDetail
                {
                    OrderHeaderId = 1,
                    ProductId = 1, // Assuming this product exists
                    Quantity = 2,
                    Price = 50.00m,
                    DiscountAmount = 5.00m
                },
                new OrderDetail
                {
                    OrderHeaderId = 2,
                    ProductId = 2, // Another product
                    Quantity = 1,
                    Price = 50.00m,
                    DiscountAmount = 0.00m
                }
            );
            modelBuilder.Entity<ContactDetails>().HasData(
                new ContactDetails()
                {
                    Id = 1,
                    Address = "Rk University, <br>Tramba<br>Phone: 8732965892",
                    Emails = "purebite@gmail.com,purebitegrocery@gmail.com",
                    PhoneNumbers = "+91 87329 65892,+91 97370 74939"
                }
            );
            modelBuilder.Entity<AboutPageContent>().HasData(
            new AboutPageContent()
            {
                Id = 1,
                Description = "<p>Welcome to <strong>Flone</strong>, your one-stop destination for quality products at the best prices. We are committed to offering a seamless shopping experience with a diverse range of categories to explore.</p>",
                Goal = "<p>At <strong>Flone</strong>, our goal is to revolutionize online shopping by providing top-quality products, unbeatable deals, and excellent customer service.</p>",
                Mission = "<p>Our mission is to build a trusted eCommerce platform where customers can shop with confidence, knowing they are getting the best products and services.</p>",
                Vision = "<p>We envision <strong>Flone</strong> as a leading eCommerce brand, known for its reliability, affordability, and customer satisfaction.</p>"
            }
        );

        }

    }
}
