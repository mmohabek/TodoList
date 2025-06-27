using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TodoList.Application.DTOs;
using TodoList.Domain.Common;
using TodoList.Domain.Entities;

namespace TodoList.Application.Interfaces
{
    public interface ITodoService
    {
        Task<IEnumerable<TodoItemDto>> GetAllAsync();
        Task<TodoItemDto> GetByIdAsync(int id);
        Task<TodoItemDto> CreateAsync(CreateTodoItemDto createTodoItemDto);
        Task UpdateAsync(int id, UpdateTodoItemDto todoItemDto);
        Task DeleteAsync(int id);
        Task ToggleCompletionStatusAsync(int id);
        Task<PaginationResponse<TodoItemDto>> GetPagedAsync(PaginationRequest pagination);


        Task<PaginationResponse<TodoItemDto>> GetByPriorityAsync(
            PaginationRequest pagination,
            PriorityLevel priority);

        Task<PaginationResponse<TodoItemDto>> GetByCategoryAsync(
            PaginationRequest pagination,
            string category);

        Task<PaginationResponse<TodoItemDto>> GetFilteredAsync(
          TodoItemQuery query,
          PaginationRequest pagination);

    }

}
