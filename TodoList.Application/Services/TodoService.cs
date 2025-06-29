using AutoMapper;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TodoList.Application.DTOs;
using TodoList.Application.Interfaces;
using TodoList.Domain.Common;
using TodoList.Domain.Entities;
using TodoList.Domain.Interfaces;

namespace TodoList.Application.Services
{
    public class TodoService : ITodoService
    {
        private readonly ITodoRepository _todoRepository;
        private readonly IMapper _mapper;
        private readonly IValidator<CreateTodoItemDto> _validator;

        public TodoService(
        ITodoRepository todoRepository,
        IMapper mapper,
        IValidator<CreateTodoItemDto> validator)
        {
            _todoRepository = todoRepository;
            _mapper = mapper;
            _validator = validator; 
        }



        public async Task<IEnumerable<TodoItemDto>> GetAllAsync()
        {
            var items = await _todoRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<TodoItemDto>>(items);
        }


        public async Task<TodoItemDto> CreateAsync(CreateTodoItemDto createTodoItemDto)
        {
            var validationResult = await _validator.ValidateAsync(createTodoItemDto);
            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors);
            }

            var todoItem = _mapper.Map<TodoItem>(createTodoItemDto);
            await _todoRepository.AddAsync(todoItem);

            return _mapper.Map<TodoItemDto>(todoItem);
        }

        public async Task<TodoItemDto> GetByIdAsync(int id)
        {

            var item = await _todoRepository.GetByIdAsync(id);
            if (item == null)
            {
                throw new KeyNotFoundException($"Todo item with id {id} not found");
            }
            return _mapper.Map<TodoItemDto>(item);
        }

        public async Task UpdateAsync(int id, UpdateTodoItemDto updateDto)
        {
            var existingItem = await _todoRepository.GetByIdAsync(id);
            if (existingItem == null)
            {
                throw new KeyNotFoundException($"Todo item with id {id} not found");
            }

            _mapper.Map(updateDto, existingItem);

            await _todoRepository.UpdateAsync(existingItem);
        }

        public async Task DeleteAsync(int id)
        {
            var item = await _todoRepository.GetByIdAsync(id);
            if (item == null)
            {
                throw new KeyNotFoundException($"Todo item with id {id} not found");
            }

            await _todoRepository.DeleteAsync(id);
        }

        public async Task ToggleCompletionStatusAsync(int id)
        {
            var item = await _todoRepository.GetByIdAsync(id);
            if (item == null)
            {
                throw new KeyNotFoundException($"Todo item with id {id} not found");
            }

            item.IsCompleted = !item.IsCompleted;
            item.CompletedDate = item.IsCompleted ? DateTime.UtcNow : null;

            await _todoRepository.UpdateAsync(item);
        }

        public async Task<PaginationResponse<TodoItemDto>> GetPagedAsync(PaginationRequest pagination)
        {
            var pagedResult = await _todoRepository.GetPagedAsync(pagination);

            return new PaginationResponse<TodoItemDto>
            {
                TotalCount = pagedResult.TotalCount,
                PageSize = pagedResult.PageSize,
                CurrentPage = pagedResult.CurrentPage,
                Items = _mapper.Map<List<TodoItemDto>>(pagedResult.Items)
            };
        }



        public async Task<PaginationResponse<TodoItemDto>> GetByPriorityAsync(
            PaginationRequest pagination,
            PriorityLevel priority)
        {
            var result = await _todoRepository.GetByPriorityAsync(pagination, priority);
            return new PaginationResponse<TodoItemDto>
            {
                TotalCount = result.TotalCount,
                PageSize = result.PageSize,
                CurrentPage = result.CurrentPage,
                Items = _mapper.Map<List<TodoItemDto>>(result.Items)
            };
        }

        public async Task<PaginationResponse<TodoItemDto>> GetByCategoryAsync(
            PaginationRequest pagination,
            string category)
        {
            var result = await _todoRepository.GetByCategoryAsync(pagination, category);
            return new PaginationResponse<TodoItemDto>
            {
                TotalCount = result.TotalCount,
                PageSize = result.PageSize,
                CurrentPage = result.CurrentPage,
                Items = _mapper.Map<List<TodoItemDto>>(result.Items)
            };
        }


        public async Task<PaginationResponse<TodoItemDto>> GetFilteredAsync(
            TodoItemQuery query,
            PaginationRequest pagination)
        {
            var result = await _todoRepository.GetFilteredAsync(query, pagination);

            return new PaginationResponse<TodoItemDto>
            {
                TotalCount = result.TotalCount,
                PageSize = result.PageSize,
                CurrentPage = result.CurrentPage,
                Items = _mapper.Map<List<TodoItemDto>>(result.Items)
            };
        }

    }

}
