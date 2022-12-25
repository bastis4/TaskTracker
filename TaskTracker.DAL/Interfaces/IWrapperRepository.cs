namespace TaskTracker.DAL.Interfaces
{
    public interface IWrapperRepository
    {
        IProjectRepository Project { get; }
        ITaskRepository Task { get; }
        Task SaveAsync(CancellationToken cancellationToken);
        Task DisposeAsync();
    }
}
