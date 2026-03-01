using Lar.TesteDotNet.Domain.Errors;

namespace Lar.TesteDotNet.Shared.Wrappers;

public class RequestResult
{
    protected RequestResult(bool isSuccess, IEnumerable<Error> errors)
    {
        IsSuccess = isSuccess;
        Errors = errors.ToList();
    }

    public bool IsSuccess { get; }
    public bool IsFailure => !IsSuccess;
    public Error? Error => Errors.FirstOrDefault();
    public IReadOnlyList<Error> Errors { get; }

    public static RequestResult Success()
    {
        return new RequestResult(true, Array.Empty<Error>());
    }

    public static RequestResult Failure(Error error)
    {
        return new RequestResult(false, new[] { error });
        // compatível
    }

    public static RequestResult Failure(IEnumerable<Error> errors)
    {
        return new RequestResult(false, errors);
        // novo
    }
}

public sealed class RequestResult<T> : RequestResult
{
    private RequestResult(bool isSuccess, IEnumerable<Error> errors, T? value)
        : base(isSuccess, errors)
    {
        Value = value;
    }

    public T? Value { get; }

    public static RequestResult<T> Success(T value)
    {
        return new RequestResult<T>(true, Array.Empty<Error>(), value);
    }

    public new static RequestResult<T> Failure(Error error)
    {
        return new RequestResult<T>(false, new[] { error }, default);
    }

    public new static RequestResult<T> Failure(IEnumerable<Error> errors)
    {
        return new RequestResult<T>(false, errors, default);
    }
}