using Employees.Application.Commands;
using Employees.Application.Common;
using Employees.Application.Interfaces.CommandQuery;
using Employees.Application.Queries;
using Employees.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace Employees.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class EmployeesController : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> GetEmployees(
            [FromQuery] GetEmployeesQuery getEmployeesQuery,
            [FromServices] IQueryHandler<GetEmployeesQuery, PaginatedResult<Employee>> paginatedQueryHandler)
        {
            var result = await paginatedQueryHandler.Handle(getEmployeesQuery, CancellationToken.None);

            if (result.IsSuccess)
            {
                return Ok(result);
            }

            return HandleFailure(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetEmployee(int id,
            [FromServices] IQueryHandler<GetEmployeeByIdQuery, Result<Employee>> getByIdQueryHandler)
        {
            var result = await getByIdQueryHandler.Handle(new GetEmployeeByIdQuery { Id = id }, CancellationToken.None);

            if (result.IsSuccess)
            {
                return Ok(result.Value);
            }

            return HandleFailure(result);
        }

        [HttpPost]
        public async Task<IActionResult> CreateEmployee(
            [FromBody] CreateEmployeeCommand employee, 
            [FromServices] ICommandHandler<CreateEmployeeCommand, int> createCommandHandler)
        {
            var result = await createCommandHandler.Handle(employee, CancellationToken.None);
            if (result.IsSuccess)
            {
                return CreatedAtAction(nameof(GetEmployee), new { id = result.Value }, employee);
            }

            return HandleFailure(result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateEmployee(int id, 
            [FromBody] UpdateEmployeeCommand employee, 
            [FromServices] ICommandHandler<UpdateEmployeeCommand> updateCommandHandler)
        {
            employee.Id = id;
            var result = await updateCommandHandler.Handle(employee, CancellationToken.None);

            if (result.IsSuccess)
            {
                return NoContent();
            }

            return HandleFailure(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEmployee(int id, 
            [FromServices] ICommandHandler<DeleteEmployeeCommand> deleteCommandHandler)
        {
            var result = await deleteCommandHandler.Handle(new DeleteEmployeeCommand { Id = id }, CancellationToken.None);
            if (result.IsSuccess)
            {
                return NoContent();
            }

            return HandleFailure(result);
        }

        [HttpPost("{id}/set-password")]
        public async Task<IActionResult> SetPassword(int id, 
            [FromBody] string newPassword, 
            [FromServices] ICommandHandler<SetEmployeePasswordCommand, bool> setPasswordCommandHandler)
        {
            var command = new SetEmployeePasswordCommand
            {
                EmployeeId = id,
                NewPassword = newPassword
            };

            var result = await setPasswordCommandHandler.Handle(command, CancellationToken.None);
            if (result.IsSuccess)
            {
                return NoContent();
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
