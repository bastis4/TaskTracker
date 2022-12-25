using System.Text.Json.Serialization;
using TaskTracker.Models.Project.Enums;

namespace TaskTracker.Models.Project
{
    public class Project
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public ProjectStatus Status { get; set; }
        public ProjectPriority Priority { get; set; }
        public List<TaskTracker.Models.Task.Task> Tasks { get; set; }
    }
}
