using Microsoft.EntityFrameworkCore;
namespace API_YTB.Models
{
    public class ProductContext : DbContext
    {
        public ProductContext(DbContextOptions<ProductContext> options)
            : base(options)
        {
        }

        public DbSet<Product> t_product { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Product>().HasKey(p => p.Id_Product);
            modelBuilder.Entity<Product>().Property(p => p.Product_Name).HasMaxLength(100);
            modelBuilder.Entity<Product>().Property(p => p.Product_Price).HasColumnType("decimal(18,0)");
        }

    }

}
