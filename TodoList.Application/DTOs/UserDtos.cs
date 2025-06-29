using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TodoList.Application.DTOs
{
  
    public class RegisterDto
    {
        [Required(ErrorMessage = "البريد الإلكتروني مطلوب")]
        [EmailAddress(ErrorMessage = "بريد إلكتروني غير صالح")]
        [Description("البريد الإلكتروني للمستخدم")]
        public string Email { get; set; }


        [Required(ErrorMessage = "كلمة المرور مطلوبة")]
        [MinLength(6, ErrorMessage = "يجب أن تكون كلمة المرور على الأقل 6 أحرف")]
        [Description("كلمة مرور المستخدم")]
        public string Password { get; set; }

        [Required]
        public string Role { get; set; } // "Owner" أو "Guest"

        [Required]
        public string Username { get; set; }
    }

    /// <summary>
    /// نموذج تسجيل الدخول
    /// </summary>
    public class LoginDto
    {
        /// <summary>
        /// البريد الإلكتروني للمستخدم
        /// </summary>
        /// <example>user@example.com</example>
        [Required, EmailAddress]
        public string Email { get; set; }

        /// <summary>
        /// كلمة المرور
        /// </summary>
        /// <example>Password123!</example>
        [Required]
        public string Password { get; set; }
    }

    /// <summary>
    /// نموذج استجابة  المستخدم
    /// </summary>
    public class UserResponseDto
    {
        /// <summary>
        /// معرف المستخدم
        /// </summary>
        /// <example>1</example>
        public int Id { get; set; }
        /// <summary>
        /// اسم المستخدم
        /// </summary>
        /// <example>user1</example>
        public string Username { get; set; }
        /// <summary>
        /// البريد الإلكتروني
        /// </summary>
        /// <example>user@example.com</example>
        public string Email { get; set; }
        /// <summary>
        /// دور المستخدم (Owner أو Guest)
        /// </summary>
        /// <example>Owner</example>
        public string Role { get; set; }

        /// <summary>
        /// توكن JWT للمصادقة
        /// </summary>
        /// <example>eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...</example>
        public string Token { get; set; }
    }

    /// <summary>
    /// نموذج تحديث بيانات المستخدم
    /// </summary>
    public class UpdateUserDto
    {
        /// <summary>
        /// البريد الإلكتروني الجديد
        /// </summary>
        /// <example>new.email@example.com</example>
        [EmailAddress(ErrorMessage = "بريد إلكتروني غير صالح")]
        public string Email { get; set; }

        /// <summary>
        /// الدور الجديد (Owner أو Guest)
        /// </summary>
        /// <example>Guest</example>
        [Required(ErrorMessage = "الدور مطلوب")]
        public string Role { get; set; }
    }


}
