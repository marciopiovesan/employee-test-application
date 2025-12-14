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
    public class EmployeesController(
        ICommandHandler<CreateEmployeeCommand, int> createCommandHandler,
        ICommandHandler<UpdateEmployeeCommand> updateCommandHandler,
        ICommandHandler<DeleteEmployeeCommand> deleteCommandHandler,
        IQueryHandler<GetEmployeeByIdQuery, Result<Employee>> getByIdQueryHandler,
        IQueryHandler<GetEmployeesQuery, PaginatedResult<Employee>> paginatedQueryHandler) : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> GetEmployees([FromQuery] GetEmployeesQuery getEmployeesQuery)
        {
            var result = await paginatedQueryHandler.Handle(getEmployeesQuery, CancellationToken.None);

            if (result.IsSuccess)
            {
                return Ok(result);
            }

            return HandleFailure(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetEmployee(int id)
        {
            var result = await getByIdQueryHandler.Handle(new GetEmployeeByIdQuery { Id = id }, CancellationToken.None);

            if (result.IsSuccess)
            {
                return Ok(result.Value);
            }

            return HandleFailure(result);
        }

        [HttpPost]
        public async Task<IActionResult> CreateEmployee([FromBody] CreateEmployeeCommand employee)
        {
            var result = await createCommandHandler.Handle(employee, CancellationToken.None);
            if (result.IsSuccess)
            {
                return CreatedAtAction(nameof(GetEmployee), new { id = result.Value }, employee);
            }

            return HandleFailure(result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateEmployee(int id, [FromBody] UpdateEmployeeCommand employee)
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
        public async Task<IActionResult> DeleteEmployee(int id)
        {
            var result = await deleteCommandHandler.Handle(new DeleteEmployeeCommand { Id = id }, CancellationToken.None);
            if (result.IsSuccess)
            {
                return NoContent();
            }

            return HandleFailure(result);
        }

        private IActionResult HandleFailure(Result result)
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
