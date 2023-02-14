using Microsoft.EntityFrameworkCore;
using UserListBackend.Models.DataModels;

namespace UserListBackend.Data
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>(b =>
            {
                b.HasKey(c => c.Id);
                b.Property(c => c.Gender)
                    .HasConversion<int>()
                    .IsRequired();
            });
        }
    }
}
