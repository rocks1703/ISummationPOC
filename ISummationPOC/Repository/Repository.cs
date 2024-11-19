using ISummationPOC.DBContext;
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
        //This AddAsync will be used for creating or say Adding new entry for the table
        public async Task AddAsync(T entity)
        {
            await _dbSet.AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        //This DeleteAsync will be used for any entry from the table by passing there id
        public async Task DeleteAsync(int id)
        {
            var entity = await GetByIdAsync(id);
            if (entity != null)
            {
                _dbSet.Remove(entity);
                await _context.SaveChangesAsync();
            }
        }
        //This GetAllAsync will be used for getting all the data from the table 
        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _dbSet.ToListAsync();
        }

        // Here GetByIdAsync will be used to get the specified data from the table by passing Unique value like ID
        public async Task<T> GetByIdAsync(int id)
        {
            return await _dbSet.FindAsync(id);
        }
        //This UpdateAsync will be used for updating any of the Table

        public async Task UpdateAsync(T entity)
        {
            _dbSet.Update(entity);
            await _context.SaveChangesAsync();
        }
             
    }
}
