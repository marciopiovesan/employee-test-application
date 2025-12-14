using Employees.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Employees.Infrastructure.Database.Configurations
{
    internal class EmployeeConfiguration : IEntityTypeConfiguration<Employee>
    {
        public void Configure(EntityTypeBuilder<Employee> builder)
        {
            builder.HasIndex(e => e.DocumentNumber).IsUnique();

            builder.HasMany(e => e.PhoneNumbers)
                    .WithOne(p => p.Employee)
                    .HasForeignKey(p => p.EmployeeId);

            builder.HasOne(e => e.Role)
                    .WithMany()
                    .HasForeignKey(p => p.RoleId);

            builder.HasOne(e => e.Manager)
                    .WithOne()
                    .HasForeignKey<Employee>(e => e.ManagerId)
                    .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
