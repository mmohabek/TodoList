using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TodoList.Domain.Entities;

namespace TodoList.Domain.Common
{
    public class TodoItemQuery
    {
        public string? SearchTerm { get; set; } = null!; 
        public bool? IsCompleted { get; set; } 
        public PriorityLevel? Priority { get; set; }
        public string? Category { get; set; } = null!;  
        public DateTime? DueDateFrom { get; set; } 
        public DateTime? DueDateTo { get; set; }

    }
}
