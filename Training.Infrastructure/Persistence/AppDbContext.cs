using Microsoft.EntityFrameworkCore;
using Training.Domain.Entities;

namespace Training.Infrastructure.Persistence
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Role>(entity =>
            {
                entity.ToTable("roles");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.RoleName)
                    .HasMaxLength(100)
                    .IsRequired();
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("users");

                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).ValueGeneratedOnAdd();

                entity.Property(e => e.LastName).HasMaxLength(100);
                entity.Property(e => e.FullName).HasMaxLength(200);
                entity.Property(e => e.Address).HasMaxLength(255);
                entity.Property(e => e.Mail).HasMaxLength(100);
                entity.Property(e => e.UserName).HasMaxLength(100);
                entity.Property(e => e.Password).HasMaxLength(255);

                entity.HasOne(e => e.Role)
                    .WithMany(r => r.Users)
                    .HasForeignKey(e => e.RoleId)
                    .OnDelete(DeleteBehavior.Restrict);
            });
        }
    }
}