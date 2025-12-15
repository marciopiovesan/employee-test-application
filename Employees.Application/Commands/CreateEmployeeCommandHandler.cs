using Employees.Application.Common;
using Employees.Application.Interfaces;
using Employees.Application.Interfaces.CommandQuery;
using Employees.Domain.Entities;
using FluentValidation;

namespace Employees.Application.Commands
{
    public class CreateEmployeeCommandHandler(IApplicationDbContext dbContext, IValidator<CreateEmployeeCommand> validator) : ICommandHandler<CreateEmployeeCommand, int>
    {
        public async Task<Result<int>> Handle(CreateEmployeeCommand command, CancellationToken cancellationToken)
        {
            var validationResult = await validator.ValidateAsync(command, cancellationToken);
            if (!validationResult.IsValid)
            {
                return ValidationResult<int>.Failure(validationResult.Errors.Select(e => e.ErrorMessage));
            }

            //Validate if Manager exists and has higher role level

            var employee = new Employee
            {
                FirstName = command.FirstName,
                LastName = command.LastName,
                Email = command.Email,
                DateOfBirth = command.DateOfBirth,
                DocumentNumber = command.DocumentNumber,
                RoleId = command.RoleId,
                ManagerId = command.ManagerId,
                PhoneNumbers = command.PhoneNumbers?.Select(pn => new Domain.Entities.PhoneNumber
                {
                    Number = pn.Number,
                    PhoneType = pn.PhoneType
                }).ToList()
            };

            dbContext.Employees.Add(employee);
            await dbContext.SaveChangesAsync(cancellationToken);
            return Result<int>.Success(employee.Id);
        }
    }
}
