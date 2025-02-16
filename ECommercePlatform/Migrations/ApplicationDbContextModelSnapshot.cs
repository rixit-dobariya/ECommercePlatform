﻿// <auto-generated />
using System;
using ECommercePlatform.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace ECommercePlatform.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    partial class ApplicationDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "9.0.1")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("ECommercePlatform.Models.Address", b =>
                {
                    b.Property<int>("AddressId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("AddressId"));

                    b.Property<string>("City")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("IsDeleted")
                        .HasColumnType("int");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Phone")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PinCode")
                        .IsRequired()
                        .HasMaxLength(6)
                        .HasColumnType("nvarchar(6)");

                    b.Property<string>("Region")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("State")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("AddressId");

                    b.HasIndex("UserId");

                    b.ToTable("Addresses");

                    b.HasData(
                        new
                        {
                            AddressId = 1,
                            City = "New York",
                            FirstName = "Rixit",
                            IsDeleted = 0,
                            LastName = "Dobariya",
                            Phone = "1234567890",
                            PinCode = "10001",
                            Region = "Manhattan",
                            State = "NY",
                            UserId = 2
                        },
                        new
                        {
                            AddressId = 2,
                            City = "Los Angeles",
                            FirstName = "Rixit",
                            IsDeleted = 0,
                            LastName = "Dobariya",
                            Phone = "9876543210",
                            PinCode = "90001",
                            Region = "Downtown",
                            State = "CA",
                            UserId = 2
                        });
                });

            modelBuilder.Entity("ECommercePlatform.Models.CartItem", b =>
                {
                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.Property<int>("ProductId")
                        .HasColumnType("int");

                    b.Property<int>("Quantity")
                        .HasColumnType("int");

                    b.HasKey("UserId", "ProductId");

                    b.HasIndex("ProductId");

                    b.ToTable("CartItems");
                });

            modelBuilder.Entity("ECommercePlatform.Models.Category", b =>
                {
                    b.Property<int>("CategoryId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("CategoryId"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("ParentCategoryId")
                        .HasColumnType("int");

                    b.HasKey("CategoryId");

                    b.HasIndex("ParentCategoryId");

                    b.ToTable("Categories");

                    b.HasData(
                        new
                        {
                            CategoryId = 1,
                            Name = "Electronics"
                        },
                        new
                        {
                            CategoryId = 2,
                            Name = "Laptops",
                            ParentCategoryId = 1
                        },
                        new
                        {
                            CategoryId = 3,
                            Name = "Smartphones",
                            ParentCategoryId = 1
                        },
                        new
                        {
                            CategoryId = 4,
                            Name = "Fashion"
                        },
                        new
                        {
                            CategoryId = 5,
                            Name = "Men's Clothing",
                            ParentCategoryId = 4
                        },
                        new
                        {
                            CategoryId = 6,
                            Name = "Women's Clothing",
                            ParentCategoryId = 4
                        },
                        new
                        {
                            CategoryId = 7,
                            Name = "Home Appliances"
                        },
                        new
                        {
                            CategoryId = 8,
                            Name = "Refrigerators",
                            ParentCategoryId = 7
                        },
                        new
                        {
                            CategoryId = 9,
                            Name = "Washing Machines",
                            ParentCategoryId = 7
                        });
                });

            modelBuilder.Entity("ECommercePlatform.Models.Offer", b =>
                {
                    b.Property<int>("OfferId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("OfferId"));

                    b.Property<decimal>("Discount")
                        .HasColumnType("decimal(18,2)");

                    b.Property<DateOnly>("EndDate")
                        .HasColumnType("date");

                    b.Property<decimal>("MinimumAmount")
                        .HasColumnType("decimal(18,2)");

                    b.Property<string>("OfferCode")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("OfferDescription")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateOnly>("StartDate")
                        .HasColumnType("date");

                    b.HasKey("OfferId");

                    b.ToTable("Offers");

                    b.HasData(
                        new
                        {
                            OfferId = 1,
                            Discount = 10.00m,
                            EndDate = new DateOnly(2025, 2, 28),
                            MinimumAmount = 50.00m,
                            OfferCode = "DISCOUNT10",
                            OfferDescription = "Get 10% off on all orders above $50",
                            StartDate = new DateOnly(2025, 2, 1)
                        });
                });

            modelBuilder.Entity("ECommercePlatform.Models.OrderDetail", b =>
                {
                    b.Property<int>("OrderHeaderId")
                        .HasColumnType("int");

                    b.Property<int>("ProductId")
                        .HasColumnType("int");

                    b.Property<decimal>("DiscountAmount")
                        .HasColumnType("decimal(18,2)");

                    b.Property<decimal>("Price")
                        .HasColumnType("decimal(18,2)");

                    b.Property<int>("Quantity")
                        .HasColumnType("int");

                    b.HasKey("OrderHeaderId", "ProductId");

                    b.HasIndex("ProductId");

                    b.ToTable("OrderDetails");

                    b.HasData(
                        new
                        {
                            OrderHeaderId = 1,
                            ProductId = 1,
                            DiscountAmount = 5.00m,
                            Price = 50.00m,
                            Quantity = 2
                        },
                        new
                        {
                            OrderHeaderId = 2,
                            ProductId = 2,
                            DiscountAmount = 0.00m,
                            Price = 50.00m,
                            Quantity = 1
                        });
                });

            modelBuilder.Entity("ECommercePlatform.Models.OrderHeader", b =>
                {
                    b.Property<int>("OrderId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("OrderId"));

                    b.Property<int?>("BillingAddressId")
                        .HasColumnType("int");

                    b.Property<DateTime>("OrderDate")
                        .HasColumnType("datetime2");

                    b.Property<int>("OrderStatus")
                        .HasColumnType("int");

                    b.Property<int>("PaymentMode")
                        .HasColumnType("int");

                    b.Property<int>("ShippingAddressId")
                        .HasColumnType("int");

                    b.Property<decimal>("ShippingCharge")
                        .HasColumnType("decimal(18,2)");

                    b.Property<decimal>("Subtotal")
                        .HasColumnType("decimal(18,2)");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("OrderId");

                    b.HasIndex("BillingAddressId");

                    b.HasIndex("ShippingAddressId");

                    b.HasIndex("UserId");

                    b.ToTable("OrderHeaders");

                    b.HasData(
                        new
                        {
                            OrderId = 1,
                            BillingAddressId = 2,
                            OrderDate = new DateTime(2025, 2, 16, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            OrderStatus = 0,
                            PaymentMode = 0,
                            ShippingAddressId = 1,
                            ShippingCharge = 5.00m,
                            Subtotal = 100.00m,
                            UserId = 2
                        },
                        new
                        {
                            OrderId = 2,
                            BillingAddressId = 2,
                            OrderDate = new DateTime(2025, 2, 17, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            OrderStatus = 0,
                            PaymentMode = 0,
                            ShippingAddressId = 2,
                            ShippingCharge = 0.00m,
                            Subtotal = 50.00m,
                            UserId = 2
                        });
                });

            modelBuilder.Entity("ECommercePlatform.Models.Product", b =>
                {
                    b.Property<int>("ProductId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ProductId"));

                    b.Property<int>("CategoryId")
                        .HasColumnType("int");

                    b.Property<decimal>("CostPrice")
                        .HasColumnType("decimal(18,2)");

                    b.Property<decimal>("Discount")
                        .HasColumnType("decimal(18,2)");

                    b.Property<string>("ImageUrl")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("IsActive")
                        .HasColumnType("int");

                    b.Property<string>("LongDescription")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<decimal>("SellPrice")
                        .HasColumnType("decimal(18,2)");

                    b.Property<string>("ShortDescription")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Stock")
                        .HasColumnType("int");

                    b.HasKey("ProductId");

                    b.HasIndex("CategoryId");

                    b.ToTable("Products");

                    b.HasData(
                        new
                        {
                            ProductId = 1,
                            CategoryId = 2,
                            CostPrice = 1200.00m,
                            Discount = 10.0m,
                            ImageUrl = "\\Images\\profile.jpg",
                            IsActive = 1,
                            LongDescription = "High-end gaming laptop",
                            Name = "Laptop",
                            SellPrice = 1500.00m,
                            ShortDescription = "High-end gaming laptop",
                            Stock = 10
                        },
                        new
                        {
                            ProductId = 2,
                            CategoryId = 3,
                            CostPrice = 600.00m,
                            Discount = 5.0m,
                            ImageUrl = "\\Images\\profile.jpg",
                            IsActive = 1,
                            LongDescription = "Latest Android smartphone",
                            Name = "Smartphone",
                            SellPrice = 800.00m,
                            ShortDescription = "Latest Android smartphone",
                            Stock = 20
                        },
                        new
                        {
                            ProductId = 3,
                            CategoryId = 5,
                            CostPrice = 12.00m,
                            Discount = 0.0m,
                            ImageUrl = "\\Images\\profile.jpg",
                            IsActive = 1,
                            LongDescription = "Comfortable cotton t-shirt",
                            Name = "Men's T-shirt",
                            SellPrice = 20.00m,
                            ShortDescription = "Comfortable cotton t-shirt",
                            Stock = 50
                        },
                        new
                        {
                            ProductId = 4,
                            CategoryId = 6,
                            CostPrice = 25.00m,
                            Discount = 10.0m,
                            ImageUrl = "\\Images\\profile.jpg",
                            IsActive = 1,
                            LongDescription = "Elegant summer dress",
                            Name = "Women's Dress",
                            SellPrice = 35.00m,
                            ShortDescription = "Elegant summer dress",
                            Stock = 30
                        },
                        new
                        {
                            ProductId = 5,
                            CategoryId = 8,
                            CostPrice = 950.00m,
                            Discount = 15.0m,
                            ImageUrl = "\\Images\\profile.jpg",
                            IsActive = 1,
                            LongDescription = "Energy-efficient refrigerator",
                            Name = "Refrigerator",
                            SellPrice = 1200.00m,
                            ShortDescription = "Energy-efficient refrigerator",
                            Stock = 15
                        },
                        new
                        {
                            ProductId = 6,
                            CategoryId = 9,
                            CostPrice = 400.00m,
                            Discount = 5.0m,
                            ImageUrl = "\\Images\\profile.jpg",
                            IsActive = 1,
                            LongDescription = "Front-load washing machine",
                            Name = "Washing Machine",
                            SellPrice = 500.00m,
                            ShortDescription = "Front-load washing machine",
                            Stock = 25
                        });
                });

            modelBuilder.Entity("ECommercePlatform.Models.ProductAttribute", b =>
                {
                    b.Property<int>("AttributeID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("AttributeID"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("ProductID")
                        .HasColumnType("int");

                    b.Property<string>("Value")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("AttributeID");

                    b.HasIndex("ProductID");

                    b.ToTable("ProductAttributes");

                    b.HasData(
                        new
                        {
                            AttributeID = 1,
                            Name = "Brand",
                            ProductID = 1,
                            Value = "BrandX"
                        },
                        new
                        {
                            AttributeID = 2,
                            Name = "Warranty",
                            ProductID = 1,
                            Value = "2 years"
                        },
                        new
                        {
                            AttributeID = 3,
                            Name = "Brand",
                            ProductID = 2,
                            Value = "BrandY"
                        },
                        new
                        {
                            AttributeID = 4,
                            Name = "Battery Life",
                            ProductID = 2,
                            Value = "12 hours"
                        },
                        new
                        {
                            AttributeID = 5,
                            Name = "Material",
                            ProductID = 3,
                            Value = "Cotton"
                        },
                        new
                        {
                            AttributeID = 6,
                            Name = "Material",
                            ProductID = 4,
                            Value = "Polyester"
                        },
                        new
                        {
                            AttributeID = 7,
                            Name = "Energy Rating",
                            ProductID = 5,
                            Value = "A+"
                        },
                        new
                        {
                            AttributeID = 8,
                            Name = "Load Capacity",
                            ProductID = 6,
                            Value = "8 kg"
                        });
                });

            modelBuilder.Entity("ECommercePlatform.Models.User", b =>
                {
                    b.Property<int>("UserId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("UserId"));

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("FullName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<bool>("IsEmailVerified")
                        .HasColumnType("bit");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("Phone")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ProfilePicture")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Role")
                        .HasColumnType("int");

                    b.HasKey("UserId");

                    b.ToTable("Users");

                    b.HasData(
                        new
                        {
                            UserId = 1,
                            Email = "Admin@admin.com",
                            FullName = "Admin",
                            IsDeleted = false,
                            IsEmailVerified = true,
                            Password = "$2a$11$aZsiddJKDzxfpg2rdgqLZOWyxRoVrZUNsJnYFd3EORCmnpoAPKwlm",
                            Phone = "8732965892",
                            ProfilePicture = "/Images/users/d8dadcfc-e8b8-480a-94b7-ca496880db91.png",
                            Role = 0
                        },
                        new
                        {
                            UserId = 2,
                            Email = "rixitdobariya05@gmail.com",
                            FullName = "Rixit Dobariya",
                            IsDeleted = false,
                            IsEmailVerified = true,
                            Password = "$2a$11$aZsiddJKDzxfpg2rdgqLZOWyxRoVrZUNsJnYFd3EORCmnpoAPKwlm",
                            Phone = "8732965892",
                            ProfilePicture = "/Images/users/d8dadcfc-e8b8-480a-94b7-ca496880db91.png",
                            Role = 1
                        });
                });

            modelBuilder.Entity("ECommercePlatform.Models.UserOTP", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("ExpirationTime")
                        .HasColumnType("datetime2");

                    b.Property<string>("OTP")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("UserOTPs");
                });

            modelBuilder.Entity("ECommercePlatform.Models.WishlistItem", b =>
                {
                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.Property<int>("ProductId")
                        .HasColumnType("int");

                    b.HasKey("UserId", "ProductId");

                    b.HasIndex("ProductId");

                    b.ToTable("WishlistItems");
                });

            modelBuilder.Entity("ECommercePlatform.Models.Address", b =>
                {
                    b.HasOne("ECommercePlatform.Models.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("ECommercePlatform.Models.CartItem", b =>
                {
                    b.HasOne("ECommercePlatform.Models.Product", "Product")
                        .WithMany()
                        .HasForeignKey("ProductId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("ECommercePlatform.Models.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Product");

                    b.Navigation("User");
                });

            modelBuilder.Entity("ECommercePlatform.Models.Category", b =>
                {
                    b.HasOne("ECommercePlatform.Models.Category", "ParentCategory")
                        .WithMany()
                        .HasForeignKey("ParentCategoryId");

                    b.Navigation("ParentCategory");
                });

            modelBuilder.Entity("ECommercePlatform.Models.OrderDetail", b =>
                {
                    b.HasOne("ECommercePlatform.Models.OrderHeader", "OrderHeader")
                        .WithMany("OrderDetails")
                        .HasForeignKey("OrderHeaderId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("ECommercePlatform.Models.Product", "Product")
                        .WithMany()
                        .HasForeignKey("ProductId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("OrderHeader");

                    b.Navigation("Product");
                });

            modelBuilder.Entity("ECommercePlatform.Models.OrderHeader", b =>
                {
                    b.HasOne("ECommercePlatform.Models.Address", "BillingAddress")
                        .WithMany()
                        .HasForeignKey("BillingAddressId")
                        .OnDelete(DeleteBehavior.NoAction);

                    b.HasOne("ECommercePlatform.Models.Address", "ShippingAddress")
                        .WithMany()
                        .HasForeignKey("ShippingAddressId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.HasOne("ECommercePlatform.Models.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("BillingAddress");

                    b.Navigation("ShippingAddress");

                    b.Navigation("User");
                });

            modelBuilder.Entity("ECommercePlatform.Models.Product", b =>
                {
                    b.HasOne("ECommercePlatform.Models.Category", "Category")
                        .WithMany()
                        .HasForeignKey("CategoryId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Category");
                });

            modelBuilder.Entity("ECommercePlatform.Models.ProductAttribute", b =>
                {
                    b.HasOne("ECommercePlatform.Models.Product", "Product")
                        .WithMany()
                        .HasForeignKey("ProductID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Product");
                });

            modelBuilder.Entity("ECommercePlatform.Models.WishlistItem", b =>
                {
                    b.HasOne("ECommercePlatform.Models.Product", "Product")
                        .WithMany()
                        .HasForeignKey("ProductId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("ECommercePlatform.Models.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Product");

                    b.Navigation("User");
                });

            modelBuilder.Entity("ECommercePlatform.Models.OrderHeader", b =>
                {
                    b.Navigation("OrderDetails");
                });
#pragma warning restore 612, 618
        }
    }
}
