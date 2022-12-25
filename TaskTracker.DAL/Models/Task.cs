using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskTracker.DAL.Models
{
    public class TaskEntity
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "ProjectId is required")]
        public int ProjectId { get; set; }

        [ForeignKey(nameof(ProjectId))]
        public virtual ProjectEntity Project { get; set; }
        public string Name { get; set; }

        [Required(ErrorMessage = "Status is required")]
        public int Status { get; set; }
        public string Description { get; set; }

        [Required(ErrorMessage = "Priority is required")]
        public int Priority { get; set; }
    }
}
