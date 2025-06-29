using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TodoList.Domain.Entities
{
    public class User
    {
        public int Id { get; set; }
        public string? Username { get; set; } = "temp_username";
        public string Email { get; set; }
        public string PasswordHash { get; set; } = null!;
        public string Role { get; set; } // "Owner" أو "Guest"
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public string? InvitationToken { get; set; }
        public DateTime? InvitationExpiry { get; set; }

    }



}
