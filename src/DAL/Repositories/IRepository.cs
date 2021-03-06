﻿using DTO;
using Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DAL.Repositories
{
    public interface IRepository<T> where T : class, IEntity
    {
        int Count();

        Task<int> CountAsync();

        T Read(int entityId);

        Task<T> ReadAsync(int entityId);

        IEnumerable<T> ReadAll();

        Task<IEnumerable<T>> ReadAllAsync();

        Task<T[]> ReadAllWithFilterAsync(string filter = null, int pageIndex = Constants.PageIndex, int pageSize = Constants.PageSize);

        T Add(T entity);

        Task<T> AddAsync(T entity);

        T Edit(T entity);

        void Delete(T entity);

        int SaveChanges();

        Task<int> SaveChangesAsync();
    }
}

