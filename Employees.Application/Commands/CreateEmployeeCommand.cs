using Employees.Application.Interfaces.CommandQuery;

namespace Employees.Application.Commands
{
    public class CreateEmployeeCommand : ICommand<int>
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string DocumentNumber { get; set; }
        public DateTime DateOfBirth { get; set; }
        public int RoleId { get; set; }
        public int RoleLevel { get; set; }
        public List<PhoneNumber>? PhoneNumbers { get; set; }
        public int? ManagerId { get; set; }
    }

    public class PhoneNumber
    {
        public int Id { get; set; }
        public string Number { get; set; }
        public string PhoneType { get; set; }
    }
}
