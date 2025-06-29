using AutoMapper;
using FluentValidation;
using FluentValidation.Results;
using Moq;
using Xunit;
using TodoList.Application.Services;
using TodoList.Application.DTOs;
using TodoList.Domain.Entities;
using TodoList.Domain.Interfaces;
using TodoList.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TodoList.UnitTests.Services
{
    public class TodoServiceTests
    {
        private readonly Mock<ITodoRepository> _todoRepoMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly Mock<IValidator<CreateTodoItemDto>> _validatorMock;
        private readonly TodoService _todoService;

        public TodoServiceTests()
        {
            _todoRepoMock = new Mock<ITodoRepository>();
            _mapperMock = new Mock<IMapper>();
            _validatorMock = new Mock<IValidator<CreateTodoItemDto>>();
            _todoService = new TodoService(
                _todoRepoMock.Object,
                _mapperMock.Object,
                _validatorMock.Object);
        }

        [Fact]
        public async Task GetAllAsync_ReturnsMappedItems()
        {
            // Arrange
            var testItems = new List<TodoItem>
            {
                new TodoItem { Id = 1, Title = "Test 1" },
                new TodoItem { Id = 2, Title = "Test 2" }
            };

            var expectedDtos = new List<TodoItemDto>
            {
                new TodoItemDto { Id = 1, Title = "Test 1" },
                new TodoItemDto { Id = 2, Title = "Test 2" }
            };

            _todoRepoMock.Setup(x => x.GetAllAsync())
                .ReturnsAsync(testItems);
            _mapperMock.Setup(x => x.Map<IEnumerable<TodoItemDto>>(testItems))
                .Returns(expectedDtos);

            // Act
            var result = await _todoService.GetAllAsync();

            // Assert
            Assert.Equal(2, result.Count());
            _todoRepoMock.Verify(x => x.GetAllAsync(), Times.Once);
            _mapperMock.Verify(x => x.Map<IEnumerable<TodoItemDto>>(testItems), Times.Once);
        }

        [Fact]
        public async Task GetByIdAsync_ReturnsMappedItem_WhenItemExists()
        {
            // Arrange
            var testItem = new TodoItem { Id = 1, Title = "Test Item" };
            var expectedDto = new TodoItemDto { Id = 1, Title = "Test Item" };

            _todoRepoMock.Setup(x => x.GetByIdAsync(1))
                .ReturnsAsync(testItem);
            _mapperMock.Setup(x => x.Map<TodoItemDto>(testItem))
                .Returns(expectedDto);

            // Act
            var result = await _todoService.GetByIdAsync(1);

            // Assert
            Assert.Equal(1, result.Id);
            _todoRepoMock.Verify(x => x.GetByIdAsync(1), Times.Once);
            _mapperMock.Verify(x => x.Map<TodoItemDto>(testItem), Times.Once);
        }

        [Fact]
        public async Task GetByIdAsync_ThrowsKeyNotFoundException_WhenItemDoesNotExist()
        {
            // Arrange
            int nonExistingId = 999;
            _todoRepoMock.Setup(x => x.GetByIdAsync(nonExistingId))
                .ReturnsAsync((TodoItem)null); // محاكاة عدم وجود العنصر

            // Act & Assert
            var exception = await Assert.ThrowsAsync<KeyNotFoundException>(
                () => _todoService.GetByIdAsync(nonExistingId));

            Assert.Contains(nonExistingId.ToString(), exception.Message);
            _todoRepoMock.Verify(x => x.GetByIdAsync(nonExistingId), Times.Once);
        }


        [Fact]
        public async Task CreateAsync_ReturnsMappedItem_WhenInputIsValid()
        {
            // Arrange
            var createDto = new CreateTodoItemDto
            {
                Title = "New Item",
                Description = "Test Description",
                Priority = PriorityLevel.Medium
            };

            var newItem = new TodoItem
            {
                Id = 1,
                Title = "New Item",
                Description = "Test Description",
                Priority = PriorityLevel.Medium
            };

            var expectedDto = new TodoItemDto
            {
                Id = 1,
                Title = "New Item",
                Description = "Test Description",
                Priority = PriorityLevel.Medium
            };

            // إعداد المحاكاة
            _validatorMock.Setup(x => x.ValidateAsync(createDto, default))
                .ReturnsAsync(new ValidationResult());

            _mapperMock.Setup(x => x.Map<TodoItem>(createDto))
                .Returns(newItem);

            // التعديل هنا: استخدام معطى واحد فقط كما في الوظيفة الأصلية
            _todoRepoMock.Setup(x => x.AddAsync(newItem))
                .Returns(Task.CompletedTask)
                .Callback<TodoItem>(item => item.Id = 1);

            _mapperMock.Setup(x => x.Map<TodoItemDto>(It.Is<TodoItem>(i => i.Id == 1)))
                .Returns(expectedDto);

            // Act
            var result = await _todoService.CreateAsync(createDto);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(expectedDto.Id, result.Id);
            Assert.Equal(expectedDto.Title, result.Title);

            _validatorMock.Verify(x => x.ValidateAsync(createDto, default), Times.Once);
            _mapperMock.Verify(x => x.Map<TodoItem>(createDto), Times.Once);
            _todoRepoMock.Verify(x => x.AddAsync(newItem), Times.Once);
            _mapperMock.Verify(x => x.Map<TodoItemDto>(It.Is<TodoItem>(i => i.Id == 1)), Times.Once);
        }


        [Fact]
        public async Task CreateAsync_ThrowsValidationException_WhenInputIsInvalid()
        {
            // Arrange
            var createDto = new CreateTodoItemDto();
            var validationErrors = new List<ValidationFailure>
            {
                new ValidationFailure("Title", "Title is required")
            };

            _validatorMock.Setup(x => x.ValidateAsync(createDto, default))
                .ReturnsAsync(new ValidationResult(validationErrors));

            // Act & Assert
            var ex = await Assert.ThrowsAsync<FluentValidation.ValidationException>(() => _todoService.CreateAsync(createDto));
            Assert.Contains("Title is required", ex.Message);
            _validatorMock.Verify(x => x.ValidateAsync(createDto, default), Times.Once);
            _todoRepoMock.Verify(x => x.AddAsync(It.IsAny<TodoItem>()), Times.Never);
        }

        [Fact]
        public async Task UpdateAsync_UpdatesItem_WhenItemExists()
        {
            // Arrange
            var existingItem = new TodoItem { Id = 1, Title = "Old Title" };
            var updateDto = new UpdateTodoItemDto { Title = "New Title" };

            _todoRepoMock.Setup(x => x.GetByIdAsync(1))
                .ReturnsAsync(existingItem);
            _todoRepoMock.Setup(x => x.UpdateAsync(existingItem))
                .Returns(Task.CompletedTask);

            // Act
            await _todoService.UpdateAsync(1, updateDto);

            // Assert
            _mapperMock.Verify(x => x.Map(updateDto, existingItem), Times.Once);
            _todoRepoMock.Verify(x => x.UpdateAsync(existingItem), Times.Once);
        }

        [Fact]
        public async Task UpdateAsync_ThrowsKeyNotFoundException_WhenItemDoesNotExist()
        {
            // Arrange
            var updateDto = new UpdateTodoItemDto { Title = "New Title" };
            _todoRepoMock.Setup(x => x.GetByIdAsync(1))
                .ReturnsAsync((TodoItem)null);

            // Act & Assert
            await Assert.ThrowsAsync<KeyNotFoundException>(() =>
                _todoService.UpdateAsync(1, updateDto));
            _todoRepoMock.Verify(x => x.UpdateAsync(It.IsAny<TodoItem>()), Times.Never);
        }

        [Fact]
        public async Task DeleteAsync_DeletesItem_WhenItemExists()
        {
            // Arrange
            var existingItem = new TodoItem { Id = 1 };
            _todoRepoMock.Setup(x => x.GetByIdAsync(1))
                .ReturnsAsync(existingItem);
            _todoRepoMock.Setup(x => x.DeleteAsync(1))
                .Returns(Task.CompletedTask);

            // Act
            await _todoService.DeleteAsync(1);

            // Assert
            _todoRepoMock.Verify(x => x.DeleteAsync(1), Times.Once);
        }

        [Fact]
        public async Task DeleteAsync_ThrowsKeyNotFoundException_WhenItemDoesNotExist()
        {
            // Arrange
            _todoRepoMock.Setup(x => x.GetByIdAsync(1))
                .ReturnsAsync((TodoItem)null);

            // Act & Assert
            await Assert.ThrowsAsync<KeyNotFoundException>(() => _todoService.DeleteAsync(1));
            _todoRepoMock.Verify(x => x.DeleteAsync(It.IsAny<int>()), Times.Never);
        }

        [Fact]
        public async Task ToggleCompletionStatusAsync_TogglesStatus_WhenItemExists()
        {
            // Arrange
            var existingItem = new TodoItem { Id = 1, IsCompleted = false };
            _todoRepoMock.Setup(x => x.GetByIdAsync(1))
                .ReturnsAsync(existingItem);
            _todoRepoMock.Setup(x => x.UpdateAsync(existingItem))
                .Returns(Task.CompletedTask);

            // Act
            await _todoService.ToggleCompletionStatusAsync(1);

            // Assert
            Assert.True(existingItem.IsCompleted);
            Assert.NotNull(existingItem.CompletedDate);
            _todoRepoMock.Verify(x => x.UpdateAsync(existingItem), Times.Once);
        }

        [Fact]
        public async Task ToggleCompletionStatusAsync_ThrowsKeyNotFoundException_WhenItemDoesNotExist()
        {
            // Arrange
            _todoRepoMock.Setup(x => x.GetByIdAsync(1))
                .ReturnsAsync((TodoItem)null);

            // Act & Assert
            await Assert.ThrowsAsync<KeyNotFoundException>(() =>
                _todoService.ToggleCompletionStatusAsync(1));
            _todoRepoMock.Verify(x => x.UpdateAsync(It.IsAny<TodoItem>()), Times.Never);
        }

        [Fact]
        public async Task GetPagedAsync_ReturnsMappedPagedResult()
        {
            // Arrange
            var pagination = new PaginationRequest { PageNumber = 1, PageSize = 10 };
            var pagedItems = new List<TodoItem>
            {
                new TodoItem { Id = 1, Title = "Item 1" }
            };
            var pagedResult = new PaginationResponse<TodoItem>
            {
                Items = pagedItems,
                TotalCount = 1,
                PageSize = 10,
                CurrentPage = 1
            };
            var expectedDto = new TodoItemDto { Id = 1, Title = "Item 1" };

            _todoRepoMock.Setup(x => x.GetPagedAsync(pagination))
                .ReturnsAsync(pagedResult);
            _mapperMock.Setup(x => x.Map<List<TodoItemDto>>(pagedItems))
                .Returns(new List<TodoItemDto> { expectedDto });

            // Act
            var result = await _todoService.GetPagedAsync(pagination);

            // Assert
            Assert.Equal(1, result.TotalCount);
            Assert.Single(result.Items);
            Assert.Equal(1, result.Items.First().Id);
            _todoRepoMock.Verify(x => x.GetPagedAsync(pagination), Times.Once);
        }

        [Fact]
        public async Task GetByPriorityAsync_ReturnsFilteredMappedResult()
        {
            // Arrange
            var pagination = new PaginationRequest { PageNumber = 1, PageSize = 10 };
            var priority = PriorityLevel.High;
            var pagedItems = new List<TodoItem>
            {
                new TodoItem { Id = 1, Title = "Urgent", Priority = PriorityLevel.High }
            };
            var pagedResult = new PaginationResponse<TodoItem>
            {
                Items = pagedItems,
                TotalCount = 1
            };

            _todoRepoMock.Setup(x => x.GetByPriorityAsync(pagination, priority))
                .ReturnsAsync(pagedResult);
            _mapperMock.Setup(x => x.Map<List<TodoItemDto>>(pagedItems))
                .Returns(new List<TodoItemDto> { new TodoItemDto { Priority = PriorityLevel.High } });

            // Act
            var result = await _todoService.GetByPriorityAsync(pagination, priority);

            // Assert
            Assert.Equal(PriorityLevel.High, result.Items.First().Priority);
            _todoRepoMock.Verify(x => x.GetByPriorityAsync(pagination, priority), Times.Once);
        }

        [Fact]
        public async Task GetByCategoryAsync_ReturnsFilteredMappedResult()
        {
            // Arrange
            var pagination = new PaginationRequest { PageNumber = 1, PageSize = 10 };
            var category = "work";
            var pagedItems = new List<TodoItem>
            {
                new TodoItem { Id = 1, Title = "Work Task", Category = "work" }
            };
            var pagedResult = new PaginationResponse<TodoItem>
            {
                Items = pagedItems,
                TotalCount = 1
            };

            _todoRepoMock.Setup(x => x.GetByCategoryAsync(pagination, category))
                .ReturnsAsync(pagedResult);
            _mapperMock.Setup(x => x.Map<List<TodoItemDto>>(pagedItems))
                .Returns(new List<TodoItemDto> { new TodoItemDto { Category = "work" } });

            // Act
            var result = await _todoService.GetByCategoryAsync(pagination, category);

            // Assert
            Assert.Equal("work", result.Items.First().Category);
            _todoRepoMock.Verify(x => x.GetByCategoryAsync(pagination, category), Times.Once);
        }

        [Fact]
        public async Task GetFilteredAsync_ReturnsFilteredMappedResult()
        {
            // Arrange
            var pagination = new PaginationRequest { PageNumber = 1, PageSize = 10 };
            var query = new TodoItemQuery { SearchTerm = "urgent" };
            var pagedItems = new List<TodoItem>
            {
                new TodoItem { Id = 1, Title = "Urgent Task" }
            };
            var pagedResult = new PaginationResponse<TodoItem>
            {
                Items = pagedItems,
                TotalCount = 1
            };

            _todoRepoMock.Setup(x => x.GetFilteredAsync(query, pagination))
                .ReturnsAsync(pagedResult);
            _mapperMock.Setup(x => x.Map<List<TodoItemDto>>(pagedItems))
                .Returns(new List<TodoItemDto> { new TodoItemDto { Title = "Urgent Task" } });

            // Act
            var result = await _todoService.GetFilteredAsync(query, pagination);

            // Assert
            Assert.Equal("Urgent Task", result.Items.First().Title);
            _todoRepoMock.Verify(x => x.GetFilteredAsync(query, pagination), Times.Once);
        }
    }
}
