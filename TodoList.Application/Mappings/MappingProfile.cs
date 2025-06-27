using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TodoList.Application.DTOs;
using TodoList.Domain.Entities;

namespace TodoList.Application.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            //CreateMap<CreateTodoItemDto, TodoItem>();
            CreateMap<TodoItem, TodoItemDto>();


            CreateMap<UpdateTodoItemDto, TodoItem>()
                .ForMember(dest => dest.Id, opt => opt.Ignore()) // نتجاهل Id لأنه موجود في الرابط
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null)); // تحديث الخصائص غير null فقط


            CreateMap<CreateTodoItemDto, TodoItem>()
            .ForMember(dest => dest.Category, opt => opt.MapFrom(src => src.Category))
            .ForMember(dest => dest.Priority, opt => opt.MapFrom(src => src.Priority));


        }
    }


}
