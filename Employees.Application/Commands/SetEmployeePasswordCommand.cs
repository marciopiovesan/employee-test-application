using Employees.Application.Interfaces.CommandQuery;

namespace Employees.Application.Commands
{
    public class SetEmployeePasswordCommand : ICommand<bool>
    {
        public int EmployeeId { get; set; }
        public string NewPassword { get; set; }
    }
}
