using Employees.Application.Commands;
using Employees.Application.Common;
using Employees.Application.Interfaces.CommandQuery;
using Microsoft.AspNetCore.Mvc;

namespace Employees.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class LoginController : ControllerBase
    {
        [HttpPost]
        public async Task<IActionResult> Login(
            [FromBody] LoginCommand loginCommand,
            [FromServices] ICommandHandler<LoginCommand, string> loginCommandHandler)
        {
            var result = await loginCommandHandler.Handle(loginCommand, CancellationToken.None);
            if (result.IsSuccess)
            {
                return Ok(new { Token = result.Value });
            }
            return HandleFailure(result);
        }

        private ObjectResult HandleFailure(Result result)
        {
            return result.ErrorType switch
            {
                ErrorTypes.NotFound => NotFound(result),
                ErrorTypes.Validation => BadRequest(result),
                ErrorTypes.Conflict => Conflict(result),
                _ => StatusCode(500, result),
            };
        }
    }
}
