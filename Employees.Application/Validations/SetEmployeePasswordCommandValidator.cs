using Employees.Application.Commands;
using FluentValidation;

namespace Employees.Application.Validations
{
    public class SetEmployeePasswordCommandValidator : AbstractValidator<SetEmployeePasswordCommand>
    {
        public SetEmployeePasswordCommandValidator()
        {
            RuleFor(x => DecodeBase64(x.NewPassword))
                .NotEmpty().WithMessage("Password is required.")
                .MinimumLength(8).WithMessage("Password must be at least 8 characters long.")
                .Matches("[A-Z]").WithMessage("Password must contain at least one uppercase letter.")
                .Matches("[a-z]").WithMessage("Password must contain at least one lowercase letter.")
                .Matches("[0-9]").WithMessage("Password must contain at least one digit.")
                .Matches("[^a-zA-Z0-9]").WithMessage("Password must contain at least one special character.");
        }

        private static string DecodeBase64(string encodedString)
        {
            try
            {
                var bytes = Convert.FromBase64String(encodedString);
                return System.Text.Encoding.UTF8.GetString(bytes);
            }
            catch (Exception)
            {
                return string.Empty;
            }
        }
    }
}
