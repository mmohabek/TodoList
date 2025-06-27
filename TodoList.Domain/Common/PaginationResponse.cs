using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TodoList.Domain.Common
{
    public class PaginationResponse<T>
    {
        public int TotalCount { get; set; }
        public int PageSize { get; set; }
        public int CurrentPage { get; set; }
        public int TotalPages => (int)Math.Ceiling(TotalCount / (double)PageSize);
        public List<T> Items { get; set; } = new();



        public PaginationResponse() { }

        public PaginationResponse(int totalCount, int pageSize, int currentPage, List<T> items)
        {
            TotalCount = totalCount;
            PageSize = pageSize;
            CurrentPage = currentPage;
            Items = items;
        }


    }
}
