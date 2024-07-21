using Accounts.Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace Accounts.Api.Controllers
{
    [Route("api/account")]
    public class AccountController : ApiController
    {
        private readonly IAccountService _accountService;

        public AccountController(IAccountService accountService)
        {
            _accountService = accountService;   
        }

        [HttpGet("balance/{accountId}")]
        public async Task<IActionResult> GetBalance([FromRoute] Guid accountId)
        {
            var result = await _accountService.GetBalance(accountId);

            return result.IsSuccess ? Ok(result.Value) : Problem(result);
        }
    }
}
