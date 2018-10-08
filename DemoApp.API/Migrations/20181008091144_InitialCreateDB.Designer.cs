﻿// <auto-generated />
using System;
using DemoApp.API.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace DemoApp.API.Migrations
{
    [DbContext(typeof(MyDbContext))]
    [Migration("20181008091144_InitialCreateDB")]
    partial class InitialCreateDB
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.1.4-rtm-31024");

            modelBuilder.Entity("DemoApp.API.Data.Product", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Description");

                    b.Property<string>("ImageUrl");

                    b.Property<bool>("IsHotProduct");

                    b.Property<decimal>("Price");

                    b.Property<string>("ProductCateCode");

                    b.Property<string>("ProductName");

                    b.HasKey("Id");

                    b.HasIndex("ProductCateCode");

                    b.ToTable("Product");
                });

            modelBuilder.Entity("DemoApp.API.Data.ProductCategory", b =>
                {
                    b.Property<string>("CateCode")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Description");

                    b.HasKey("CateCode");

                    b.ToTable("ProductCategory");
                });

            modelBuilder.Entity("DemoApp.API.Data.Role", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Description");

                    b.Property<string>("Name");

                    b.HasKey("Id");

                    b.ToTable("Role");
                });

            modelBuilder.Entity("DemoApp.API.Data.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Email");

                    b.Property<string>("FirstName");

                    b.Property<string>("LastName");

                    b.Property<byte[]>("PasswordHash");

                    b.Property<byte[]>("PasswordSalt");

                    b.Property<int>("RoleId");

                    b.Property<string>("Username");

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("User");
                });

            modelBuilder.Entity("DemoApp.API.Data.Product", b =>
                {
                    b.HasOne("DemoApp.API.Data.ProductCategory", "ProductCategory")
                        .WithMany("Products")
                        .HasForeignKey("ProductCateCode");
                });

            modelBuilder.Entity("DemoApp.API.Data.User", b =>
                {
                    b.HasOne("DemoApp.API.Data.Role", "Role")
                        .WithMany("Users")
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}
