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
    /// نموذج إنشاء مهمة جديدة
    /// </summary>
    public class CreateTodoItemDto
    {
        /// <summary>
        /// عنوان المهمة
        /// </summary>
        /// <example>إكمال تقرير المشروع</example>
        [Required(ErrorMessage = "عنوان المهمة مطلوب")]
        [MaxLength(100, ErrorMessage = "يجب ألا يتجاوز العنوان 100 حرف")]
        public string Title { get; set; }

        /// <summary>
        /// وصف المهمة
        /// </summary>
        /// <example>كتابة القسم الثالث من التقرير وإرفاق الصور</example>
        [MaxLength(500, ErrorMessage = "يجب ألا يتجاوز الوصف 500 حرف")]
        public string Description { get; set; }

        /// <summary>
        /// تاريخ الاستحقاق
        /// </summary>
        /// <example>2023-12-31</example>
        public DateTime? DueDate { get; set; }


        /// <summary>
        /// مستوى الأولوية
        /// </summary>
        /// <example>High</example>
        [Required(ErrorMessage = "مستوى الأولوية مطلوب")]
        public PriorityLevel Priority { get; set; }

        /// <summary>
        /// التصنيف
        /// </summary>
        /// <example>العمل</example>
        [Required(ErrorMessage = "التصنيف مطلوب")]
        [MaxLength(100, ErrorMessage = "يجب ألا يتجاوز التصنيف 100 حرف")]
        public string Category { get; set; }

    }
}
