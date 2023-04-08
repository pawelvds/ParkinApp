using FluentValidation.Results;

namespace ParkinApp.Domain.Common
{
    public class Result<T>
    {
        public bool IsSuccessful { get; set; }
        public T Value { get; set; }
        public List<string> Errors { get; set; }

        public static Result<T> Success(T value)
        {
            return new Result<T> { IsSuccessful = true, Value = value };
        }

        public static Result<T> Failure(List<string> errors)
        {
            return new Result<T> { IsSuccessful = false, Errors = errors };
        }

        public static Result<T> Failure(string error)
        {
            return new Result<T> { IsSuccessful = false, Errors = new List<string> { error } };
        }

        public static Result<T> Failure(List<ValidationFailure> validationResultErrors)
        {
            return new Result<T>
            {
                IsSuccessful = false,
                Errors = validationResultErrors.Select(e => e.ErrorMessage).ToList()
            };
        }
    }
}
