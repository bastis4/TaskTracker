using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;
using TaskTracker.DAL.Interfaces;

namespace TaskTracker.DAL.Repositories
{
    public abstract class BaseRepository<T> : IDisposable, IBaseRepository<T> where T : class
    {

        private TaskTrackerDbContext _context;
        private DbSet<T> _table;
        protected TaskTrackerDbContext Context => _context;

        public IQueryable<T> Table => _table;

        public BaseRepository(TaskTrackerDbContext dbContext)
        {
            _context = dbContext;
            _table = _context.Set<T>();
        }

        public IQueryable<T> FindAll() => _table.AsNoTracking();
        public IQueryable<T> FindByCondition(Expression<Func<T, bool>> expression) => _table.Where(expression).AsNoTracking();

        public async Task<T> GetRecord(int id, CancellationToken cancellationToken) => await _table.FindAsync(id, cancellationToken);

        public async Task<T> AddRecord(T entity, CancellationToken cancellationToken)
        {
            await _table.AddAsync(entity, cancellationToken);

            return entity;
        }

        public void UpdateRecord(T entity) => _table.Update(entity);

        public void DeleteRecord(T entity) => _table.Remove(entity);

        public void Dispose()
        {
            _context?.Dispose();
        }

    }
}
