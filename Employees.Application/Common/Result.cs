using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Employees.Application.Common
{
    public class Result
    {
        public bool IsSuccess { get; }
        public string? ErrorMessage { get; }
        public ErrorTypes ErrorType { get; }

        protected Result(bool isSuccess, string? errorMessage, ErrorTypes errorType)
        {
            IsSuccess = isSuccess;
            ErrorMessage = errorMessage;
            ErrorType = errorType;
        }

        public static Result Success() => new (true, null, ErrorTypes.None);
        public static Result Failure(ErrorTypes errorType, string errorMessage) => new (false, errorMessage, errorType);
    }

    public class Result<T> : Result
    {
        public T? Value { get; }
        protected Result(bool isSuccess, T? value, string? errorMessage, ErrorTypes errorType)
            : base(isSuccess, errorMessage, errorType)
        {
            Value = value;
        }
     
        public static Result<T> Success(T value) => new (true, value, null, ErrorTypes.None);
        public static new Result<T> Failure(ErrorTypes errorType, string errorMessage) => new (false, default, errorMessage, errorType);

        public static implicit operator Result<T>(T? value) =>
            value is not null ? Success(value) : Failure(ErrorTypes.NotFound, "Null result");
    }

    public class PaginatedResult<T> : Result<IEnumerable<T>>
    {
        public int TotalCount { get; }
        public int PageSize { get; set; }
        public int PageNumber { get; set; }
        public int ActualPageCount { get { return Value == null ? 0 : Value.Count(); } }
        public int TotalPages
        {
            get
            {
                return (int)Math.Ceiling((double)TotalCount / PageSize);
            }
        }

        private PaginatedResult(bool isSuccess, IEnumerable<T>? value, int totalCount, string? errorMessage, ErrorTypes errorType)
            : base(isSuccess, value, errorMessage, errorType)
        {
            TotalCount = totalCount;
        }

        public static PaginatedResult<T> Success(IEnumerable<T> value, int totalCount) =>
            new (true, value, totalCount, null, ErrorTypes.None);

        public static new PaginatedResult<T> Failure(ErrorTypes errorType, string errorMessage) =>
            new (false, null, 0, errorMessage, errorType);
    }

    public class ValidationResult<T> : Result<T>
    {
        public IEnumerable<string> ValidationErrors { get; }
        private ValidationResult(bool isSuccess, T? value, IEnumerable<string> validationErrors, string? errorMessage)
            : base(isSuccess, value, errorMessage, ErrorTypes.Validation)
        {
            ValidationErrors = validationErrors;
        }
        public static new ValidationResult<T> Failure(IEnumerable<string> validationErrors) =>
            new (false, default, validationErrors, "Validation failed");
    }
}
