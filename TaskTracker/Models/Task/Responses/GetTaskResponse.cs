using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using TaskTracker.Models.Task.Enums;
using TaskStatus = TaskTracker.Models.Task.Enums.TaskStatus;

namespace TaskTracker.Models.Task.Responses
{
    public class GetTaskResponse
    {
        public int Id { get; set; }
        public int ProjectId { get; set; }
        public string Name { get; set; }

        [JsonConverter(typeof(JsonStringEnumConverter))]
        public TaskStatus Status { get; set; }
        public string Description { get; set; }
        public TaskPriority Priority { get; set; }
    }
}
