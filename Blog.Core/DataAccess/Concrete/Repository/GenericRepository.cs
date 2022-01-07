﻿using Blog.Core.DataAccess.Abstract;
using Blog.Core.Entites.Abstract;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Blog.Core.DataAccess.Concrete.Repository
{
    public class GenericRepository<T> : IRepository<T>
       where T : class, IEntity, new()
    {
        private readonly DbContext _context;

        public GenericRepository(DbContext context)
        {
            _context = context;
        }

        public async Task<T> AddAsync(T entity)
        {
            await _context.Set<T>().AddAsync(entity);
            return entity;
        }

        public async Task<bool> AnyAsync(Expression<Func<T, bool>> filter)
        {
            return await _context.Set<T>().AnyAsync(filter);

        }

        public async Task<int> CountAsync(Expression<Func<T, bool>> filter)
        {
            return await _context.Set<T>().CountAsync(filter);
        }

        public async Task DeleteAsync(T entity)
        {
            await Task.Run(() => _context.Set<T>().Remove(entity));
        }

        public async Task<List<T>> GetAllAsync(Expression<Func<T, bool>> filter = null, params Expression<Func<T, object>>[] includes)
        {
            IQueryable<T> query = _context.Set<T>();
            if (filter != null)
            {
                query = query.Where(filter);
            }
            if (includes.Any())
            {
                foreach (var include in includes)
                {
                    query = query.Include(include);
                }
            }
            return await query.ToListAsync();
        }

        public async Task<T> GetAsync(Expression<Func<T, bool>> filter, params Expression<Func<T, object>>[] includes)
        {
            IQueryable<T> query = _context.Set<T>();
            if (filter != null)
            {
                query = query.Where(filter);
            }
            if (includes.Any())
            {
                foreach (var include in includes)
                {
                    query = query.Include(include);
                }
            }
            return await query.SingleOrDefaultAsync();
        }

        public async Task<T> UpdateAsync(T entity)
        {
            await Task.Run(() => _context.Set<T>().Update(entity));
            return entity;
        }
    }
}