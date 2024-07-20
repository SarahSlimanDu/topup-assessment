using Microsoft.AspNetCore.Mvc;
using TopUpBeneficiary.Application.Services.Beneficiaries;

namespace TopUpBeneficiary.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BeneficiariesController(IBeneficiaryService beneficiaryService) : ControllerBase
    {
        private readonly IBeneficiaryService _beneficiaryService = beneficiaryService;

        [HttpGet("{userId}")]
        public async Task<IActionResult> GetBeneficiaries([FromRoute]Guid userId)
        {
           var result =  await  _beneficiaryService.GetBeneficiaries(userId);
          /*  if(result == null)
            {
                return NotFound();
            }*/
            return Ok(result);
        }
    }
}
