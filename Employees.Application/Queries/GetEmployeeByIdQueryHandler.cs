using Employees.Application.Common;
using Employees.Application.Interfaces;
using Employees.Application.Interfaces.CommandQuery;
using Employees.Domain.Entities;

namespace Employees.Application.Queries
{
    public class GetEmployeeByIdQueryHandler(IApplicationDbContext dbContext) : IQueryHandler<GetEmployeeByIdQuery, Result<Employee>>
    {
        public async Task<Result<Employee>> Handle(GetEmployeeByIdQuery query, CancellationToken cancellationToken)
        {
            var employee = await dbContext.Employees.FindAsync([query.Id], cancellationToken);

            if (employee == null)
            {
                return Result<Employee>.Failure( ErrorTypes.NotFound, $"Employee with ID {query.Id} not found.");
            }

            return Result<Employee>.Success(employee);
        }
    }
}
