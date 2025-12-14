namespace Employees.Domain.Entities
{
    public class PhoneNumber
    {
        public long Id { get; set; }
        public string Number { get; set; }
        public string PhoneType { get; set; } // Mobile, Home, Work
        public int EmployeeId { get; set; }
        public Employee Employee { get; set; }
    }
}
