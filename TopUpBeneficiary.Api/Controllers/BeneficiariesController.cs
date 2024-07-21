using Microsoft.AspNetCore.Mvc;
using TopUpBeneficiary.Application.Dtos.Request;
using TopUpBeneficiary.Application.Services.Beneficiaries;

namespace TopUpBeneficiary.Api.Controllers
{
    [Route("api/beneficiary")]
    public class BeneficiariesController(IBeneficiaryService beneficiaryService) : ApiController
    {
        private readonly IBeneficiaryService _beneficiaryService = beneficiaryService;

        [HttpGet("{userId}")]
        public async Task<IActionResult> GetBeneficiaries([FromRoute]Guid userId)
        {
           var result =  await  _beneficiaryService.GetBeneficiaries(userId);

            return result.IsSuccess ? Ok(result.Value) : Problem(result);
        }

        [HttpPost("add")]
        public async Task<IActionResult> AddBeneficiary(AddBeneficiaryDto request)
        {
            var result = await _beneficiaryService.AddBeneficiary(request);

            return result.IsSuccess ? NoContent() : Problem(result);
        }
    }
}
