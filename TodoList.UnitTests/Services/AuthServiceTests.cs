using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moq;
using FluentAssertions;
using TodoList.Application.Services;
using TodoList.Domain.Entities;
using TodoList.Domain.Interfaces;
using TodoList.Application.DTOs;
using Microsoft.Extensions.Configuration;
using TodoList.Application.Interfaces;
using System.IdentityModel.Tokens.Jwt;

namespace TodoList.UnitTests.Services
{

    public class AuthServiceTests
    {
        private readonly Mock<IUserRepository> _userRepoMock;
        private readonly Mock<IEmailService> _emailServiceMock;
        private readonly AuthService _authService;

        public AuthServiceTests()
        {
            _userRepoMock = new Mock<IUserRepository>();
            _emailServiceMock = new Mock<IEmailService>();

            var inMemorySettings = new Dictionary<string, string> {
                {"Jwt:Key", "YourSuperSecretKeyHereAtLeast32Characters"},
                {"Jwt:Issuer", "TestIssuer"},
                {"Jwt:Audience", "TestAudience"}
            };

            var configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(inMemorySettings)
                .Build();

            _authService = new AuthService(
                _userRepoMock.Object,
                configuration,
                _emailServiceMock.Object
            );
        }

        [Fact]
        public async Task Register_WithNewEmail_ReturnsUserResponse()
        {
            // Arrange
            var testEmail = "test@example.com";
            _userRepoMock.Setup(x => x.GetByEmailAsync(testEmail))
                .ReturnsAsync((User)null);

            // Act
            var result = await _authService.Register(new RegisterDto
            {
                Email = testEmail,
                Password = "P@ssw0rd123",
                Role = "Guest"
            });

            // Assert
            result.Should().NotBeNull();
            result.Email.Should().Be(testEmail);
            _userRepoMock.Verify(x => x.AddAsync(It.IsAny<User>()), Times.Once);
            _emailServiceMock.Verify(x => x.SendInvitationEmail(It.IsAny<string>(), It.IsAny<string>()), Times.Never);
        }

        [Fact]
        public async Task Login_WithValidCredentials_ReturnsToken()
        {
            // Arrange
            var testEmail = "user@example.com";
            var hashedPassword = BCrypt.Net.BCrypt.HashPassword("P@ssw0rd123");

            _userRepoMock.Setup(x => x.GetByEmailAsync(testEmail))
                .ReturnsAsync(new User
                {
                    Email = testEmail,
                    PasswordHash = hashedPassword,
                    Role = "Guest"
                });

            // Act
            var result = await _authService.Login(new LoginDto
            {
                Email = testEmail,
                Password = "P@ssw0rd123"
            });

            // Assert
            result.Should().NotBeNull();
            result.Token.Should().NotBeNullOrEmpty();
        }

        [Fact]
        public async Task Register_WithExistingEmail_ThrowsException()
        {
            // Arrange
            var testEmail = "existing@example.com";
            _userRepoMock.Setup(x => x.GetByEmailAsync(testEmail))
                .ReturnsAsync(new User { Email = testEmail });

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(() => _authService.Register(new RegisterDto
            {
                Email = testEmail,
                Username = "newuser",
                Password = "P@ssw0rd123",
                Role = "Guest"
            }));
        }


        [Fact]
        public async Task Login_WithInvalidCredentials_ThrowsException()
        {
            // Arrange
            var testEmail = "user@example.com";
            var validHash = BCrypt.Net.BCrypt.HashPassword("correctpassword");

            _userRepoMock.Setup(x => x.GetByEmailAsync(testEmail))
                .ReturnsAsync(new User
                {
                    Email = testEmail,
                    PasswordHash = validHash // استخدام هاش صالح
                });

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(() => _authService.Login(new LoginDto
            {
                Email = testEmail,
                Password = "wrongpassword" // كلمة مرور خاطئة
            }));
        }

        [Fact]
        public async Task InviteUser_ByOwner_ReturnsToken()
        {
            // Arrange
            var inviterId = 1;
            var testEmail = "newuser@example.com";
            _userRepoMock.Setup(x => x.GetByIdAsync(inviterId))
                .ReturnsAsync(new User { Id = inviterId, Role = UserRoles.Owner });
            _userRepoMock.Setup(x => x.GetByEmailAsync(testEmail))
                .ReturnsAsync((User)null);
            _userRepoMock.Setup(x => x.AddAsync(It.IsAny<User>()))
                .Returns(Task.CompletedTask);
            _emailServiceMock.Setup(x => x.SendInvitationEmail(It.IsAny<string>(), It.IsAny<string>()))
                .Returns(Task.CompletedTask);

            // Act
            var result = await _authService.InviteUser(new InviteUserDto
            {
                Email = testEmail,
                Role = "Guest"
            }, inviterId);

            // Assert
            result.Should().NotBeNullOrEmpty();
            _userRepoMock.Verify(x => x.AddAsync(It.IsAny<User>()), Times.Once);
            _emailServiceMock.Verify(x => x.SendInvitationEmail(testEmail, It.IsAny<string>()), Times.Once);
        }

        [Fact]
        public async Task InviteUser_ByNonOwner_ThrowsUnauthorizedAccessException()
        {
            // Arrange
            var inviterId = 2;
            _userRepoMock.Setup(x => x.GetByIdAsync(inviterId))
                .ReturnsAsync(new User { Id = inviterId, Role = "Guest" });

            // Act & Assert
            await Assert.ThrowsAsync<UnauthorizedAccessException>(() => _authService.InviteUser(
                new InviteUserDto { Email = "test@example.com", Role = "Guest" },
                inviterId
            ));
        }

        [Fact]
        public async Task AcceptInvitation_WithValidToken_ReturnsUserResponse()
        {
            // Arrange
            var testToken = "valid-token";
            var testUser = new User
            {
                Id = 1,
                Email = "test@example.com",
                Role = "Guest", // تأكد من تعيين Role
                InvitationToken = testToken,
                InvitationExpiry = DateTime.UtcNow.AddDays(1)
            };

            _userRepoMock.Setup(x => x.GetByInvitationToken(testToken))
                .ReturnsAsync(testUser);
            _userRepoMock.Setup(x => x.GetByUsernameAsync(It.IsAny<string>()))
                .ReturnsAsync((User)null);
            _userRepoMock.Setup(x => x.UpdateAsync(It.IsAny<User>()))
                .Returns(Task.CompletedTask);

            // Act
            var result = await _authService.AcceptInvitation(new AcceptInvitationDto
            {
                Token = testToken,
                Username = "newuser",
                Password = "P@ssw0rd123"
            });

            // Assert
            result.Should().NotBeNull();
            result.Username.Should().Be("newuser");
            result.Token.Should().NotBeNullOrEmpty();
            _userRepoMock.Verify(x => x.UpdateAsync(It.IsAny<User>()), Times.Once);
        }

        [Fact]
        public async Task AcceptInvitation_WithExpiredToken_ThrowsException()
        {
            // Arrange
            var testToken = "expired-token";
            var testUser = new User
            {
                Email = "test@example.com",
                InvitationToken = testToken,
                InvitationExpiry = DateTime.UtcNow.AddDays(-1)
            };

            _userRepoMock.Setup(x => x.GetByInvitationToken(testToken))
                .ReturnsAsync(testUser);

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(() => _authService.AcceptInvitation(new AcceptInvitationDto
            {
                Token = testToken,
                Username = "newuser",
                Password = "P@ssw0rd123"
            }));
        }

    }
}



