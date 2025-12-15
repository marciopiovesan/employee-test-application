using Employees.Application.Common;
using Employees.Application.Interfaces;
using Employees.Application.Interfaces.CommandQuery;
using Employees.Application.Interfaces.Security;
using FluentValidation;

namespace Employees.Application.Commands
{
    public class SetEmployeePasswordCommandHandler(IApplicationDbContext dbContext, 
        IValidator<SetEmployeePasswordCommand> validator, 
        IPasswordHasher passwordHasher) 
        : ICommandHandler<SetEmployeePasswordCommand, bool>
    {
        public async Task<Result<bool>> Handle(SetEmployeePasswordCommand command, CancellationToken cancellationToken)
        {
            var validationResult = await validator.ValidateAsync(command, cancellationToken);
            if (!validationResult.IsValid)
            {
                return ValidationResult<bool>.Failure(validationResult.Errors.Select(e => e.ErrorMessage));
            }

            var employee = await dbContext.Employees.FindAsync([command.EmployeeId], cancellationToken);
            if (employee == null)
            {
                return Result<bool>.Failure(ErrorTypes.NotFound, $"Employee with ID {command.EmployeeId} not found.");
            }
            
            var newPasswordHash = passwordHasher.Hash(command.NewPassword);
            
            if(!string.IsNullOrEmpty(employee.Password))
            {
                var isSamePassword = passwordHasher.Verify(command.NewPassword, employee.Password);
                if (isSamePassword)
                {
                    return Result<bool>.Failure(ErrorTypes.Validation, "The new password cannot be the same as the current password.");
                }
            }

            employee.Password = newPasswordHash;
            await dbContext.SaveChangesAsync(cancellationToken);
            return Result<bool>.Success(true);

        }
    }
}
