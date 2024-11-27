﻿using ISummationPOC.DBContext;
using ISummationPOC.Entity;
using Microsoft.EntityFrameworkCore;

namespace ISummationPOC.Repository
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly ISummationDbContext _context;
        private readonly DbSet<T> _dbSet;

        public Repository(ISummationDbContext context)
        {
            _context = context;
            _dbSet = _context.Set<T>();
        }
     
        public async Task AddAsync(T entity)
        {
            await _dbSet.AddAsync(entity);
            await _context.SaveChangesAsync();
        }

      
        public async Task DeleteAsync(int id)
        {
            var entity = await GetByIdAsync(id);
            if (entity != null)
            {
                _dbSet.Remove(entity);
                await _context.SaveChangesAsync();
            }
        }
       
        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _dbSet.ToListAsync();
        }

      
        public async Task<T> GetByIdAsync(int id)
        {
            return await _dbSet.FindAsync(id);
        }
      

        public async Task UpdateAsync(T entity)
        {
            _dbSet.Update(entity);
            await _context.SaveChangesAsync();
        }
             
    }
}
