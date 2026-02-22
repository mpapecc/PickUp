using Microsoft.EntityFrameworkCore;
using PickUp.Common.Infrastructure.Database;
using PickUp.DriverService.Domain;

namespace PickUp.DriverService.Infrastructure.Persistance
{
    public class DriverServiceDbContext : BaseDbContext
    {
        public DriverServiceDbContext(DbContextOptions<DriverServiceDbContext> options) : base(options)
        {
        }

        public override string Schema { get; set; } = "driverservice";

        DbSet<Driver> Drivers { get; set; }
    }
}
