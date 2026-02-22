using Microsoft.EntityFrameworkCore;
using PickUp.Common.Infrastructure.Database;
using PickUp.RiderService.Domain.RideRequestModels;
using PickUp.RiderService.Domain.RiderModels;

namespace PickUp.RiderService.Infrastructure.Persistance
{
    public class RiderServiceDbContext : BaseDbContext
    {
        public RiderServiceDbContext(DbContextOptions<RiderServiceDbContext> options) : base(options)
        {
        }

        public override string Schema { get; set; } = "riderservice";

        DbSet<Rider> Riders { get; set; }
        DbSet<RideRequest> RideRequests { get; set; }
    }
}
