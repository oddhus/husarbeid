using Microsoft.EntityFrameworkCore;

namespace husarbeid.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<FamilyTask>().HasOne(u => u.CreatedBy).WithMany(u => u.UserCreatedTasks);
            modelBuilder.Entity<FamilyTask>().HasOne(u => u.AssignedTo).WithMany(u => u.UserTasks);
        }

        public DbSet<User> Users { get; set; } = default!;
        public DbSet<Family> Families { get; set; } = default!;
        public DbSet<FamilyTask> FamilyTasks { get; set; } = default!;
    }
}