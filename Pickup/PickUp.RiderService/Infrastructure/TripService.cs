using System.Text.Json;
using PickUp.Common.Domain;
using PickUp.Common.Domain.BaseModels;
using PickUp.RiderService.Application;

namespace PickUp.RiderService.Infrastructure
{
    public class TripService : ITripService
    {
        private readonly HttpClient httpClient;
        private readonly ILogger logger;

        public TripService(HttpClient httpClient)
        {
            this.httpClient = httpClient;
            this.logger = LoggerFactory.Create(builder => builder.AddSimpleConsole()).CreateLogger("test");
        }

        public async Task<BaseResponse<CreateTripResponse>> CreateTripForRide(CreateTripRequest createTripRequest)
        {
            logger.LogInformation("Creating trip for rider {RiderId} with pickup location {PickupLocation} and dropoff location {DropoffLocation}",
                createTripRequest.RiderId, createTripRequest.PickupLocation, createTripRequest.DropoffLocation);


            var createTrip = await httpClient.PostAsJsonAsync("Trip", createTripRequest);

            var response = await createTrip.Content.ReadFromJsonAsync<BaseResponse<CreateTripResponse>>();

            return response;
        }
    }
}
