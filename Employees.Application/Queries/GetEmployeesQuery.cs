using Employees.Application.Common;
using Employees.Application.Interfaces.CommandQuery;
using Employees.Domain.Entities;

namespace Employees.Application.Queries
{
    public class GetEmployeesQuery : IQuery<PaginatedResult<Employee>>
    {
        public string? GenericFilter { get; set; }
        public int PageSize { get; set; }
        public int PageNumber { get; set; }
    }
}
