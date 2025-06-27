using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TodoList.Application.DTOs
{
    public class AcceptInvitationDto
    {
        public string Token { get; set; }
        [MinLength(6)]
        public string Password { get; set; }
    }

}
