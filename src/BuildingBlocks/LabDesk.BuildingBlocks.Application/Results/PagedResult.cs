using System;
using System.Collections.Generic;
using System.Text;

namespace LabDesk.SeedWork.Application.Results
{
    public class PagedResult<T>
    {
        public IReadOnlyList<T> Items { get; }
        public int Page { get; }
        public int PageSize { get; }
        public long TotalCount { get; }
        public int TotalPages => (int)Math.Ceiling(TotalCount / (double)PageSize);
        public bool HasNextPage => Page < TotalPages;
        public bool HasPreviousPage => Page > 1;

        public PagedResult(IReadOnlyList<T> items, long totalCount, int page, int pageSize)
        {
            Items = items;
            TotalCount = totalCount;
            Page = page;
            PageSize = pageSize;
        }

        public static PagedResult<T> Create(IReadOnlyList<T> items, long totalCount, int page, int pageSize)
        {
            return new PagedResult<T>(items, totalCount, page, pageSize);
        }
    }
}
