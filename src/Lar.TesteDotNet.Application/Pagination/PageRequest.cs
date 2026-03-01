namespace Lar.TesteDotNet.Application.Pagination;

public sealed class PageRequest
{
    public PageRequest()
    {
    }

    public PageRequest(int limit, string? cursor = null, IReadOnlyList<OrderField>? orderBy = null)
    {
        Limit = NormalizeLimit(limit);
        Cursor = cursor;
        OrderBy = orderBy;
    }

    public int Limit { get; init; } = 20;
    public string? Cursor { get; init; }
    public IReadOnlyList<OrderField>? OrderBy { get; init; }

    public static int NormalizeLimit(int limit, int defaultLimit = 20, int maxLimit = 200)
    {
        if (limit <= 0) return defaultLimit;
        if (limit > maxLimit) return maxLimit;
        return limit;
    }
}