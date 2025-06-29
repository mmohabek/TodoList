using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TodoList.Domain.Entities;
using TodoList.Domain.Interfaces;
using TodoList.Infrastructure.Data;
using TodoList.Domain.Common;


namespace TodoList.Infrastructure.Repositories
{
    public class TodoRepository : ITodoRepository
    {
        private readonly ApplicationDbContext _context;

        public TodoRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<TodoItem> GetByIdAsync(int id)
        {
            return await _context.TodoItems.FindAsync(id);
        }

        public async Task<IEnumerable<TodoItem>> GetAllAsync()
        {
            return await _context.TodoItems.ToListAsync();
        }

        public async Task AddAsync(TodoItem item)
        {
            await _context.TodoItems.AddAsync(item);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(TodoItem item)
        {
            _context.TodoItems.Update(item);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var item = await _context.TodoItems.FindAsync(id);
            if (item != null)
            {
                _context.TodoItems.Remove(item);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<PaginationResponse<TodoItem>> GetPagedAsync(PaginationRequest pagination)
        {
           
            var query = _context.TodoItems.AsQueryable();

            var totalCount = await query.CountAsync();

         
            var items = await query
                .OrderBy(x => x.Id)
                .Skip((pagination.PageNumber - 1) * pagination.PageSize)
                .Take(pagination.PageSize)
                .AsNoTracking() 
                .ToListAsync();

          
            return new PaginationResponse<TodoItem>
            {
                TotalCount = totalCount,
                PageSize = pagination.PageSize,
                CurrentPage = pagination.PageNumber,
                Items = items
            };
        }

        public async Task<PaginationResponse<TodoItem>> GetByPriorityAsync(
           PaginationRequest pagination,
           PriorityLevel priority)
        {
            var query = _context.TodoItems
                .Where(x => x.Priority == priority);

            var totalCount = await query.CountAsync();

            var items = await query
                .OrderBy(x => x.CreatedAt)
                .Skip((pagination.PageNumber - 1) * pagination.PageSize)
                .Take(pagination.PageSize)
                .ToListAsync();

            return new PaginationResponse<TodoItem>(totalCount, pagination.PageSize, pagination.PageNumber, items);
        }


        public async Task<PaginationResponse<TodoItem>> GetByCategoryAsync(
            PaginationRequest pagination,
            string category)
        {
            var query = _context.TodoItems
                .Where(x => x.Category == category);

            var totalCount = await query.CountAsync();

            var items = await query
                .OrderBy(x => x.CreatedAt)
                .Skip((pagination.PageNumber - 1) * pagination.PageSize)
                .Take(pagination.PageSize)
                .ToListAsync();

            return new PaginationResponse<TodoItem>(totalCount, pagination.PageSize, pagination.PageNumber, items);
        }


        public async Task<PaginationResponse<TodoItem>> GetFilteredAsync(
            TodoItemQuery query,
            PaginationRequest pagination)
        {
            var dbQuery = _context.TodoItems.AsQueryable();

            // تطبيق الفلاتر
            if (!string.IsNullOrWhiteSpace(query.SearchTerm))
            {
                dbQuery = dbQuery.Where(x =>
                    x.Title.Contains(query.SearchTerm) ||
                    x.Description.Contains(query.SearchTerm));
            }

            if (query.IsCompleted.HasValue)
            {
                dbQuery = dbQuery.Where(x => x.IsCompleted == query.IsCompleted);
            }

            if (query.Priority.HasValue)
            {
                dbQuery = dbQuery.Where(x => x.Priority == query.Priority);
            }

            if (!string.IsNullOrWhiteSpace(query.Category))
            {
                dbQuery = dbQuery.Where(x => x.Category == query.Category);
            }

            if (query.DueDateFrom.HasValue)
            {
                dbQuery = dbQuery.Where(x => x.DueDate >= query.DueDateFrom);
            }

            if (query.DueDateTo.HasValue)
            {
                dbQuery = dbQuery.Where(x => x.DueDate <= query.DueDateTo);
            }

            var totalCount = await dbQuery.CountAsync();

            var items = await dbQuery
                .OrderByDescending(x => x.CreatedAt)
                .Skip((pagination.PageNumber - 1) * pagination.PageSize)
                .Take(pagination.PageSize)
                .AsNoTracking()
                .ToListAsync();

            return new PaginationResponse<TodoItem>(totalCount, pagination.PageSize, pagination.PageNumber, items);
        }

      
    }

}
