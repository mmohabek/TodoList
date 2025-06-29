using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TodoList.Domain.Common
{
    /// <summary>
    /// نموذج التقسيم إلى صفحات
    /// </summary>
    /// <typeparam name="T">نوع العناصر</typeparam>
    public class PaginationResponse<T>
    {
        /// <summary>
        /// العدد الكلي للعناصر
        /// </summary>
        /// <example>100</example>
        public int TotalCount { get; set; }

        /// <summary>
        /// عدد العناصر في الصفحة
        /// </summary>
        /// <example>10</example>
        public int PageSize { get; set; }

        /// <summary>
        /// رقم الصفحة الحالية
        /// </summary>
        /// <example>1</example>
        public int CurrentPage { get; set; }
        /// <summary>
        /// عدد الصفحات الكلي
        /// </summary>
        /// <example>10</example>
        public int TotalPages => (int)Math.Ceiling(TotalCount / (double)PageSize);

        /// <summary>
        /// قائمة العناصر في الصفحة الحالية
        /// </summary>
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
