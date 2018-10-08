using Microsoft.EntityFrameworkCore;

namespace DemoApp.API.Data
{
    public class MyDbContext : DbContext
    {
        public MyDbContext(DbContextOptions<MyDbContext> options) : base(options)
        { }

        public DbSet<User> User { get; set; }
        public DbSet<Role> Role { get; set; }
        public DbSet<Product> Product { get; set; }
        public DbSet<ProductCategory> ProductCategory { get; set; }

        protected override void OnModelCreating (ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Product>().Property(s => s.Id).ValueGeneratedOnAdd();
            modelBuilder.Entity<Role>().Property(s => s.Id).ValueGeneratedOnAdd();
            modelBuilder.Entity<User>().Property(s => s.Id).ValueGeneratedOnAdd();
            modelBuilder.Entity<Product>()
                .HasOne<ProductCategory>(s => s.ProductCategory)
                .WithMany(g => g.Products)
                .HasForeignKey(s => s.ProductCateCode);
            modelBuilder.Entity<User>()
                .HasOne<Role>(s => s.Role)
                .WithMany(g => g.Users)
                .HasForeignKey(s => s.RoleId);
        }
    }
}