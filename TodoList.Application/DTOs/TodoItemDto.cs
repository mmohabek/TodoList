using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TodoList.Domain.Entities;

namespace TodoList.Application.DTOs
{
    /// <summary>
    /// نموذج بيانات المهمة
    /// </summary>
    public class TodoItemDto
    {
        /// <summary>
        /// معرف المهمة
        /// </summary>
        /// <example>1</example>
        public int Id { get; set; }

        /// <summary>
        /// عنوان المهمة
        /// </summary>
        /// <example>إنشاء وثيقة المتطلبات</example>
        public string Title { get; set; }

        /// <summary>
        /// وصف المهمة
        /// </summary>
        /// <example>كتابة القسم الأول من الوثيقة</example>
        public string Description { get; set; }

        /// <summary>
        /// حالة الإكمال
        /// </summary>
        /// <example>false</example>
        public bool IsCompleted { get; set; }

        /// <summary>
        /// تاريخ الاستحقاق
        /// </summary>
        /// <example>2023-12-31</example>
        public DateTime? DueDate { get; set; }

        /// <summary>
        /// مستوى الأولوية
        /// </summary>
        /// <example>High</example>
        public PriorityLevel Priority { get; set; }

        /// <summary>
        /// الفئة
        /// </summary>
        /// <example>العمل</example>
        public string Category { get; set; }
    }
}
