using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Gateway.Interfaces.Services
{
    public interface IRepositoryService
    {
        Task<IEnumerable<TEntity>> Add<TEntity>(IEnumerable<TEntity> items, bool AutoSave = true) where TEntity : class;
        Task<TEntity> AddAsync<TEntity>(TEntity item, bool AutoSave = true) where TEntity : class;
        Task<bool> Delete<TEntity>(IEnumerable<TEntity> items, bool AutoSave = true) where TEntity : class;
        Task<bool> Delete<TEntity>(TEntity item, bool AutoSave = true) where TEntity : class;
        Task<bool> DeleteById<TEntity>(Expression<Func<TEntity, bool>> predicate, bool AutoSave = true) where TEntity : class;
        IQueryable<TEntity> Find<TEntity, TKey>(Expression<Func<TEntity, bool>> predicate, int pageIndex, int pageSize, Expression<Func<TEntity, TKey>> orderBy) where TEntity : class;
        IQueryable<TEntity> Find<TEntity>(Expression<Func<TEntity, bool>> predicate) where TEntity : class;
        IQueryable<TEntity> GetAll<TEntity, TKey>(Expression<Func<TEntity, TKey>> orderBy) where TEntity : class;
        IQueryable<TEntity> GetAll<TEntity, TKey>(int pageIndex, int pageSize, Expression<Func<TEntity, TKey>> orderBy) where TEntity : class;
        IQueryable<TEntity> GetAll<TEntity>() where TEntity : class;
        IQueryable<TEntity> GetAll<TEntity>(int pageIndex, int pageSize) where TEntity : class;
        Task<TEntity> GetByID<TEntity>(Expression<Func<TEntity, bool>> id) where TEntity : class;
        Task<int> SaveChangesAsync();
        Task<bool> Update<TEntity>(IEnumerable<TEntity> items, bool AutoSave = true) where TEntity : class;
        Task<bool> UpdateAsync<TEntity>(TEntity item, bool AutoSave = true) where TEntity : class;
    }

}