using TaskTracker.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskTracker.DAL.Interfaces;
using TaskEntity = TaskTracker.DAL.Models.TaskEntity;

namespace TaskTracker.DAL.Repositories
{
    public class TaskRepository : BaseRepository<TaskEntity>, ITaskRepository
    {
        public TaskRepository(TaskTrackerDbContext dbContext) : base(dbContext)
        {
        }
    }
}
