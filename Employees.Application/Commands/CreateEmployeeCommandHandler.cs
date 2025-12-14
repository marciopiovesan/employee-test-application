using Employees.Application.Common;
using Employees.Application.Interfaces;
using Employees.Application.Interfaces.CommandQuery;
using Employees.Domain.Entities;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace Employees.Application.Commands
{
    public class CreateEmployeeCommandHandler(IApplicationDbContext dbContext, IValidator<CreateEmployeeCommand> validator) : ICommandHandler<CreateEmployeeCommand, long>
    {
        public async Task<Result<long>> Handle(CreateEmployeeCommand command, CancellationToken cancellationToken)
        {
            var validationResult = await validator.ValidateAsync(command, cancellationToken);
            if (!validationResult.IsValid)
            {
                return ValidationResult<long>.Failure(validationResult.Errors.Select(e => e.ErrorMessage));
            }

            var employee = new Employee
            {
                FirstName = command.FirstName,
                LastName = command.LastName,
                Email = command.Email,
                DateOfBirth = command.DateOfBirth,
                DocumentNumber = command.DocumentNumber,
                Role = await dbContext.EmployeeRoles.FirstOrDefaultAsync(r => r.Id == command.RoleId, cancellationToken) 
                       ?? throw new Exception("Role not found"),
                Manager = command.ManagerId.HasValue 
                          ? await dbContext.Employees.FirstOrDefaultAsync(e => e.Id == command.ManagerId.Value, cancellationToken) 
                            ?? throw new Exception("Manager not found") 
                          : null,
                PhoneNumbers = command.PhoneNumbers
            };

            dbContext.Employees.Add(employee);
            await dbContext.SaveChangesAsync(cancellationToken);
            return Result<long>.Success(employee.Id);
        }
    }
}
