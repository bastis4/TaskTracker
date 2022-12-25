using TaskTracker.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskEntity = TaskTracker.DAL.Models.TaskEntity;

namespace TaskTracker.DAL.Interfaces
{
    public interface ITaskRepository : IBaseRepository<TaskEntity>
    {
    }
}
