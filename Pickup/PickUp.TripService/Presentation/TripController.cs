using Microsoft.AspNetCore.Mvc;
using PickUp.Common.Application;
using PickUp.Common.Domain;
using PickUp.Common.Domain.BaseModels;
using PickUp.TripService.Application;
using PickUp.TripService.Domain;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace PickUp.TripService.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TripController : ControllerBase
    {
        private readonly IDriverService driverService;
        private readonly IBaseRepository<Trip> tripRepository;

        public TripController(IDriverService driverService, IBaseRepository<Trip> tripRepository)
        {
            this.driverService = driverService;
            this.tripRepository = tripRepository;
        }

        [HttpPost]
        public async Task<IBaseResponse> CreateTrip(CreateTripRequest createTripRequest)
        {
            var availableDrivers = await RetrierService.RetryOnExceptionAsync(async () =>
            {
                var result = await driverService.GetAvailableDrivers( new GetAvailableDriversRequest()
                {
                    Longitude = 43.51299566020603,
                    Latitude = 16.46257497770571
                });

                return result;
            });

            var tripId = Guid.NewGuid();

            //what if all fetched drivers are assigned to other trips before we assign them to this trip?

            foreach (var driverId in availableDrivers)
            {
                var assigned = await driverService.AssignDriver(new AssignDriverRequest() 
                { 
                    DriverId = driverId, 
                    TripId = tripId, 
                    CorrelationId = createTripRequest.CorrelationId
                });

                if (assigned.IsSuccessful)
                {
                    var trip = new Trip()
                    {
                        Id = tripId,
                        DriverId = driverId,
                        RiderId = createTripRequest.RiderId,
                        PickupLocation = createTripRequest.PickupLocation,
                        DropoffLocation = createTripRequest.DropoffLocation,
                        Status = TripStatus.Accepted,
                        CorrelationId = createTripRequest.CorrelationId
                    };

                    await tripRepository.AddAsync(trip);
                    await tripRepository.CommitAsync();

                    return BaseResponse<CreateTripResponse>.CreateSuccess(new CreateTripResponse(tripId, driverId));
                }
            }

            return BaseResponse<CreateTripResponse>.CreateFail("No available drivers");
        }
    }
}
