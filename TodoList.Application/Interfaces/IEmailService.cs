using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TodoList.Application.Interfaces
{
    public interface IEmailService
    {
        Task SendInvitationEmail(string email, string invitationLink);

    }
}
