using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TodoList.Domain.Entities;
using Microsoft.EntityFrameworkCore;


namespace TodoList.Infrastructure.Data
{
    public static class SeedData
    {
        public static void Initialize(ApplicationDbContext context)
        {
            context.Database.EnsureCreated();

            if (!context.Users.Any())
            {
                var users = new User[]
                {
                    new User
                    {
                        Email = "owner1@example.com",
                        Username = "owner1",
                        PasswordHash = BCrypt.Net.BCrypt.HashPassword("Abc@123"),
                        Role = UserRoles.Owner,
                        CreatedAt = DateTime.UtcNow
                    },
                    new User
                    {
                        Email = "guest1@example.com",
                        Username = "guest1",
                        PasswordHash = BCrypt.Net.BCrypt.HashPassword("Abc@1234"),
                        Role = UserRoles.Guest,
                        CreatedAt = DateTime.UtcNow
                    }
                };

                context.Users.AddRange(users);
                context.SaveChanges();

                if (!context.TodoItems.Any())
                {
                    var todos = new TodoItem[]
                    {
                        new TodoItem
                        {
                            Title = "تعلم ASP.NET Core",
                            Description = "إنشاء مشروع TodoList",
                            DueDate = DateTime.UtcNow.AddDays(7),
                            IsCompleted = false,
                            CreatedAt = DateTime.UtcNow,
                            Category = "A",
                            Priority = PriorityLevel.High
                        },
                        new TodoItem
                        {
                            Title = "كتابة اختبارات وحدة",
                            Description = "اختبار خدمة المصادقة",
                            DueDate = DateTime.UtcNow.AddDays(3),
                            IsCompleted = false,
                            CreatedAt = DateTime.UtcNow,
                            Category = "B",
                            Priority = PriorityLevel.Low                        
                        }
                    };

                    context.TodoItems.AddRange(todos);
                    context.SaveChanges();
                }
            }
        }
    }
}
