using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using TodoList.Application.DTOs;
using TodoList.Application.Interfaces;
using TodoList.Domain.Entities;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.Extensions.Configuration;
using BCrypt.Net;
using TodoList.Domain.Interfaces;


namespace TodoList.Application.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _userRepo;
        private readonly IConfiguration _config;
        private readonly IEmailService _emailService;

        public AuthService(IUserRepository userRepo, IConfiguration config, IEmailService emailService)
        {
            _userRepo = userRepo;
            _config = config;
            _emailService = emailService;

        }

        public async Task<UserResponseDto> Register(RegisterDto dto)
        {
            if (await _userRepo.GetByEmailAsync(dto.Email) != null)
                throw new Exception("Email already exists");

            if (await _userRepo.GetByUsernameAsync(dto.Username) != null)
                throw new Exception("Username already exists");

            var user = new User
            {
                Email = dto.Email,
                Username = dto.Username,
                Role = dto.Role,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password)
            };

            await _userRepo.AddAsync(user);

            return new UserResponseDto
            {
                Id = user.Id,
                Username = user.Username,
                Email = user.Email,
                Role = user.Role,
                Token = GenerateJwtToken(user)
            };
        }


        public async Task<UserResponseDto> Login(LoginDto dto)
        {
            var user = await _userRepo.GetByEmailAsync(dto.Email);
            if (user == null || !BCrypt.Net.BCrypt.Verify(dto.Password, user.PasswordHash))
                throw new Exception("Invalid credentials");

            return new UserResponseDto
            {
                Id = user.Id,
                Email = user.Email,
                Role = user.Role,
                Token = GenerateJwtToken(user)
            };
        }


        public async Task<string> InviteUser(InviteUserDto dto, int inviterId)
        {
            var inviter = await _userRepo.GetByIdAsync(inviterId);
            if (inviter?.Role != UserRoles.Owner)
                throw new UnauthorizedAccessException("Only owners can invite users");

            if (await _userRepo.GetByEmailAsync(dto.Email) != null)
                throw new Exception("Email already registered");

            var token = Guid.NewGuid().ToString();
            var expiry = DateTime.UtcNow.AddDays(7);

            var user = new User
            {
                Email = dto.Email,
                Role = dto.Role,
                InvitationToken = token,
                InvitationExpiry = expiry
            };

            await _userRepo.AddAsync(user);

            // Send invitation email
            var invitationLink = $"{_config["AppBaseUrl"]}/accept-invitation?token={token}";
            await _emailService.SendInvitationEmail(dto.Email, invitationLink);

            return token;
        }

        public async Task<UserResponseDto> AcceptInvitation(AcceptInvitationDto dto)
        {
            var user = await _userRepo.GetByInvitationToken(dto.Token);
            if (user == null || user.InvitationExpiry < DateTime.UtcNow)
                throw new Exception("Invalid or expired invitation token");

            user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password);
            user.InvitationToken = null;
            user.InvitationExpiry = null;

            await _userRepo.UpdateAsync(user);

            return new UserResponseDto
            {
                Id = user.Id,
                Email = user.Email,
                Role = user.Role,
                Token = GenerateJwtToken(user)
            };
        }


        public string GenerateJwtToken(User user)
        {
            var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Email, user.Email),
            new Claim(ClaimTypes.Role, user.Role)
        };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _config["Jwt:Issuer"],
                audience: _config["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddDays(1),
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
