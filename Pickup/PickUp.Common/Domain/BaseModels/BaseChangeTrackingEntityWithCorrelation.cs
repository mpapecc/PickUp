namespace PickUp.Common.Domain.BaseModels
{
    public class BaseChangeTrackingEntityWithCorrelation : BaseEntityWithChangeTracking
    {
        public Guid CorrelationId { get; set; }
    }
}
