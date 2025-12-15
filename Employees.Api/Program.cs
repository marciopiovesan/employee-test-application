using Employees.Api.Middlewares;
using Employees.Application.DependencyInjection;
using Employees.Application.Interfaces;
using Employees.Application.Validations;
using Employees.Domain.Entities;
using Employees.Infrastructure.Database;
using Employees.Infrastructure.DependencyInjection;
using Employees.Infrastructure.Logging;
using FluentValidation;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.ConfigureSerilog(builder.Environment);

builder.Services.AddDbContext<ApplicationDbContext>(
    options =>
        options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"),
        b => b.MigrationsAssembly("Employees.Infrastructure")));

builder.Services.AddScoped<IApplicationDbContext>(sp => sp.GetRequiredService<ApplicationDbContext>());

builder.Services.AddValidatorsFromAssembly(typeof(CreateEmployeeCommandValidator).Assembly);

builder.Services.AddApplicationLayerServices();
builder.Services.AddInfrastructureLayerServices();

builder.Services.AddScoped<RequestLoggingMiddleware>();

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();

    using IServiceScope scope = app.Services.CreateScope();

    using ApplicationDbContext dbContext =
        scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

    dbContext.Database.Migrate();
}

app.UseHttpsRedirection();

app.UseMiddleware<RequestLoggingMiddleware>();

app.UseAuthorization();

app.MapControllers();

app.Run();
