﻿// <auto-generated />
using System;
using ECommercePlatform.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace ECommercePlatform.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20250213185609_ChangeTableStrcutureOfAddress")]
    partial class ChangeTableStrcutureOfAddress
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
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

                    b.ToTable("orderHeaders");
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

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<decimal>("Discount")
                        .HasColumnType("decimal(18,2)");

                    b.Property<string>("ImageUrl")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("IsActive")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<decimal>("SellPrice")
                        .HasColumnType("decimal(18,2)");

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
                            Description = "High-end gaming laptop",
                            Discount = 10.0m,
                            ImageUrl = "th\\Images\\profile.jpg",
                            IsActive = 1,
                            Name = "Laptop",
                            SellPrice = 1500.00m,
                            Stock = 10
                        },
                        new
                        {
                            ProductId = 2,
                            CategoryId = 3,
                            CostPrice = 600.00m,
                            Description = "Latest Android smartphone",
                            Discount = 5.0m,
                            ImageUrl = "th\\Images\\profile.jpg",
                            IsActive = 1,
                            Name = "Smartphone",
                            SellPrice = 800.00m,
                            Stock = 20
                        },
                        new
                        {
                            ProductId = 3,
                            CategoryId = 5,
                            CostPrice = 12.00m,
                            Description = "Comfortable cotton t-shirt",
                            Discount = 0.0m,
                            ImageUrl = "th\\Images\\profile.jpg",
                            IsActive = 1,
                            Name = "Men's T-shirt",
                            SellPrice = 20.00m,
                            Stock = 50
                        },
                        new
                        {
                            ProductId = 4,
                            CategoryId = 6,
                            CostPrice = 25.00m,
                            Description = "Elegant summer dress",
                            Discount = 10.0m,
                            ImageUrl = "th\\Images\\profile.jpg",
                            IsActive = 1,
                            Name = "Women's Dress",
                            SellPrice = 35.00m,
                            Stock = 30
                        },
                        new
                        {
                            ProductId = 5,
                            CategoryId = 8,
                            CostPrice = 950.00m,
                            Description = "Energy-efficient refrigerator",
                            Discount = 15.0m,
                            ImageUrl = "th\\Images\\profile.jpg",
                            IsActive = 1,
                            Name = "Refrigerator",
                            SellPrice = 1200.00m,
                            Stock = 15
                        },
                        new
                        {
                            ProductId = 6,
                            CategoryId = 9,
                            CostPrice = 400.00m,
                            Description = "Front-load washing machine",
                            Discount = 5.0m,
                            ImageUrl = "th\\Images\\profile.jpg",
                            IsActive = 1,
                            Name = "Washing Machine",
                            SellPrice = 500.00m,
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
                        .WithMany()
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
#pragma warning restore 612, 618
        }
    }
}
