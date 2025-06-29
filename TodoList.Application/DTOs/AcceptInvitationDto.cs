using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TodoList.Application.DTOs
{
    /// <summary>
    /// نموذج قبول الدعوة
    /// </summary>
    public class AcceptInvitationDto
    {
        /// <summary>
        /// رمز الدعوة
        /// </summary>
        /// <example>ABCDEF123456</example>
        [Required]
        public string Token { get; set; }


        /// <summary>
        /// اسم المستخدم 
        /// </summary>
        /// <example>newuser</example>
        [Required]
        [StringLength(50, MinimumLength = 3)]
        public string Username { get; set; }


        /// <summary>
        /// كلمة المرور الجديدة
        /// </summary>
        /// <example>NewPassword123!</example>
        [Required]
        [MinLength(6)]
        public string Password { get; set; }
    }

}
