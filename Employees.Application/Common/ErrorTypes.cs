namespace Employees.Application.Common
{
    public enum ErrorTypes
    {
        None = 0,
        Validation = 400,
        NotFound = 404,
        Conflict = 409,
        Unauthorized = 401,
        Forbidden = 403,
        Internal = 500
    }
}
