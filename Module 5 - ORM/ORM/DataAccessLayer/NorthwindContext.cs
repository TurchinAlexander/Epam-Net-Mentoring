using DataAccessLayer.Entities;
using Microsoft.EntityFrameworkCore;

namespace DataAccessLayer
{
    public class NorthwindContext : DbContext
    {
        public NorthwindContext(DbContextOptions<NorthwindContext> options)
            : base(options)
        { }


        public DbSet<Category> Categories { get; set; }

        public DbSet<Customer> Customers { get; set; }

        public DbSet<Employee> Employees { get; set; }

        public DbSet<Order> Orders { get; set; }

        public DbSet<OrderDetails> OrderDetails { get; set; }

        public DbSet<Product> Products { get; set; }

        public DbSet<Region> Regions { get; set; }

        public DbSet<Territory> Territories { get; set; }

        public DbSet<EmployeeCard> EmployeeCards { get; set; }
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<OrderDetails>()
                .ToTable("Order Details")
                .HasKey(od => new {od.OrderId, od.ProductId});

            modelBuilder.Entity<Category>().HasData(
                new Category {CategoryId = 1, CategoryName = "Fruits", Description = "It's fruits!'"},
                new Category {CategoryId = 2, CategoryName = "Vegetables", Description = "It's vegetables'!'"});

            modelBuilder.Entity<Region>().HasData(
                new Region() {RegionId = 1, RegionDescription = "New region"},
                new Region() {RegionId = 2, RegionDescription = "Another region"});

            modelBuilder.Entity<Territory>().HasData(
                new Territory() {TerritoryId = 1, RegionId = 1, TerritoryDescription = "New territory"},
                new Territory() {TerritoryId = 2, RegionId = 2, TerritoryDescription = "Another territory"});

            base.OnModelCreating(modelBuilder);
        }
    }
}