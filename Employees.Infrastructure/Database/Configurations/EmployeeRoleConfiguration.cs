using Employees.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Employees.Infrastructure.Database.Configurations
{
    internal class EmployeeRoleConfiguration : IEntityTypeConfiguration<EmployeeRole>
    {
        public void Configure(EntityTypeBuilder<EmployeeRole> builder)
        {
            builder.HasData(
                new EmployeeRole { Id = 1, Description = "Employee", Level = 1 },
                new EmployeeRole { Id = 2, Description = "Leader", Level = 2 },
                new EmployeeRole { Id = 3, Description = "Director", Level = 3 }
            );
        }
    }
}
