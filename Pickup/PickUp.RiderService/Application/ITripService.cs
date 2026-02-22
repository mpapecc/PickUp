using PickUp.Common.Domain;
using PickUp.Common.Domain.BaseModels;

namespace PickUp.RiderService.Application
{
    public interface ITripService
    {
        Task<BaseResponse<CreateTripResponse>> CreateTripForRide(CreateTripRequest rideId);
    }
}
