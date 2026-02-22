namespace PickUp.Common.Domain
{
    public class CreateTripRequest
    {
        public Guid RiderId { get; set; }
        public string PickupLocation { get; set; }
        public string DropoffLocation { get; set; }
        public Guid CorrelationId { get; set; }
    }
}
