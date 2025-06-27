using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TodoList.Domain.Common;
using TodoList.Domain.Entities;

namespace TodoList.Domain.Interfaces
{
    public interface ITodoRepository
    {
        Task<TodoItem> GetByIdAsync(int id);
        Task<IEnumerable<TodoItem>> GetAllAsync();
        Task AddAsync(TodoItem item);
        Task UpdateAsync(TodoItem item);
        Task DeleteAsync(int id);

        Task<PaginationResponse<TodoItem>> GetPagedAsync(PaginationRequest pagination);

        Task<PaginationResponse<TodoItem>> GetByPriorityAsync(
            PaginationRequest pagination,
            PriorityLevel priority);

        Task<PaginationResponse<TodoItem>> GetByCategoryAsync(
            PaginationRequest pagination,
            string category);


        Task<PaginationResponse<TodoItem>> GetFilteredAsync(
            TodoItemQuery query,
            PaginationRequest pagination);

    }

}
