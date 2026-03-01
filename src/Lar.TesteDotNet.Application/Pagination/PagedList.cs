namespace Lar.TesteDotNet.Application.Pagination;

public sealed class PagedList<T>
{
    public PagedList()
    {
    }

    private PagedList(List<T> items, string? cursor, bool hasMore)
    {
        Items = items;
        Cursor = cursor;
        HasMore = hasMore;
    }

    public List<T> Items { get; set; } = new();

    public string? Cursor { get; set; }

    public bool HasMore { get; set; }

    public static PagedList<T> Create(
        List<T> items,
        string? cursor,
        bool hasMore)
    {
        return new PagedList<T>(items, cursor, hasMore);
    }
}