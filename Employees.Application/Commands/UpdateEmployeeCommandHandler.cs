using Employees.Application.Common;
using Employees.Application.Interfaces;
using Employees.Application.Interfaces.CommandQuery;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace Employees.Application.Commands
{
    public class UpdateEmployeeCommandHandler(IApplicationDbContext dbContext, IValidator<UpdateEmployeeCommand> validator) : ICommandHandler<UpdateEmployeeCommand>
    {
        public async Task<Result> Handle(UpdateEmployeeCommand command, CancellationToken cancellationToken)
        {
            var validationResult = await validator.ValidateAsync(command, cancellationToken);
            if (!validationResult.IsValid)
            {
                return ValidationResult<int>.Failure(validationResult.Errors.Select(e => e.ErrorMessage));
            }

            var employee = await dbContext.Employees.FirstOrDefaultAsync(e => e.Id == command.Id, cancellationToken);

            if (employee == null)
            {
                return Result.Failure( ErrorTypes.NotFound, "Employee not found");
            }

            employee.FirstName = command.FirstName;
            employee.LastName = command.LastName;
            employee.Email = command.Email;
            employee.DateOfBirth = command.DateOfBirth;
            employee.DocumentNumber = command.DocumentNumber;
            employee.Role.Id = command.RoleId;
            employee.Manager.Id = command.ManagerId;

            foreach (var phone in command.PhoneNumbers)
            {
                var existingPhone = employee.PhoneNumbers.FirstOrDefault(p => p.Id == phone.Id);
                if (existingPhone != null)
                {
                    existingPhone.Number = phone.Number;
                    existingPhone.PhoneType = phone.PhoneType;
                }
                else
                {
                    employee.PhoneNumbers.Add(phone);
                }
            }

            await dbContext.SaveChangesAsync(cancellationToken);
            return Result.Success();
        }
    }
}
