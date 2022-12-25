using TaskTracker.DAL.Interfaces;

namespace TaskTracker.DAL.Repositories
{
    public class WrapperRepository : IWrapperRepository
    {
        private readonly TaskTrackerDbContext _context;
        private IProjectRepository _project;
        private ITaskRepository _task;

        public IProjectRepository Project => _project ??= new ProjectRepository(_context);

        public ITaskRepository Task => _task ??= new TaskRepository(_context);

        public WrapperRepository(TaskTrackerDbContext context)
        {
            _context = context;
        }

        public async Task SaveAsync(CancellationToken cancellationToken)
        {
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task DisposeAsync()
        {
            await _context.DisposeAsync();
        }
    }
}
