using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TodoList.Application.DTOs;
using TodoList.Application.Interfaces;
using TodoList.Application.Services;
using TodoList.Domain.Entities;

namespace TodoList.Api.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterDto dto)
        {
            var result = await _authService.Register(dto);
            return Ok(result);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDto dto)
        {
            var result = await _authService.Login(dto);
            return Ok(result);
        }

        [Authorize(Roles = UserRoles.Owner)]
        [HttpPost("invite")]
        public async Task<IActionResult> InviteUser(InviteUserDto dto)
        {
            var inviterId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var token = await _authService.InviteUser(dto, inviterId);
            return Ok(new { invitationToken = token });
        }

        [HttpPost("accept-invitation")]
        public async Task<IActionResult> AcceptInvitation(AcceptInvitationDto dto)
        {
            var result = await _authService.AcceptInvitation(dto);
            return Ok(result);
        }
    }

}
