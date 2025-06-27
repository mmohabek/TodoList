using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TodoList.Domain.Entities
{
    public class TodoItem
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public bool IsCompleted { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? DueDate { get; set; }
        public DateTime? CompletedDate { get; set; }


        public PriorityLevel Priority { get; set; }
        [Required]
        [MaxLength(100)]
        public string Category { get; set; }




    }


    public enum PriorityLevel
    {
        Low,
        Medium,
        High,
        Critical
    }

}
