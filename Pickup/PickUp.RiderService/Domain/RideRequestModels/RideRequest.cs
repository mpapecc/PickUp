using PickUp.Common.Domain.BaseModels;
using PickUp.RiderService.Domain.RiderModels;

namespace PickUp.RiderService.Domain.RideRequestModels
{
    public class RideRequest : BaseEntityWithCorrelation
    {
        public Rider Rider { get; set; }
        public Guid RiderId { get; set; }
        public string PickupLocation { get; set; }
        public string DropoffLocation { get; set; }
        public RideRequestStatus Status { get; set; }
    }

    public enum RideRequestStatus
    {
        Requested,
        Assigned,
        Cancelled
    }   
}
