using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PickUp.Common.Application;
using PickUp.Common.Domain.BaseModels;
using PickUp.RiderService.Domain.RiderModels;

namespace PickUp.RiderService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RiderController : ControllerBase
    {
        private readonly IBaseRepository<Rider> riderRepository;

        public RiderController(IBaseRepository<Rider> riderRepository)
        {
            this.riderRepository = riderRepository;
        }

        [HttpPost]
        public async Task<BaseResponse> CreateAccount(CreateRiderRequest createRiderRequest)
        {
            var isExistingEmail = await riderRepository.Query().Where(x => x.Email == createRiderRequest.Email).AnyAsync();

            if (isExistingEmail)
            {
                return BaseResponse.CreateFail("Email already in use.");
            }

            var newRider = new Rider() { Email = createRiderRequest.Email , Name = createRiderRequest.Name};

            await riderRepository.AddAsync(newRider);

            await riderRepository.CommitAsync();

            return BaseResponse.CreateSuccess($"Welcome {createRiderRequest.Name}");
        }
    }
}
