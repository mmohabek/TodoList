using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TodoList.Application.DTOs;

namespace TodoList.Application.Validators
{
    public class UpdateTodoItemDtoValidator : AbstractValidator<UpdateTodoItemDto>
    {
        public UpdateTodoItemDtoValidator()
        {
            RuleFor(x => x.Title)
                .NotEmpty().WithMessage("Title is required")
                .MaximumLength(100).WithMessage("Title must not exceed 100 characters");
        }
    }
}
