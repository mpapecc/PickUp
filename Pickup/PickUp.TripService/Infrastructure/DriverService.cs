using PickUp.Common.Domain;
using PickUp.Common.Domain.BaseModels;
using PickUp.TripService.Application;

namespace PickUp.TripService.Infrastructure
{
    public class DriverService : IDriverService
    {
        private readonly HttpClient httpClient;

        public DriverService(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }

        public async Task<IBaseResponse> AssignDriver(AssignDriverRequest assignDriverRequest)
        {
            var result = await httpClient.PostAsJsonAsync("Driver/AssignDriverTotrip", assignDriverRequest);

            var response = await result.Content.ReadFromJsonAsync<BaseResponse>();

            return response;
        }

        public async Task<IEnumerable<Guid>> GetAvailableDrivers()
        {
            var result =  await httpClient.GetFromJsonAsync<ICollection<Guid>>("Driver/GetAvailableDrivers");

            return result ?? Enumerable.Empty<Guid>();
        }
    }
}
