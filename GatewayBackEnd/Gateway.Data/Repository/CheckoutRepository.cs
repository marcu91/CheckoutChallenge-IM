using Gateway.Interfaces.Repository;
using Microsoft.EntityFrameworkCore;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Gateway.Data.Repository
{

    public class GatewayRepository : IGatewayRepository
    {
        private GatewayDBContext Context { get; }

        public GatewayRepository(GatewayDBContext context)
        {
            Context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<TEntity> AddAsync<TEntity>(TEntity item) where TEntity : class
        {
            if (item == null)
            {
                Log.Warning("GatewayRepository.AddAsync<TEntity> invoked without {@nameof(item)}", nameof(item));
                return item;
            }
            await Context.AddAsync(item).ConfigureAwait(false);
            return item;
        }

        public async Task<IEnumerable<TEntity>> AddAsync<TEntity>(IEnumerable<TEntity> items) where TEntity : class
        {
            if (items == null)
            {
                Log.Warning("GatewayRepository.AddAsync<TEntity> invoked without {@nameof(items)}", nameof(items));
                return items;
            }

            await Context.AddRangeAsync(items).ConfigureAwait(false);
            return items;
        }

        public bool Delete<TEntity>(TEntity item) where TEntity : class
        {
            if (item == null)
            {
                Log.Warning("GatewayRepository.Delete<TEntity> invoked without {@nameof(item)}", nameof(item));
                return false;
            }

            Context.Attach(item);
            Context.Remove(item);
            return true;
        }

        public bool Delete<TEntity>(IEnumerable<TEntity> items) where TEntity : class
        {
            if (items == null)
            {
                Log.Warning("GatewayRepository.Delete<TEntity> invoked without {@nameof(items)}", nameof(items));
                return false;
            }

            Context.AttachRange(items);
            Context.RemoveRange(items);
            return true;
        }

        public bool Update<TEntity>(TEntity item) where TEntity : class
        {
            if (item == null)
            {
                Log.Warning("GatewayRepository.Update<TEntity> invoked without {@nameof(item)}", nameof(item));
                return false;
            }

            Context.Attach(item);
            Context.Update(item);
            Context.Entry(item).State = EntityState.Modified;
            return true;
        }

        public bool Update<TEntity>(IEnumerable<TEntity> items) where TEntity : class
        {
            if (items == null)
            {
                Log.Warning("GatewayRepository.Update<TEntity> invoked without {@nameof(items)}", nameof(items));
                return false;
            }

            Context.AttachRange(items);
            Context.UpdateRange(items);
            return true;
        }

        public async Task<int> SaveChangesAsync()
        {
            try
            {
                return await this.Context.SaveChangesAsync().ConfigureAwait(false);
            }
            catch(Exception ex)
            {
                Log.Error("{Exception} - {Inner}", ex.Message, ex.InnerException.Message);
                throw;
            }
        }

        public async Task<TEntity> GetByIdAsync<TEntity>(Expression<Func<TEntity, bool>> id) where TEntity : class
        {
            if (id == null)
            {
                Log.Warning("GatewayRepository.GetByIdAsync<TEntity> invoked without {id}", id);
                return await Task.FromResult(default(TEntity)).ConfigureAwait(false);
            }

            return await Context.Set<TEntity>().SingleOrDefaultAsync(id).ConfigureAwait(false);
        }

        public IQueryable<TEntity> GetAll<TEntity>() where TEntity : class
        {
            return Context.Set<TEntity>();
        }

        public IQueryable<TEntity> GetAll<TEntity, TKey>(Expression<Func<TEntity, TKey>> orderBy) where TEntity : class
        {
            try
            {
                return Context.Set<TEntity>().OrderBy(orderBy);
            }
            catch(Exception ex)
            {
                Log.Error("{Exception} - {Inner}", ex.Message, ex.InnerException.Message);
                throw;
            }
        }

        public IQueryable<TEntity> GetAll<TEntity>(int pageIndex, int pageSize) where TEntity : class
        {
            try
            {
                return Context.Set<TEntity>().Skip((pageIndex - 1) * pageSize).Take(pageSize);
            }
            catch (Exception ex)
            {
                Log.Error("{Exception} - {Inner}", ex.Message, ex.InnerException.Message);
                throw;
            }
        }

        public IQueryable<TEntity> GetAll<TEntity, TKey>(int pageIndex, int pageSize, Expression<Func<TEntity, TKey>> orderBy) where TEntity : class
        {
            try
            {
                return Context.Set<TEntity>().Skip((pageIndex - 1) * pageSize).Take(pageSize).OrderBy(orderBy);
            }
            catch (Exception ex)
            {
                Log.Error("{Exception} - {Inner}", ex.Message, ex.InnerException.Message);
                throw;
            }
        }

        public IQueryable<TEntity> Find<TEntity>(Expression<Func<TEntity, bool>> predicate) where TEntity : class
        {
            try
            {
                return Context.Set<TEntity>().Where(predicate);
            }
            catch (Exception ex)
            {
                Log.Error("{Exception} - {Inner}", ex.Message, ex.InnerException.Message);
                throw;
            }
        }

        public IQueryable<TEntity> Find<TEntity, TKey>(Expression<Func<TEntity, bool>> predicate, int pageIndex, int pageSize, Expression<Func<TEntity, TKey>> orderBy) where TEntity : class
        {
            try
            {
                return Context.Set<TEntity>().Where(predicate).Skip((pageIndex - 1) * pageSize).Take(pageSize).OrderBy(orderBy);
            }
            catch (Exception ex)
            {
                Log.Error("{Exception} - {Inner}", ex.Message, ex.InnerException.Message);
                throw;
            }
        }
    }
}