using Microsoft.EntityFrameworkCore;

namespace Data.Db.Repositories
{
    public class Repository<T>(TCoeusDbContext tcoeusDbContext) : IRepository<T> where T : class
    {
        private readonly TCoeusDbContext _tcoeusDbContext = tcoeusDbContext;
        public async Task<T?> GetByIdAsync(int id)
        {
            return await _tcoeusDbContext.Set<T>().FindAsync(id);
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _tcoeusDbContext.Set<T>().ToListAsync();
        }

        public async Task AddAsync(T entity)
        {
            await _tcoeusDbContext.Set<T>().AddAsync(entity);
            await _tcoeusDbContext.SaveChangesAsync();
        }

        public async Task UpdateAsync(T entity)
        {
            _tcoeusDbContext.Set<T>().Update(entity);
            await _tcoeusDbContext.SaveChangesAsync();
        }

        public async Task DeleteAsync(T entity)
        {
            _tcoeusDbContext.Set<T>().Remove(entity);
            await _tcoeusDbContext.SaveChangesAsync();
        }
    }
}
