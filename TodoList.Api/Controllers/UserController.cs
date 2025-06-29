using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Security.Claims;
using TodoList.Application.DTOs;
using TodoList.Application.Interfaces;
using TodoList.Domain.Entities;
using TodoList.Domain.Interfaces;

namespace TodoList.Api.Controllers
{
    [ApiController]
    [Route("api/users")]
    [Authorize(Roles = UserRoles.Owner)]
    public class UserController : ControllerBase
    {
        private readonly IUserRepository _userRepo;

        public UserController(IUserRepository userRepo)
        {
            _userRepo = userRepo;
        }

        /// <summary>
        /// الحصول على جميع المستخدمين
        /// </summary>
        /// <returns>قائمة بالمستخدمين</returns>
        /// <response code="200">تم الحصول على المستخدمين بنجاح</response>
        /// <response code="401">غير مصرح به</response>
        [SwaggerOperation(Tags = new[] { "Users" })]
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<User>), 200)]
        [ProducesResponseType(401)]
        public async Task<IActionResult> GetAll()
        {
            var users = await _userRepo.GetAllAsync();
            return Ok(users);
        }

        /// <summary>
        /// حذف مستخدم
        /// </summary>
        /// <param name="id">معرف المستخدم</param>
        /// <returns>لا يوجد محتوى</returns>
        /// <response code="204">تم الحذف بنجاح</response>
        /// <response code="400">لا يمكن حذف حسابك الخاص</response>
        /// <response code="401">غير مصرح به</response>
        /// <response code="404">لم يتم العثور على المستخدم</response>
        [SwaggerOperation(Tags = new[] { "Users" })]
        [HttpDelete("{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> Delete(int id)
        {
            var currentUserId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            if (id == currentUserId)
                return BadRequest("لا يمكن حذف حسابك الخاص");

            await _userRepo.DeleteAsync(id);
            return NoContent();
        }

        /// <summary>
        /// تحديث بيانات مستخدم
        /// </summary>
        /// <param name="id">معرف المستخدم</param>
        /// <param name="dto">بيانات التحديث</param>
        /// <returns>بيانات المستخدم المحدثة</returns>
        /// <response code="200">تم التحديث بنجاح</response>
        /// <response code="401">غير مصرح به</response>
        /// <response code="404">لم يتم العثور على المستخدم</response>
        [SwaggerOperation(Tags = new[] { "Users" })]
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(User), 200)]
        [ProducesResponseType(401)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> Update(int id, UpdateUserDto dto)
        {
            var user = await _userRepo.GetByIdAsync(id);
            if (user == null) return NotFound();

            user.Email = dto.Email;
            user.Role = dto.Role;

            await _userRepo.UpdateAsync(user);
            return Ok(user);
        }
    }


}
