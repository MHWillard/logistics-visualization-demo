using logistics_visualization_demo.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;

namespace logistics_visualization_demo.Data
{
    public class RecordContext : DbContext
    {
        public RecordContext(DbContextOptions<RecordContext> options)
            : base(options)
        {
        }

        public DbSet<Company> Companies { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<OrderDetail> OrderDetails { get; set; }
        public DbSet<MonthlyOrderStat> MonthlyOrderStats { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Company>().ToTable("Company");
            modelBuilder.Entity<Order>().ToTable("Order");
            modelBuilder.Entity<Product>().ToTable("Product")
                .Property(p => p.Price)
                .HasPrecision(18, 2);
            modelBuilder.Entity<OrderDetail>().ToTable("OrderDetail");
            modelBuilder.Entity<MonthlyOrderStat>().ToView("MonthlyOrderStats").HasNoKey()
                .Property(m => m.TotalIncome).HasPrecision(18, 2);
        }
    }
}
