using Microsoft.EntityFrameworkCore;

namespace API_YTB.Models
{
    public class UserContext : DbContext
    {

        public UserContext(DbContextOptions<UserContext> options)
            : base(options)
        {
        }

        public DbSet<User> t_user { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().HasKey(u => u.Id_User);
            modelBuilder.Entity<User>().Property(u => u.Username).HasMaxLength(255);
            modelBuilder.Entity<User>().Property(u => u.Password);
        }

    }
}
