namespace PickUp.RiderService.Domain.RideRequestModels
{
    public class CreateRideRequestRequest
    {
        public Guid RiderId { get; set; }
        public string PickupLocation { get; set; }
        public string DropoffLocation { get; set; }
    }
}
