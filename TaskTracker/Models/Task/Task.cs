using TaskTracker.Models.Task.Enums;
using TaskStatus = TaskTracker.Models.Task.Enums.TaskStatus;

namespace TaskTracker.Models.Task
{
    public class Task
    {
        public int Id { get; set; }
        public int ProjectId { get; set; }
        public string Name { get; set; }
        public TaskStatus Status { get; set; }
        public string Description { get; set; }
        public TaskPriority Priority { get; set; }
    }
}
