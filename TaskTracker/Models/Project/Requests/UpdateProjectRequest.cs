using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using TaskTracker.Models.Project.Enums;

namespace TaskTracker.Models.Project.Requests
{
    public class UpdateProjectRequest
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        [JsonConverter(typeof(JsonStringEnumConverter))]
        public ProjectStatus Status { get; set; }
        public ProjectPriority Priority { get; set; }
    }
}
