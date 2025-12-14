using Employees.Application.Commands;
using FluentValidation;

namespace Employees.Application.Validations
{
    public class UpdateEmployeeCommandValidator : AbstractValidator<UpdateEmployeeCommand>
    {
        public UpdateEmployeeCommandValidator()
        {
            RuleFor(x => x.FirstName)
                .NotEmpty().WithMessage("Name is required.")
                .MaximumLength(100).WithMessage("Name cannot exceed 100 characters.");
            RuleFor(x => x.LastName)
                .NotEmpty().WithMessage("Last Name is required.")
                .MaximumLength(100).WithMessage("Last Name cannot exceed 100 characters.");
            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email is required.")
                .EmailAddress().WithMessage("A valid email is required.");
            RuleFor(x => x.DocumentNumber)
                .NotEmpty().WithMessage("Document Number is required.")
                .MaximumLength(20).WithMessage("Document Number cannot exceed 20 characters.");
            RuleFor(x => x.DateOfBirth)
                .NotEmpty().WithMessage("Date of Birth is required.")
                .LessThan(DateTime.Now.AddYears(-18)).WithMessage("Can't register employees under the age of 18.");
            RuleFor(x => x.PhoneNumbers)
                .Must(phoneNumbers => phoneNumbers != null && phoneNumbers.Count > 0).WithMessage("At least one phone number is required.");
            RuleFor(x => x.RoleId)
                .NotEmpty().WithMessage("Role is required.");
        }
    }
}
