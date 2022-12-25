using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using TaskTracker.Models.Task.Enums;
using TaskStatus = TaskTracker.Models.Task.Enums.TaskStatus;

namespace TaskTracker.Models.Task.Requests
{
    public class GetTaskRequest
    {
        public int Id { get; set; }
    }
}
