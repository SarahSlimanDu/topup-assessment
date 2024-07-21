using Microsoft.AspNetCore.Mvc;
using TopUpBeneficiary.Application.Services.Beneficiaries;

namespace TopUpBeneficiary.Api.Controllers
{
    [Route("api/[controller]")]
    public class BeneficiariesController(IBeneficiaryService beneficiaryService) : ApiController
    {
        private readonly IBeneficiaryService _beneficiaryService = beneficiaryService;

        [HttpGet("{userId}")]
        public async Task<IActionResult> GetBeneficiaries([FromRoute]Guid userId)
        {
           var result =  await  _beneficiaryService.GetBeneficiaries(userId);

            return result.IsSuccess ? Ok(result.Value) : Problem(result);
        }
    }
}
