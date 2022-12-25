using TaskTracker.DAL.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskTracker.DAL.Interfaces;

namespace TaskTracker.DAL.Repositories
{
    public class ProjectRepository : BaseRepository<ProjectEntity>, IProjectRepository
    {
        public ProjectRepository(TaskTrackerDbContext dbContext) : base(dbContext)
        {

        }

        public async Task<ProjectEntity> GetProjectWithAllTasks(int id)
        {
            var entity = await Context.Projects
                .Include(p => p.Tasks)
                .FirstOrDefaultAsync(t => t.Id == id);
            
            return entity;
        }
    }
}
