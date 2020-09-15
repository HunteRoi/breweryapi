using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DTO;
using DAL.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Models;

namespace DAL.Repositories
{
    public class RepositoryBase<T> : IRepository<T> where T : class, IEntity
    {
        protected readonly ILogger<RepositoryBase<T>> _logger;
        protected BreweryDbContext Context { get; }

        private RepositoryBase(ILogger<RepositoryBase<T>> logger)
        {
            _logger = logger;
        }

        public RepositoryBase(BreweryDbContext context, ILogger<RepositoryBase<T>> logger) : this(logger)
        {
            Context = context;
        }

        protected DbSet<T> GetDbSet()
        {
            return Context.Set<T>();
        }

        public virtual int Count()
        {
            return GetDbSet().Count();
        }

        public virtual Task<int> CountAsync()
        {
            return GetDbSet().CountAsync();
        }

        public virtual T Read(int entityId)
        {
            return GetDbSet().Find(entityId);
        }

        public virtual Task<T> ReadAsync(int entityId)
        {
            return GetDbSet().FindAsync(entityId).AsTask();
        }

        public virtual IEnumerable<T> ReadAll()
        {
            return GetDbSet().ToArray();
        }

        public virtual Task<IEnumerable<T>> ReadAllAsync()
        {
            return Task.FromResult(ReadAll());
        }

        public virtual Task<T[]> ReadAllWithFilterAsync(string filter = null, int pageIndex = Constants.PageIndex, int pageSize = Constants.PageSize)
        {
            return Task.FromResult(GetDbSet().TakePage(pageIndex, pageSize).ToArray());
        }

        public virtual T Add(T entity)
        {
            var entityEntry = Context.Add<T>(entity);
            return entityEntry.Entity;
        }

        public virtual async Task<T> AddAsync(T entity)
        {
            var entityEntry = await Context.AddAsync(entity);
            return entityEntry.Entity;
        }

        public virtual T Edit(T entity)
        {
            Context.Update(entity);

            // Concurrency token docs : https://docs.microsoft.com/en-us/ef/core/modeling/concurrency#timestamprow-version
            // Context.Entry(entity).OriginalValues["RowVersion"] = entity.RowVersion;

            return entity;
        }

        public virtual void Delete(T entity)
        {
            Context.Remove(entity);
        }

        public int SaveChanges()
        {
            return Context.SaveChanges();
        }

        public Task<int> SaveChangesAsync()
        {
            return Context.SaveChangesAsync();
        }
    }
}
