using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TodoList.Domain.Entities;

namespace TodoList.Application.DTOs
{
    /// <summary>
    /// نموذج تحديث المهمة
    /// </summary>
    public class UpdateTodoItemDto
    {
        /// <summary>
        /// عنوان المهمة
        /// </summary>
        /// <example>تحديث تقرير المشروع</example>
        [Required(ErrorMessage = "عنوان المهمة مطلوب")]
        [MaxLength(100, ErrorMessage = "يجب ألا يتجاوز العنوان 100 حرف")]
        public string Title { get; set; }

        /// <summary>
        /// وصف المهمة
        /// </summary>
        /// <example>إضافة الملاحظات الجديدة إلى التقرير</example>
        [MaxLength(500, ErrorMessage = "يجب ألا يتجاوز الوصف 500 حرف")]
        public string Description { get; set; }

        /// <summary>
        /// حالة الإكمال
        /// </summary>
        /// <example>false</example>
        public bool IsCompleted { get; set; }

        /// <summary>
        /// تاريخ الاستحقاق
        /// </summary>
        /// <example>2024-01-15</example>
        public DateTime? DueDate { get; set; }

        /// <summary>
        /// مستوى الأولوية
        /// </summary>
        /// <example>Medium</example>
        public PriorityLevel Priority { get; set; }

        /// <summary>
        /// الصنف
        /// </summary>
        /// <example>العمل</example>
        public string Category { get; set; }


    }
}
