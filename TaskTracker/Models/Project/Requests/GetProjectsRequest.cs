using System.Text.Json.Serialization;
using TaskTracker.Models.Common.Classes;
using TaskTracker.Models.Common.Enums;
using TaskTracker.Models.Project.Enums;

namespace TaskTracker.Models.Project.Requests
{
    public class GetProjectsRequest
    {
        public string? SearchName { get; set; }
        public Range<DateTime?> StartDate { get; set; }
        public Range<DateTime?> EndDate { get; set; }

        [JsonConverter(typeof(JsonStringEnumConverter))]
        public List<ProjectStatus> Statuses { get; set; }
        public List<ProjectPriority> Priorities { get; set; }

        public ProjectSortType SortType { get; set; }
        public SortOrder SortOrder { get; set; }
    }
}
