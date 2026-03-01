using Lar.TesteDotNet.Domain.Errors;

namespace Lar.TesteDotNet.Domain.Common;

public sealed class DomainResult<T>
{
    private DomainResult(T value)
    {
        IsSuccess = true;
        Value = value;
        Errors = new List<Error>();
    }

    private DomainResult(IEnumerable<Error> errors)
    {
        IsSuccess = false;
        Errors = errors.ToList();
    }

    public bool IsSuccess { get; }
    public T Value { get; } = default!;
    public IReadOnlyList<Error> Errors { get; }

    public static DomainResult<T> Ok(T value)
    {
        return new DomainResult<T>(value);
    }

    public static DomainResult<T> Fail(IEnumerable<Error> errors)
    {
        return new DomainResult<T>(errors);
    }

    public static DomainResult<T> From(Func<T> factory, List<Error> errors)
    {
        if (errors.Any()) return Fail(errors);
        return Ok(factory());
    }
}

public sealed class DomainResult
{
    private DomainResult(IEnumerable<Error> errors)
    {
        IsSuccess = false;
        Errors = errors.ToList();
    }

    private DomainResult()
    {
        IsSuccess = true;
        Errors = new List<Error>();
    }

    public bool IsSuccess { get; }

    public IReadOnlyList<Error> Errors { get; }

    public static DomainResult Ok()
    {
        return new DomainResult();
    }

    public static DomainResult Fail(IEnumerable<Error> errors)
    {
        return new DomainResult(errors);
    }

    public static DomainResult From(Action act, List<Error> errors)
    {
        if (errors.Any()) return Fail(errors);
        act.Invoke();
        return Ok();
    }
}