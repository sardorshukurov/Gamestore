﻿// <auto-generated />
using System;
using Gamestore.DAL.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace Gamestore.DAL.Migrations
{
    [DbContext(typeof(MainDbContext))]
    partial class MainDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.7")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("Gamestore.Domain.Entities.Ban", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("Duration")
                        .HasColumnType("int");

                    b.Property<DateTime?>("EndDate")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("StartDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("UserName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Bans");
                });

            modelBuilder.Entity("Gamestore.Domain.Entities.Comment", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Body")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid>("GameId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid?>("ParentCommentId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("GameId");

                    b.HasIndex("ParentCommentId");

                    b.ToTable("Comments");
                });

            modelBuilder.Entity("Gamestore.Domain.Entities.Game", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Discount")
                        .HasColumnType("int");

                    b.Property<string>("Key")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("OriginalId")
                        .HasColumnType("int");

                    b.Property<double>("Price")
                        .HasColumnType("float");

                    b.Property<Guid>("PublisherId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("QuantityPerUnit")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("ReorderLevel")
                        .HasColumnType("int");

                    b.Property<DateTime>("PublishingDate")
                        .HasColumnType("datetime2");

                    b.Property<int>("UnitInStock")
                        .HasColumnType("int");

                    b.Property<int>("UnitOnOrder")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("Key")
                        .IsUnique();

                    b.HasIndex("PublisherId");

                    b.ToTable("Games");
                });

            modelBuilder.Entity("Gamestore.Domain.Entities.GameGenre", b =>
                {
                    b.Property<Guid>("GameId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("GenreId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("GameId", "GenreId");

                    b.HasIndex("GenreId");

                    b.ToTable("GamesGenres");
                });

            modelBuilder.Entity("Gamestore.Domain.Entities.GamePlatform", b =>
                {
                    b.Property<Guid>("GameId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("PlatformId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("GameId", "PlatformId");

                    b.HasIndex("PlatformId");

                    b.ToTable("GamesPlatforms");
                });

            modelBuilder.Entity("Gamestore.Domain.Entities.Genre", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<Guid?>("ParentGenreId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<byte[]>("Picture")
                        .HasColumnType("varbinary(max)");

                    b.HasKey("Id");

                    b.HasIndex("Name")
                        .IsUnique();

                    b.ToTable("Genres");
                });

            modelBuilder.Entity("Gamestore.Domain.Entities.Order", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("CustomerId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime?>("Date")
                        .HasColumnType("datetime2");

                    b.Property<int>("EmployeeId")
                        .HasColumnType("int");

                    b.Property<double>("Freight")
                        .HasColumnType("float");

                    b.Property<DateTime>("RequiredDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("ShipAddress")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ShipCity")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ShipCountry")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ShipName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("ShipPostalCode")
                        .HasColumnType("int");

                    b.Property<string>("ShipRegion")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("ShipVia")
                        .HasColumnType("int");

                    b.Property<DateTime>("ShippedVia")
                        .HasColumnType("datetime2");

                    b.Property<int>("Status")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("Order");
                });

            modelBuilder.Entity("Gamestore.Domain.Entities.OrderGame", b =>
                {
                    b.Property<Guid>("OrderId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("ProductId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("Discount")
                        .HasColumnType("int");

                    b.Property<double>("Price")
                        .HasColumnType("float");

                    b.Property<int>("Quantity")
                        .HasColumnType("int");

                    b.HasKey("OrderId", "ProductId");

                    b.HasIndex("ProductId");

                    b.ToTable("OrdersGames");
                });

            modelBuilder.Entity("Gamestore.Domain.Entities.PaymentMethod", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ImageUrl")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("PaymentMethods");
                });

            modelBuilder.Entity("Gamestore.Domain.Entities.Platform", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Type")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("Type")
                        .IsUnique();

                    b.ToTable("Platforms");
                });

            modelBuilder.Entity("Gamestore.Domain.Entities.Publisher", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Address")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("City")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("CompanyName")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ContactName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ContactTitle")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Country")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Fax")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("HomePage")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Phone")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PostalCode")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Region")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("CompanyName")
                        .IsUnique();

                    b.ToTable("Publishers");
                });

            modelBuilder.Entity("Gamestore.Domain.Entities.Comment", b =>
                {
                    b.HasOne("Gamestore.Domain.Entities.Game", null)
                        .WithMany("Comments")
                        .HasForeignKey("GameId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Gamestore.Domain.Entities.Comment", null)
                        .WithMany()
                        .HasForeignKey("ParentCommentId");
                });

            modelBuilder.Entity("Gamestore.Domain.Entities.Game", b =>
                {
                    b.HasOne("Gamestore.Domain.Entities.Publisher", null)
                        .WithMany()
                        .HasForeignKey("PublisherId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Gamestore.Domain.Entities.GameGenre", b =>
                {
                    b.HasOne("Gamestore.Domain.Entities.Game", "Game")
                        .WithMany("GameGenres")
                        .HasForeignKey("GameId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Gamestore.Domain.Entities.Genre", "Genre")
                        .WithMany()
                        .HasForeignKey("GenreId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Game");

                    b.Navigation("Genre");
                });

            modelBuilder.Entity("Gamestore.Domain.Entities.GamePlatform", b =>
                {
                    b.HasOne("Gamestore.Domain.Entities.Game", "Game")
                        .WithMany("GamePlatforms")
                        .HasForeignKey("GameId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Gamestore.Domain.Entities.Platform", "Platform")
                        .WithMany()
                        .HasForeignKey("PlatformId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Game");

                    b.Navigation("Platform");
                });

            modelBuilder.Entity("Gamestore.Domain.Entities.OrderGame", b =>
                {
                    b.HasOne("Gamestore.Domain.Entities.Order", "Order")
                        .WithMany()
                        .HasForeignKey("OrderId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Gamestore.Domain.Entities.Game", "Product")
                        .WithMany("OrderGames")
                        .HasForeignKey("ProductId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Order");

                    b.Navigation("Product");
                });

            modelBuilder.Entity("Gamestore.Domain.Entities.Comment", b =>
                {
                    b.Navigation("Replies");
                });

            modelBuilder.Entity("Gamestore.Domain.Entities.Game", b =>
                {
                    b.Navigation("Comments");

                    b.Navigation("GameGenres");

                    b.Navigation("GamePlatforms");

                    b.Navigation("OrderGames");
                });
#pragma warning restore 612, 618
        }
    }
}
