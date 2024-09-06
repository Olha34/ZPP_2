using System;
using System.Collections.Generic;

namespace zpp_aplikacja.Pages.Models
{
    public class Tasks
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Points { get; set; }
        public TaskStatus Status { get; set; }
        public string Description { get; set; }
        public DateTime CreatedDate { get; set; }
        public bool CreatedByUserId { get; set; }
        public bool CompletedByUserId { get; set; }

        public Tasks()
        {
            CreatedDate = DateTime.Now;
            Status = TaskStatus.Pending;
        }

        internal string? GetTaskById()
        {
            throw new NotImplementedException();
        }
    }

    public enum TaskStatus
    {
        Pending,
        Completed
    }
}