using Employees.Application.Common;
using Employees.Application.Interfaces;
using Employees.Application.Interfaces.CommandQuery;
using Employees.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Employees.Application.Queries
{
    public class GetEmployeesQueryHandler(IApplicationDbContext dbContext) : IQueryHandler<GetEmployeesQuery, PaginatedResult<Employee>>
    {
        public async Task<PaginatedResult<Employee>> Handle(GetEmployeesQuery query, CancellationToken cancellationToken)
        {
            var employees = dbContext.Employees.AsQueryable();

            if (!string.IsNullOrWhiteSpace(query.GenericFilter))
            {
                var genericFilter = query.GenericFilter.Trim().ToLower();

                employees = employees
                    .Where(e => e.FirstName.ToLower().Contains(genericFilter) ||
                            e.LastName.ToLower().Contains(genericFilter) ||
                            e.Email.ToLower().Contains(genericFilter) ||
                            e.DocumentNumber.ToLower().Contains(genericFilter));
            }

            var totalResults = employees.Count();

            var paginatedEmployees = await employees
                .OrderBy(e => e.FirstName)
                .Skip((query.PageNumber - 1) * query.PageSize)
                .Take(query.PageSize)
                .ToListAsync(cancellationToken);

            if(paginatedEmployees == null || paginatedEmployees.Count == 0)
            {
                return PaginatedResult<Employee>.Failure(ErrorTypes.NotFound, "No employees found matching the criteria.");
            }

            var result = PaginatedResult<Employee>.Success(paginatedEmployees, totalResults);
            result.PageNumber = query.PageNumber;
            result.PageSize = query.PageSize;

            return result;
        }
    }
}
