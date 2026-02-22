namespace PickUp.Common.Domain.BaseModels
{
    public class BaseEntityWithCorrelation : BaseEntity
    {
        public Guid CorrelationId { get; set; }
    }
}
