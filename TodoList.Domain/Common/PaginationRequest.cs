using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TodoList.Domain.Common
{
    /// <summary>
    /// نموذج طلب التقسيم إلى صفحات
    /// </summary>
    public class PaginationRequest
    {
        private const int MaxPageSize = 50;
        private int _pageSize = 10;
        /// <summary>
        /// رقم الصفحة المطلوبة (تبدأ من 1)
        /// </summary>
        /// <example>1</example>
        public int PageNumber { get; set; } = 1;


        /// <summary>
        /// عدد العناصر في الصفحة (الحد الأقصى 50)
        /// </summary>
        /// <example>10</example>
        public int PageSize
        {
            get => _pageSize;
            set => _pageSize = (value > MaxPageSize) ? MaxPageSize : value;
        }
    }

}
