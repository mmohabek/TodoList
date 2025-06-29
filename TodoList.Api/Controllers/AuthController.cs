using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Security.Claims;
using System.Text.Json.Serialization;
using TodoList.Application.DTOs;
using TodoList.Application.Interfaces;
using TodoList.Application.Services;
using TodoList.Domain.Entities;

namespace TodoList.Api.Controllers
{
    [ApiController]
    [Route("api/auth")]
    [Authorize(Roles = UserRoles.Owner)]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }


        /// <summary>
        /// تسجيل مستخدم جديد
        /// </summary>
        /// <param name="dto">بيانات التسجيل</param>
        /// <returns>نتيجة التسجيل</returns>
        /// <response code="200">تم التسجيل بنجاح</response>
        /// <response code="400">بيانات غير صالحة</response>

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterDto dto)
        {
            var result = await _authService.Register(dto);
            return Ok(result);
        }


        /// <summary>
        /// تسجيل الدخول للحصول على توكن JWT
        /// </summary>
        /// <remarks>
        /// مثال:
        /// <code>
        /// POST /api/auth/login
        /// {
        ///   "email": "user@example.com",
        ///   "password": "string"
        /// }
        /// </code>
        /// </remarks>
        /// <param name="dto">بيانات تسجيل الدخول</param>
        /// <returns>تفاصيل المستخدم مع التوكن</returns>
        /// <response code="200">تم التسجيل بنجاح</response>
        /// <response code="400">بيانات غير صالحة</response>
        [SwaggerOperation(Tags = new[] { "Authentication" })]
        [HttpPost("login")]
        [ProducesResponseType(typeof(UserResponseDto), 200)]
        [ProducesResponseType(400)]
        [AllowAnonymous]
        public async Task<IActionResult> Login(LoginDto dto)
        {
            var result = await _authService.Login(dto);
            return Ok(result);
        }






        [JsonSerializable(typeof(ApiResult))]
        public record ApiResult(string InvitationToken);



        /// <summary>
        /// دعوة مستخدم جديد عبر البريد الإلكتروني
        /// </summary>
        /// <param name="dto">بيانات الدعوة</param>
        /// <returns>رمز الدعوة</returns>
        /// <response code="200">تم إنشاء رمز الدعوة</response>
        /// <response code="400">بيانات غير صالحة</response>
        /// <response code="401">غير مصرح به</response>
        [ProducesResponseType(typeof(ApiResult), StatusCodes.Status200OK)]
        [SwaggerOperation(Tags = new[] { "Authentication" })]
        [HttpPost("invite")]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        public async Task<IActionResult> InviteUser(InviteUserDto dto)
        {
            var inviterId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var token = await _authService.InviteUser(dto, inviterId);
            return Ok(new ApiResult(token));
        }


        /// <summary>
        /// قبول دعوة مستخدم جديد
        /// </summary>
        /// <param name="dto">بيانات قبول الدعوة</param>
        /// <returns>نتيجة القبول</returns>
        /// <response code="200">تم القبول بنجاح</response>
        /// <response code="400">بيانات غير صالحة أو رمز دعوة غير صالح</response>
        [SwaggerOperation(Tags = new[] { "Authentication" })]
        [HttpPost("accept-invitation")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(UserResponseDto), 200)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> AcceptInvitation(AcceptInvitationDto dto)
        {
            var result = await _authService.AcceptInvitation(dto);
            return Ok(result);
        }


     
    }

}
