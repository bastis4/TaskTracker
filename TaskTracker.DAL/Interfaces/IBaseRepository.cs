using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using TaskTracker.DAL.Models;

namespace TaskTracker.DAL.Interfaces
{
    public interface IBaseRepository<T>
    {
        IQueryable<T> Table { get; }

        //List<T> GetAllRecords();
        IQueryable<T> FindAll();
        IQueryable<T> FindByCondition(Expression<Func<T, bool>> expression);
        Task<T> GetRecord(int id, CancellationToken cancellationToken);
        Task<T> AddRecord(T entity, CancellationToken cancellationToken);
        void UpdateRecord(T entity);
        void DeleteRecord(T entity);
    }
}
