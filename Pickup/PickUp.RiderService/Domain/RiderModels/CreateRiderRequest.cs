using PickUp.Common.Domain.BaseModels;

namespace PickUp.RiderService.Domain.RiderModels
{
    public class CreateRiderRequest : BaseEntityWithChangeTracking
    {
        public string Name { get; set; }
        public string Email { get; set; }
    }
}
