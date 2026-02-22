using PickUp.Common.Domain.BaseModels;

namespace PickUp.TripService.Domain
{
    public class Trip : BaseChangeTrackingEntityWithCorrelation
    {
        public Guid RiderId { get; set; }
        public Guid DriverId { get; set; }
        public string PickupLocation { get; set; }
        public string DropoffLocation { get; set; }
        public TripStatus Status { get; set; }
    }

    public enum TripStatus
    {
        Created,
        Accepted,
        Started,
        Completed,
        Cancelled
    }
}
