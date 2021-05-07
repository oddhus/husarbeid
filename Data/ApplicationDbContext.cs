using Microsoft.EntityFrameworkCore;

namespace husarbeid.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<User> Users { get; set; } = default!;
        public DbSet<Family> Families { get; set; } = default!;
        public DbSet<FamilyTask> FamilyTasks { get; set; } = default!;
    }
}