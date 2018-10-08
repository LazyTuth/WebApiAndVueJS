using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using DemoApp.API.Data;
using Microsoft.EntityFrameworkCore;

namespace DemoApp.API.services
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly MyDbContext _context;
        private DbSet<T> _entities;
        public Repository(MyDbContext context)
        {
            _context = context;
            _entities = _context.Set<T>();
        }
        public async Task<IEnumerable<T>> GetAll()
        {
            return await _entities.ToListAsync();
        }

        public async Task<IEnumerable<T>> GetAllWithCondition(Expression<Func<T, bool>> expression)
        {
            return await _entities.Where(expression).ToListAsync();
        }

        public async Task<T> Find(object id)
        {
            return await _entities.FindAsync(id);
        }

        public void Insert(T entity)
        {
            _entities.Add(entity);
        }

        public void Update(T entity, object key)
        {
            var exist = _entities.Find(key);
            if (exist != null)
            {
                _context.Entry(exist).CurrentValues.SetValues(entity);
            }
        }
        
        public void Delete(T entity)
        {
            _entities.Remove(entity);
        }

        public async Task<T> InsertAsync(T entity)
        {
            try
            {
                _entities.Add(entity);
                await _context.SaveChangesAsync();
                return entity;
            }
            catch
            {
                return null;
            }
        }

        public async Task<T> UpdateAsync(T entity, object key)
        {
            try
            {
                var exist = await _entities.FindAsync(key);
                if (exist != null)
                {
                    _context.Entry(exist).CurrentValues.SetValues(entity);
                    await _context.SaveChangesAsync();
                }
                else
                    return null;

                return exist;
            }
            catch
            {
                return null;
            }
        }

        public async Task<int> DeleteAsync(T entity)
        {
            try
            {
                _entities.Remove(entity);
                return await _context.SaveChangesAsync();
            }
            catch (Exception)
            {
                return 0;
            }
        }

        public void Save()
        {
            _context.SaveChanges();
        }
    }
}