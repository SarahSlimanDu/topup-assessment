using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TopUpBeneficiary.Application.Dtos.Request;
using TopUpBeneficiary.Application.Services.TopUp;

namespace TopUpBeneficiary.Api.Controllers
{
    [Route("api/topup")]
    public class TopUpController : ApiController
    {
        private readonly ITopUpService _topUpService;
        public TopUpController(ITopUpService topUpService)
        {
                _topUpService = topUpService;   
        }

        [HttpPost]
        public async Task<IActionResult> TopUp(TopUpDto request)
        {
           var result =  await _topUpService.TopUpBeneficiary(request);
            return result.IsSuccess ? NoContent() : Problem(result);
        }

        [HttpGet("options")]
        public async Task<IActionResult> GetTopUpOptions()
        {
            var result = await _topUpService.GetTopUpOptions();
            return result.IsSuccess ? Ok(result.Value) : Problem(result);
        }
    }
}
