using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TodoList.Application.Interfaces;

namespace TodoList.Application.Services
{
    public class EmailService : IEmailService
    {
        private readonly IConfiguration _config;

        public EmailService(IConfiguration config)
        {
            _config = config;
        }

        public async Task SendInvitationEmail(string email, string invitationLink)
        {
            var subject = "You've been invited to TodoList App";
            var body = $"Click this link to accept your invitation: {invitationLink}";

            //send Email here

            await Task.CompletedTask;
        }
    }

}
