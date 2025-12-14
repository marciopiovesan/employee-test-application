using Employees.Application.Common;
using Employees.Application.Interfaces;
using Employees.Application.Interfaces.CommandQuery;

namespace Employees.Application.Commands
{
    public class DeleteEmployeeCommandHandler(IApplicationDbContext dbContext) : ICommandHandler<DeleteEmployeeCommand>
    {
        public async Task<Result> Handle(DeleteEmployeeCommand command, CancellationToken cancellationToken)
        {
            var employee = await dbContext.Employees.FindAsync([command.Id], cancellationToken);

            if (employee == null)
            {
                return Result.Failure(ErrorTypes.NotFound, "Employee not found");
            }

            dbContext.Employees.Remove(employee);
            await dbContext.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }
    }
}
