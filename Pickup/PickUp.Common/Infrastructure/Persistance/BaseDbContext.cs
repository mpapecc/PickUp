using Microsoft.EntityFrameworkCore;
using PickUp.Common.Infrastructure.Persistance;

namespace PickUp.Common.Infrastructure.Database
{
    public abstract class BaseDbContext : DbContext
    {
        public abstract string Schema { get; set; }
        protected BaseDbContext(DbContextOptions options) : base(options)
        {
            
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.AddInterceptors(new ChangeTrackingEntityInterceptor());
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema(Schema);
        }
    }
}
