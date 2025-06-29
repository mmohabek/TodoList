using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TodoList.Application.DTOs
{
    /// <summary>
    /// نموذج دعوة مستخدم جديد
    /// </summary>
    public class InviteUserDto
    {
        /// <summary>
        /// البريد الإلكتروني للمدعو
        /// </summary>
        /// <example>new.user@example.com</example>
        [Required(ErrorMessage = "البريد الإلكتروني مطلوب")]
        [EmailAddress(ErrorMessage = "بريد إلكتروني غير صالح")]
        public string Email { get; set; }

        /// <summary>
        /// دور المستخدم (Owner أو Guest)
        /// </summary>
        /// <example>Guest</example>
        [Required(ErrorMessage = "الدور مطلوب")]
        public string Role { get; set; }
    }

}
