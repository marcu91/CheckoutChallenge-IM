using Gateway.Interfaces.Repository;
using Gateway.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Gateway.Data.Services
{
    public class RepositoryService : IRepositoryService
    {
        private const bool SAVE = true;
        private readonly IGatewayRepository repository;
        public RepositoryService(IGatewayRepository repository)
        {
            this.repository = repository;
        }
        public async Task<TEntity> AddAsync<TEntity>(TEntity item, bool AutoSave = SAVE) where TEntity : class
        {
            await repository.AddAsync(item).ConfigureAwait(false);
            if (AutoSave) await SaveChangesAsync().ConfigureAwait(false);
            return item;
        }

        public async Task<IEnumerable<TEntity>> Add<TEntity>(IEnumerable<TEntity> items, bool AutoSave = SAVE) where TEntity : class
        {
            await repository.AddAsync(items).ConfigureAwait(false);
            if (AutoSave) await SaveChangesAsync().ConfigureAwait(false); ;
            return items;
        }

        public async Task<bool> UpdateAsync<TEntity>(TEntity item, bool AutoSave = SAVE) where TEntity : class
        {
            var result = repository.Update(item);
            if (AutoSave) await SaveChangesAsync().ConfigureAwait(false); ;
            return result;
        }

        public async Task<bool> Update<TEntity>(IEnumerable<TEntity> items, bool AutoSave = SAVE) where TEntity : class
        {
            var result = repository.Update(items);
            if (AutoSave) await SaveChangesAsync().ConfigureAwait(false); ;
            return result;
        }

        public async Task<bool> Delete<TEntity>(TEntity item, bool AutoSave = SAVE) where TEntity : class
        {
            var result = repository.Delete(item);
            if (AutoSave) await SaveChangesAsync().ConfigureAwait(false); ;
            return result;
        }

        public async Task<bool> Delete<TEntity>(IEnumerable<TEntity> items, bool AutoSave = SAVE) where TEntity : class
        {
            var result = repository.Delete(items);
            if (AutoSave) await SaveChangesAsync().ConfigureAwait(false);
            return result;
        }

        public async Task<bool> DeleteById<TEntity>(Expression<Func<TEntity, bool>> predicate, bool AutoSave = SAVE) where TEntity : class
        {
            var items = repository.Find(predicate);
            if (items == null) return false;

            var result = repository.Delete<TEntity>(items);
            if (AutoSave) await SaveChangesAsync().ConfigureAwait(false);
            return result;
        }

        public async Task<int> SaveChangesAsync()
        {
            return await repository.SaveChangesAsync().ConfigureAwait(false);
        }

        public async Task<TEntity> GetByID<TEntity>(Expression<Func<TEntity, bool>> id) where TEntity : class
        {
            return await repository.GetByIdAsync(id).ConfigureAwait(false);
        }

        public IQueryable<TEntity> GetAll<TEntity>() where TEntity : class
        {
            return repository.GetAll<TEntity>();
        }

        public IQueryable<TEntity> GetAll<TEntity, TKey>(Expression<Func<TEntity, TKey>> orderBy) where TEntity : class
        {
            return repository.GetAll(orderBy);
        }

        public IQueryable<TEntity> GetAll<TEntity>(int pageIndex, int pageSize) where TEntity : class
        {
            return repository
                .GetAll<TEntity>(pageIndex, pageSize);
        }

        public IQueryable<TEntity> GetAll<TEntity, TKey>(int pageIndex, int pageSize, Expression<Func<TEntity, TKey>> orderBy) where TEntity : class
        {
            return repository
                .GetAll(pageIndex, pageSize, orderBy);
        }

        public IQueryable<TEntity> Find<TEntity>(Expression<Func<TEntity, bool>> predicate) where TEntity : class
        {
            return repository
                .Find(predicate);
        }

        public IQueryable<TEntity> Find<TEntity, TKey>(Expression<Func<TEntity, bool>> predicate, int pageIndex, int pageSize, Expression<Func<TEntity, TKey>> orderBy) where TEntity : class
        {
            return repository
                .Find(predicate, pageIndex, pageSize, orderBy);
        }
    }
}