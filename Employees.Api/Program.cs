using Employees.Application.DependencyInjection;
using Employees.Application.Interfaces;
using Employees.Application.Validations;
using Employees.Infrastructure.Database;
using Employees.Infrastructure.Logging;
using Microsoft.EntityFrameworkCore;
using FluentValidation;
using Employees.Api.Middlewares;

var builder = WebApplication.CreateBuilder(args);

builder.Services.ConfigureSerilog(builder.Environment);

builder.Services.AddDbContext<ApplicationDbContext>(
    options =>
        options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"),
        b => b.MigrationsAssembly("Employees.Infrastructure")));

builder.Services.AddScoped<IApplicationDbContext>(sp => sp.GetRequiredService<ApplicationDbContext>());

builder.Services.AddValidatorsFromAssembly(typeof(CreateEmployeeCommandValidator).Assembly);

builder.Services.AddApplicationLayerServices();

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
