using Microsoft.EntityFrameworkCore;
using ParkinApp.Domain.Abstractions.Repositories;

namespace ParkinApp.Persistence.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        private readonly DbContext _context;

        protected GenericRepository(DbContext context)
        {
            _context = context;
        }

        public async Task<List<T>> GetAllAsync()
        {
            return await _context.Set<T>().ToListAsync();
        }

        public async Task<T?> GetByIdAsync(int id)
        {
            return await _context.Set<T>().FindAsync(id);
        }

        public async Task AddAsync(T entity)
        {
            await _context.Set<T>().AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(T entity)
        {
            var entityType = _context.Model.FindEntityType(typeof(T));
            var primaryKey = entityType.FindPrimaryKey();
            var keyValues = primaryKey.Properties.Select(p => p.PropertyInfo.GetValue(entity)).ToArray();
            var trackedEntity = await _context.Set<T>().FindAsync(keyValues);

            if (trackedEntity != null)
            {
                _context.Entry(trackedEntity).CurrentValues.SetValues(entity);
            }
            else
            {
                _context.Set<T>().Update(entity);
            }

            await _context.SaveChangesAsync();
        }
        
        public async Task DeleteAsync(T entity)
        {
            _context.Set<T>().Remove(entity);
            await _context.SaveChangesAsync();
        }
    }
}