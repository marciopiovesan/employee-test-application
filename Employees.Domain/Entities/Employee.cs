namespace Employees.Domain.Entities
{
    public class Employee
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string DocumentNumber { get; set; }
        public DateTime DateOfBirth { get; set; }
        public int RoleId { get; set; }
        public EmployeeRole Role { get; set; }
        public List<PhoneNumber>? PhoneNumbers { get; set; }
        public int? ManagerId { get; set; }
        public Employee? Manager { get; set; }
    }
}
