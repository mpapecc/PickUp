using PickUp.Common.Domain;
using PickUp.Common.Domain.BaseModels;

namespace PickUp.TripService.Application
{
    public interface IDriverService
    {
        Task<IEnumerable<Guid>> GetAvailableDrivers();
        Task<IBaseResponse> AssignDriver(AssignDriverRequest assignDriverRequest);
    }
}
