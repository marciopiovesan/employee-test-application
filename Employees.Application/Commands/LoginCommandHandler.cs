using Employees.Application.Common;
using Employees.Application.Interfaces;
using Employees.Application.Interfaces.CommandQuery;
using Employees.Application.Interfaces.Security;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace Employees.Application.Commands
{
    public class LoginCommandHandler(
        IApplicationDbContext dbContext, 
        IValidator<LoginCommand> validator, 
        IPasswordHasher passwordHasher,
        ITokenProvider tokenProvider) : ICommandHandler<LoginCommand, string>
    {
        public async Task<Result<string>> Handle(LoginCommand command, CancellationToken cancellationToken)
        {
            var validationResult = validator.Validate(command);
            if (!validationResult.IsValid)
            {
                return ValidationResult<string>.Failure(validationResult.Errors.Select(e => e.ErrorMessage));
            }

            var user = await dbContext.Employees
                .FirstOrDefaultAsync(e => e.Email == command.Username || e.DocumentNumber == command.Username, cancellationToken);

            if(user == null)
            {
                return Result<string>.Failure(ErrorTypes.NotFound, "User not found");
            }

            if(!passwordHasher.Verify(command.Password, user.Password))
            {
                return Result<string>.Failure(ErrorTypes.Unauthorized, "Invalid username or password");
            }

            var token = tokenProvider.GenerateToken(user);
            return Result<string>.Success(token);
        }
    }
}
