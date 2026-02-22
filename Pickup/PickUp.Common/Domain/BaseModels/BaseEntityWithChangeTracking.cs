namespace PickUp.Common.Domain.BaseModels
{
    public abstract class BaseEntityWithChangeTracking : BaseEntity
    {
        public DateTime CreatedOn { get; set; }
        public DateTime UpdatedOn { get; set; }
    }
}
