using TaskTracker.DAL.Models;
using Microsoft.EntityFrameworkCore;
using TaskEntity = TaskTracker.DAL.Models.TaskEntity;

namespace TaskTracker.DAL
{
    public class TaskTrackerDbContext : DbContext
    {
        public DbSet<ProjectEntity> Projects { get; set; }
        public DbSet<TaskEntity> Tasks { get; set; }

        public TaskTrackerDbContext(DbContextOptions<TaskTrackerDbContext> options) : base(options)
        {
            Database.EnsureCreated();
        }

/*        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Project>()
               .HasMany(e => e.Tasks);
        }*/
    }
}