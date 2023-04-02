namespace ParkinApp.Domain.Common.Errors;

public class Error
{
    public int StatusCode { get; set; }
    public string Message { get; set; } = string.Empty;
    public string ExceptionType { get; set; } = string.Empty;
}