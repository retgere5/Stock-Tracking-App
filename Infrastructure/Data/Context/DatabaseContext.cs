using System;
using Microsoft.EntityFrameworkCore;
using DesktopApp.Core.Entities;

namespace DesktopApp.Infrastructure.Data.Context
{
    public sealed class DatabaseContext : DbContext
    {
        private const string ConnectionString = "Data Source=inventory.db";

        public DbSet<Product> Products => Set<Product>();
        public DbSet<StockMovement> StockMovements => Set<StockMovement>();

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (optionsBuilder == null)
            {
                throw new ArgumentNullException(nameof(optionsBuilder));
            }

            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlite(ConnectionString);
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            if (modelBuilder == null)
            {
                throw new ArgumentNullException(nameof(modelBuilder));
            }

            base.OnModelCreating(modelBuilder);

            // Configure Product entity
            modelBuilder.Entity<Product>(entity =>
            {
                entity.ToTable("Products");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Name).IsRequired().HasMaxLength(100);
                entity.Property(e => e.Description).HasMaxLength(500);
                entity.Property(e => e.Price).HasColumnType("decimal(18,2)");
                entity.Property(e => e.CurrentStock).IsRequired();
                
                entity.HasMany(p => p.StockMovements)
                    .WithOne(sm => sm.Product)
                    .HasForeignKey(sm => sm.ProductId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasIndex(e => e.Name);
            });

            // Configure StockMovement entity
            modelBuilder.Entity<StockMovement>(entity =>
            {
                entity.ToTable("StockMovements");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Type)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasConversion<string>();
                entity.Property(e => e.Quantity).IsRequired();
                entity.Property(e => e.Reference).HasMaxLength(100);
                entity.Property(e => e.Notes).HasMaxLength(500);
                entity.Property(e => e.CreatedBy).IsRequired().HasMaxLength(50);
                entity.Property(e => e.CreatedAt).IsRequired();

                entity.HasIndex(e => e.CreatedAt);
                entity.HasIndex(e => e.Type);
            });

            // Seed initial data
            modelBuilder.Entity<Product>().HasData(
                new Product
                {
                    Id = 1,
                    Name = "Sample Product",
                    Description = "This is a sample product",
                    Price = 29.99m,
                    CurrentStock = 100
                }
            );
        }
    }
} 