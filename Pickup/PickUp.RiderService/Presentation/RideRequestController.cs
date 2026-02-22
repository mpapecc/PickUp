using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PickUp.Common.Application;
using PickUp.Common.Domain;
using PickUp.Common.Domain.BaseModels;
using PickUp.RiderService.Application;
using PickUp.RiderService.Domain.RideRequestModels;

namespace PickUp.RiderService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RideRequestController : ControllerBase
    {
        private readonly IBaseRepository<RideRequest> rideRequestRepository;
        private readonly ITripService tripService;

        public RideRequestController(
            IBaseRepository<RideRequest> rideRequestRepository,
            ITripService tripService)
        {
            this.rideRequestRepository = rideRequestRepository;
            this.tripService = tripService;
        }

        [HttpPost]
        //[Idempotent]
        [ProducesResponseType<IBaseResponse>(StatusCodes.Status201Created)]
        public async Task<IBaseResponse> CreateRideRequest(CreateRideRequestRequest createRideRequestRequest)
        {
            var newRideRequest = new RideRequest()
            {
                RiderId = createRideRequestRequest.RiderId,
                PickupLocation = createRideRequestRequest.PickupLocation,
                DropoffLocation = createRideRequestRequest.DropoffLocation,
                Status = RideRequestStatus.Requested
            };

            await rideRequestRepository.AddAsync(newRideRequest);
            await rideRequestRepository.CommitAsync();

            var newCreatTripRequest = new CreateTripRequest()
            {
                RiderId = createRideRequestRequest.RiderId,
                PickupLocation = createRideRequestRequest.PickupLocation,
                DropoffLocation = createRideRequestRequest.DropoffLocation,
                CorrelationId = Guid.NewGuid()
            };

            //call trip service
            var tripResposne = await tripService.CreateTripForRide(newCreatTripRequest);

            return tripResposne.IsSuccessful ? BaseResponse<CreateTripResponse>.CreateSuccess(tripResposne.Data, "TripId") : BaseResponse<Guid>.CreateFail(tripResposne.Message);
        }

        [HttpPost(nameof(Cancel))]
        public async Task<BaseResponse> Cancel(Guid rideRequestId)
        {
            var updatedRecords = await rideRequestRepository.Query()
                .Where(x => x.Id == rideRequestId && x.Status != RideRequestStatus.Cancelled)
                .ExecuteUpdateAsync(x => x.SetProperty( rr => rr.Status, RideRequestStatus.Cancelled));

            if(updatedRecords == 0)
                return BaseResponse.CreateFail("No ride request to be canceled.");

            return BaseResponse.CreateSuccess("Canceled successfully.");
        }

        [HttpPost(nameof(Status))]
        public async Task<BaseResponse<RideRequestStatus>> Status(Guid rideRequestId)
        {
            var status = await rideRequestRepository.Query()
                .Where(x => x.Id == rideRequestId)
                .Select(x => x.Status)
                .FirstOrDefaultAsync();

            return BaseResponse<RideRequestStatus>.CreateSuccess(status);   
        }
    }
}
