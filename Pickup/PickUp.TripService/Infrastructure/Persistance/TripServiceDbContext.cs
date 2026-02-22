using Microsoft.EntityFrameworkCore;
using PickUp.Common.Infrastructure.Database;
using PickUp.TripService.Domain;

namespace PickUp.TripService.Infrastructure.Persistance
{
    public class TripServiceDbContext : BaseDbContext
    {
        public TripServiceDbContext(DbContextOptions<TripServiceDbContext> options) : base(options)
        {
        }

        public override string Schema { get; set; } = "tripservice";

        DbSet<Trip> Trips { get; set; }
    }
}
