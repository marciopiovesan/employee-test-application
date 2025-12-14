using Employees.Application.Common;
using Employees.Application.Interfaces.CommandQuery;
using Employees.Domain.Entities;

namespace Employees.Application.Queries
{
    public class GetEmployeeByIdQuery : IQuery<Result<Employee>>
    {
        public long Id { get; set; }
    }
}
