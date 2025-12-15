using Employees.Application.Interfaces.CommandQuery;

namespace Employees.Application.Commands
{
    public class LoginCommand : ICommand<string>
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }
}
