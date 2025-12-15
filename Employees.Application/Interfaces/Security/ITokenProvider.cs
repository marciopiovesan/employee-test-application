using Employees.Domain.Entities;

namespace Employees.Application.Interfaces.Security
{
    public interface ITokenProvider
    {
        string GenerateToken(Employee user);
    }
}
