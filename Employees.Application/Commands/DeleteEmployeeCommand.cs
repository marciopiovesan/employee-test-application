using Employees.Application.Interfaces.CommandQuery;

namespace Employees.Application.Commands
{
    public class DeleteEmployeeCommand : ICommand
    {
        public int Id { get; set; }
    }
}
