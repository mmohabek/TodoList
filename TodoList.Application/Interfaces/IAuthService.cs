using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TodoList.Application.DTOs;
using TodoList.Domain.Entities;

namespace TodoList.Application.Interfaces
{
    public interface IAuthService
    {
        Task<UserResponseDto> Register(RegisterDto dto);
        Task<UserResponseDto> Login(LoginDto dto);
        Task<string> InviteUser(InviteUserDto dto, int inviterId);
        Task<UserResponseDto> AcceptInvitation(AcceptInvitationDto dto);
        string GenerateJwtToken(User user);

    }
}

