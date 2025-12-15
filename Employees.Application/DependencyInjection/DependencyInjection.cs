using Employees.Application.Commands;
using Employees.Application.Common;
using Employees.Application.Interfaces.CommandQuery;
using Employees.Application.Queries;
using Employees.Domain.Entities;
using Microsoft.Extensions.DependencyInjection;

namespace Employees.Application.DependencyInjection
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplicationLayerServices(this IServiceCollection services)
        {
            services.AddScoped<ICommandHandler<CreateEmployeeCommand, int>, CreateEmployeeCommandHandler>();
            services.AddScoped<ICommandHandler<UpdateEmployeeCommand>, UpdateEmployeeCommandHandler>();
            services.AddScoped<ICommandHandler<DeleteEmployeeCommand>, DeleteEmployeeCommandHandler>();
            services.AddScoped<IQueryHandler<GetEmployeeByIdQuery, Result<Employee>>, GetEmployeeByIdQueryHandler>();
            services.AddScoped<IQueryHandler<GetEmployeesQuery, PaginatedResult<Employee>>, GetEmployeesQueryHandler>();
            services.AddScoped<ICommandHandler<SetEmployeePasswordCommand, bool>, SetEmployeePasswordCommandHandler>();
            return services;
        }
    }
}
