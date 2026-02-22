namespace PickUp.Common.Domain
{
    public class AssignDriverRequest
    {
        public Guid TripId { get; set; }
        public Guid DriverId { get; set; }
        public Guid CorrelationId { get; set; }
    }
}
