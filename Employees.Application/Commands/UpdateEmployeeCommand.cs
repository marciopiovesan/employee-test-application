using Employees.Application.Interfaces.CommandQuery;
using Employees.Domain.Entities;

namespace Employees.Application.Commands
{
    public class UpdateEmployeeCommand : ICommand
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string DocumentNumber { get; set; }
        public DateTime DateOfBirth { get; set; }
        public int RoleId { get; set; }
        public List<PhoneNumber> PhoneNumbers { get; set; }
        public int ManagerId { get; set; }
    }
}
