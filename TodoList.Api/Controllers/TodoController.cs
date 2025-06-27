using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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

        [HttpGet]
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

        [HttpGet("{id}")]
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

        [HttpPost]
        [Authorize(Roles = UserRoles.Owner)]
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

        [HttpPut("{id}")]
        [Authorize(Roles = UserRoles.Owner)]
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

        [HttpDelete("{id}")]
        [Authorize(Roles = UserRoles.Owner)]
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

        [HttpPatch("{id}/toggle-completion")]
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

        [HttpGet("paged")]
        public async Task<IActionResult> GetPaged([FromQuery] PaginationRequest pagination)
        {
            var result = await _todoService.GetPagedAsync(pagination);
            return Ok(result);
        }


        [HttpGet("by-priority/{priority}")]
        public async Task<IActionResult> GetByPriority(
            [FromRoute] PriorityLevel priority,
            [FromQuery] PaginationRequest pagination)
        {
            var result = await _todoService.GetByPriorityAsync(pagination, priority);
            return Ok(result);
        }

        [HttpGet("by-category/{category}")]
        public async Task<IActionResult> GetByCategory(
            [FromRoute] string category,
            [FromQuery] PaginationRequest pagination)
        {
            var result = await _todoService.GetByCategoryAsync(pagination, category);
            return Ok(result);
        }


        [HttpGet("filtered")]
        public async Task<IActionResult> GetFiltered(
            [FromQuery] TodoItemQuery query,
            [FromQuery] PaginationRequest pagination)
        {
            var result = await _todoService.GetFilteredAsync(query, pagination);
            return Ok(result);
        }


    }

}
