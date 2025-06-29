using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel.DataAnnotations;
using TodoList.Application.DTOs;
using TodoList.Application.Interfaces;
using TodoList.Domain.Common;
using TodoList.Domain.Entities;

namespace TodoList.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class TodoController : ControllerBase
    {
        private readonly ITodoService _todoService;

        public TodoController(ITodoService todoService)
        {
            _todoService = todoService;
        }

        /// <summary>
        /// الحصول على جميع المهام
        /// </summary>
        /// <returns>قائمة بالمهام</returns>
        /// <response code="200">تم الحصول على المهام بنجاح</response>
        [SwaggerOperation(Tags = new[] { "Todo Items" })]
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<TodoItemDto>), 200)]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var items = await _todoService.GetAllAsync();
                return Ok(items);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An unexpected error occurred while fetching all items");
            }
        }

        /// <summary>
        /// الحصول على مهمة بواسطة المعرف
        /// </summary>
        /// <param name="id">معرف المهمة</param>
        /// <returns>تفاصيل المهمة</returns>
        /// <response code="200">تم العثور على المهمة</response>
        /// <response code="404">لم يتم العثور على المهمة</response>
        [SwaggerOperation(Tags = new[] { "Todo Items" })]
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(TodoItemDto), 200)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var item = await _todoService.GetByIdAsync(id);
                return Ok(item);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (ValidationException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An unexpected error occurred");
            }
        }
        /// <summary>
        /// إنشاء مهمة جديدة
        /// </summary>
        /// <param name="createTodoItemDto">بيانات المهمة الجديدة</param>
        /// <returns>المهمة التي تم إنشاؤها</returns>
        /// <response code="201">تم إنشاء المهمة بنجاح</response>
        /// <response code="400">بيانات غير صالحة</response>
        /// <response code="401">غير مصرح به</response>
        /// <response code="500">خطأ غير متوقع</response>
        [SwaggerOperation(Tags = new[] { "Todo Items" })]
        [HttpPost]
        [Authorize(Roles = UserRoles.Owner)]
        [ProducesResponseType(typeof(TodoItemDto), 201)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> Create([FromBody] CreateTodoItemDto createTodoItemDto)
        {
            try
            {
                var createdItem = await _todoService.CreateAsync(createTodoItemDto);
                return CreatedAtAction(nameof(GetById), new { id = createdItem.Id }, createdItem);
            }
            catch (ValidationException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An unexpected error occurred while creating the item");
            }
        }

        /// <summary>
        /// تحديث مهمة موجودة
        /// </summary>
        /// <param name="id">معرف المهمة</param>
        /// <param name="updateDto">بيانات التحديث</param>
        /// <returns>لا يوجد محتوى</returns>
        /// <response code="204">تم التحديث بنجاح</response>
        /// <response code="400">بيانات غير صالحة</response>
        /// <response code="401">غير مصرح به</response>
        /// <response code="404">لم يتم العثور على المهمة</response>
        /// <response code="500">خطأ غير متوقع</response>
        [SwaggerOperation(Tags = new[] { "Todo Items" })]
        [HttpPut("{id}")]
        [Authorize(Roles = UserRoles.Owner)]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateTodoItemDto updateDto)
        {
            try
            {
                await _todoService.UpdateAsync(id, updateDto);
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }

        }

        /// <summary>
        /// حذف مهمة
        /// </summary>
        /// <param name="id">معرف المهمة</param>
        /// <returns>لا يوجد محتوى</returns>
        /// <response code="204">تم الحذف بنجاح</response>
        /// <response code="401">غير مصرح به</response>
        /// <response code="404">لم يتم العثور على المهمة</response>
        /// <response code="500">خطأ غير متوقع</response>
        [SwaggerOperation(Tags = new[] { "Todo Items" })]
        [HttpDelete("{id}")]
        [Authorize(Roles = UserRoles.Owner)]
        [ProducesResponseType(204)]
        [ProducesResponseType(401)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _todoService.DeleteAsync(id);
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An unexpected error occurred while deleting the item");
            }
        }


        /// <summary>
        /// تبديل حالة إكمال المهمة
        /// </summary>
        /// <param name="id">معرف المهمة</param>
        /// <returns>لا يوجد محتوى</returns>
        /// <response code="204">تم التبديل بنجاح</response>
        /// <response code="404">لم يتم العثور على المهمة</response>
        /// <response code="500">خطأ غير متوقع</response>
        [SwaggerOperation(Tags = new[] { "Todo Items" })]
        [HttpPatch("{id}/toggle-completion")]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> ToggleCompletion(int id)
        {
            try
            {
                await _todoService.ToggleCompletionStatusAsync(id);
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An unexpected error occurred while toggling completion status");
            }
        }


        /// <summary>
        /// الحصول على المهام مع التقسيم إلى صفحات
        /// </summary>
        /// <param name="pagination">إعدادات التقسيم إلى صفحات</param>
        /// <returns>قائمة بالمهام مع بيانات التقسيم</returns>
        /// <response code="200">تم الحصول على البيانات بنجاح</response>
        [SwaggerOperation(Tags = new[] { "Todo Items" })]
        [HttpGet("paged")]
        [ProducesResponseType(typeof(PaginationResponse<TodoItemDto>), 200)]
        public async Task<IActionResult> GetPaged([FromQuery] PaginationRequest pagination)
        {
            var result = await _todoService.GetPagedAsync(pagination);
            return Ok(result);
        }


        /// <summary>
        /// الحصول على المهام حسب الأولوية مع التقسيم إلى صفحات
        /// </summary>
        /// <param name="priority">مستوى الأولوية</param>
        /// <param name="pagination">إعدادات التقسيم إلى صفحات</param>
        /// <returns>قائمة بالمهام مع بيانات التقسيم</returns>
        /// <response code="200">تم الحصول على البيانات بنجاح</response>
        [SwaggerOperation(Tags = new[] { "Todo Items" })]
        [HttpGet("by-priority/{priority}")]
        [ProducesResponseType(typeof(PaginationResponse<TodoItemDto>), 200)]
        public async Task<IActionResult> GetByPriority(
            [FromRoute] string priority,
            [FromQuery] PaginationRequest pagination)
        {
            if (!Enum.TryParse<PriorityLevel>(priority, true, out var priorityLevel))
            {
                return BadRequest("Invalid priority value. Valid values are: Low, Medium, High, Critical");
            }

            var result = await _todoService.GetByPriorityAsync(pagination, priorityLevel);
            return Ok(result);

        }

        /// <summary>
        /// الحصول على المهام حسب الفئة مع التقسيم إلى صفحات
        /// </summary>
        /// <param name="category">اسم الفئة</param>
        /// <param name="pagination">إعدادات التقسيم إلى صفحات</param>
        /// <returns>قائمة بالمهام مع بيانات التقسيم</returns>
        /// <response code="200">تم الحصول على البيانات بنجاح</response>
        [SwaggerOperation(Tags = new[] { "Todo Items" })]
        [HttpGet("by-category/{category}")]
        [ProducesResponseType(typeof(PaginationResponse<TodoItemDto>), 200)]
        public async Task<IActionResult> GetByCategory(
            [FromRoute] string category,
            [FromQuery] PaginationRequest pagination)
        {
            var result = await _todoService.GetByCategoryAsync(pagination, category);
            return Ok(result);
        }


        /// <summary>
        /// الحصول على المهام مع التصفية والتقسيم إلى صفحات
        /// </summary>
        /// <param name="query">معايير التصفية</param>
        /// <param name="pagination">إعدادات التقسيم إلى صفحات</param>
        /// <returns>قائمة بالمهام مع بيانات التقسيم</returns>
        /// <response code="200">تم الحصول على البيانات بنجاح</response>
        [SwaggerOperation(Tags = new[] { "Todo Items" })]
        [HttpGet("filtered")]
        [ProducesResponseType(typeof(PaginationResponse<TodoItemDto>), 200)]
        public async Task<IActionResult> GetFiltered(
                    [FromQuery] TodoItemQuery query,
                    [FromQuery] PaginationRequest pagination)
        {
            var result = await _todoService.GetFilteredAsync(query, pagination);
            return Ok(result);
        }


    }

}
