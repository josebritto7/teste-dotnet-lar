namespace Lar.TesteDotNet.Domain.Errors;

public sealed record Error(
    string Code,
    string Message,
    int? Status = null)
{
    public static Error NotFound(string code, string message)
    {
        return new Error(code, message, 404);
    }

    public static Error Validation(string code, string message)
    {
        return new Error(code, message, 400);
    }

    public static Error Conflict(string code, string message)
    {
        return new Error(code, message, 409);
    }

    public static Error Problem(string code, string message, int? status = null)
    {
        return new Error(code, message, status);
    }
}