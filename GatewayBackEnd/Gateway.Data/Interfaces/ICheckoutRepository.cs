using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Gateway.Interfaces.Repository
{
    public interface IGatewayRepository
    {
        Task<IEnumerable<TEntity>> AddAsync<TEntity>(IEnumerable<TEntity> items) where TEntity : class;
        Task<TEntity> AddAsync<TEntity>(TEntity item) where TEntity : class;
        bool Delete<TEntity>(IEnumerable<TEntity> items) where TEntity : class;
        bool Delete<TEntity>(TEntity item) where TEntity : class;
        IQueryable<TEntity> Find<TEntity, TKey>(Expression<Func<TEntity, bool>> predicate, int pageIndex, int pageSize, Expression<Func<TEntity, TKey>> orderBy) where TEntity : class;
        IQueryable<TEntity> Find<TEntity>(Expression<Func<TEntity, bool>> predicate) where TEntity : class;
        IQueryable<TEntity> GetAll<TEntity, TKey>(Expression<Func<TEntity, TKey>> orderBy) where TEntity : class;
        IQueryable<TEntity> GetAll<TEntity, TKey>(int pageIndex, int pageSize, Expression<Func<TEntity, TKey>> orderBy) where TEntity : class;
        IQueryable<TEntity> GetAll<TEntity>() where TEntity : class;
        IQueryable<TEntity> GetAll<TEntity>(int pageIndex, int pageSize) where TEntity : class;
        Task<TEntity> GetByIdAsync<TEntity>(Expression<Func<TEntity, bool>> id) where TEntity : class;
        Task<int> SaveChangesAsync();
        bool Update<TEntity>(IEnumerable<TEntity> items) where TEntity : class;
        bool Update<TEntity>(TEntity item) where TEntity : class;
    }

}