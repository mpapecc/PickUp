using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PickUp.Common.Application;
using PickUp.Common.Domain;
using PickUp.Common.Domain.BaseModels;
using PickUp.DriverService.Domain;

namespace PickUp.DriverService.Presentation
{
    [ApiController]
    [Route("[controller]")]
    public class DriverController : ControllerBase
    {
        private readonly IBaseRepository<Driver> driverRepository;

        public DriverController(IBaseRepository<Driver> driverRepository)
        {
            this.driverRepository = driverRepository;
        }

        [HttpPost]
        public async Task<IBaseResponse> Create(CreateDriverRequest createDriverRequest)
        {
            var isExistingEmail = await driverRepository.Query().Where(x => x.Email == createDriverRequest.Email).AnyAsync();

            if (isExistingEmail)
                return BaseResponse.CreateFail("Email in use already");

            var newDriver = new Driver()
            {
                Name = createDriverRequest.Name,
                Email = createDriverRequest.Email,
                Status = DriverStatus.Offline
            };

            await driverRepository.AddAsync(newDriver);
            await driverRepository.CommitAsync();

            return BaseResponse<Guid>.CreateSuccess(newDriver.Id);
        }

        [HttpPost(nameof(ToggleAvailability))]
        public async Task<BaseResponse> ToggleAvailability(Guid diverId, DriverStatus newStatus)
        {
            var updatedRecords = await driverRepository.Query()
                .Where(x => x.Id == diverId)
                .ExecuteUpdateAsync(x => x.SetProperty(d => d.Status, newStatus));

            if (updatedRecords == 0)
                return BaseResponse.CreateFail("No driver for update");

            return BaseResponse.CreateSuccess($"Sucessfuly updated status to {newStatus}");
        }

        [HttpPost(nameof(AssignDriverTotrip))]
        public async Task<IBaseResponse> AssignDriverTotrip(AssignDriverRequest assignDriverRequest)
        {
            var updatedRecords = await driverRepository.Query()
                .Where(x => x.Id == assignDriverRequest.DriverId && x.Status == DriverStatus.Available)
                .ExecuteUpdateAsync(x => 
                    x.SetProperty(d => d.Status, DriverStatus.Bussy)
                    .SetProperty(d => d.CurrentTripId, assignDriverRequest.TripId)
                    .SetProperty(d => d.CorrelationId, assignDriverRequest.CorrelationId)
                );

            if (updatedRecords == 0)
                return BaseResponse.CreateFail($"Driver {assignDriverRequest.DriverId} is not available.");

            return BaseResponse.CreateSuccess($"Driver {assignDriverRequest.DriverId} assigned to trip {assignDriverRequest.TripId}.");
        }

        [HttpGet(nameof(GetAvailableDrivers))]
        public async Task<IEnumerable<Guid>> GetAvailableDrivers()
        {
            return await driverRepository.Query()
                .Where(x => x.Status == DriverStatus.Available)
                .Take(5) //dont fetch all drivers
                .Select(x => x.Id)
                .ToListAsync();
        }
    }
}
