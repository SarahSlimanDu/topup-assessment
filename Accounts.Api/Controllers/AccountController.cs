using Accounts.Application.Dtos.Request;
using Accounts.Application.Services;
using Commons.Errors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace Accounts.Api.Controllers
{
    [Route("api/account")]
    public class AccountController : ApiController
    {
        private readonly IAccountService _accountService;
        private readonly IConfiguration _config;

        public AccountController(IAccountService accountService, IConfiguration config)
        {
            _accountService = accountService;   
            _config = config;
        }

        [HttpGet("balance/{accountIban}")]
        public async Task<IActionResult> GetBalance([FromRoute] string accountIban, [FromQuery] string key)
        {
            if (!key.Equals(_config.GetSection("ApiKey").Value))
                return Problem(Result.Failure(new Error("Unauthorized","you are not authorized for this action", ErrorTypes.Unauthorized)));

            var result = await _accountService.GetBalance(accountIban);

            return result.IsSuccess ? Ok(result.Value) : Problem(result);
        }
        [HttpPost("debit")]
        public async Task<IActionResult> DebitBalance(DebitBalanceRequest request, [FromQuery] string key)
        {
            if (!key.Equals(_config.GetSection("ApiKey").Value))
                return Problem(Result.Failure(new Error("Unauthorized", "you are not authorized for this action", ErrorTypes.Unauthorized)));

            var result = await _accountService.DebitBalance(request);

            return result.IsSuccess ? NoContent() : Problem(result);
        }
    }
}
