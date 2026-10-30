using System.Collections.Generic;

namespace PSite3.Services.Common;

public sealed class PagedResult<T>
{
    public IReadOnlyList<T> Items { get; init; } = [];
    public int TotalCount { get; init; }
}
