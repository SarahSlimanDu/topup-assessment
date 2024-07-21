using Commons.Errors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Accounts.Api.Controllers
{
    [ApiController]
    public class ApiController : ControllerBase
    {
        protected IActionResult Problem(Result result)
        {
            if (result.IsSuccess)
            {
                throw new InvalidOperationException("Can't convert success result to problem");
            }

            var statusCode = result.Error.type switch
            {

                ErrorTypes.Conflict => StatusCodes.Status409Conflict,
                ErrorTypes.Validation => StatusCodes.Status400BadRequest,
                ErrorTypes.NotFound => StatusCodes.Status404NotFound,
                _ => StatusCodes.Status500InternalServerError,
            };

            //TODO: check this response more
            return Problem(
                statusCode: statusCode,
                title: result.Error.Code,
                type: "https://tools.ietf.org/html/rfc7231#section-6.5.1"
                );
        }
    }
}
