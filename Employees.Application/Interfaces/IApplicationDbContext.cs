using Employees.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Employees.Application.Interfaces
{
    public interface IApplicationDbContext
    {
        DbSet<Employee> Employees { get; }
        DbSet<EmployeeRole> EmployeeRoles { get; }
        DbSet<PhoneNumber> PhoneNumbers { get; }

        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}
