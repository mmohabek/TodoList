using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TodoList.Domain.Entities;

namespace TodoList.Domain.Common
{
    /// <summary>
    /// نموذج استعلام تصفية المهام
    /// </summary>
    public class TodoItemQuery
    {
        /// <summary>
        /// نص البحث (في العنوان أو الوصف)
        /// </summary>
        /// <example>تقرير</example>
        public string? SearchTerm { get; set; } = null!;

        /// <summary>
        /// حالة الإكمال
        /// </summary>
        /// <example>false</example>
        public bool? IsCompleted { get; set; }

        /// <summary>
        /// مستوى الأولوية
        /// </summary>
        /// <example>High</example>
        public PriorityLevel? Priority { get; set; }

        /// <summary>
        /// الصنف
        /// </summary>
        /// <example>العمل</example>
        public string? Category { get; set; } = null!;

        /// <summary>
        /// من تاريخ 
        /// </summary>
        /// <example>2023-01-01</example>
        public DateTime? DueDateFrom { get; set; }

        /// <summary>
        /// إلى تاريخ
        /// </summary>
        /// <example>2023-12-31</example>
        public DateTime? DueDateTo { get; set; }

    }
}
