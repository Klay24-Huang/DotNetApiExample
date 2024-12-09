using Data.DataStore.LLM_Platform;
using Microsoft.EntityFrameworkCore;
using Shared.Attributes;
using Shared.Helpers;

namespace Data.DataStore.LLM_Platform.Common
{
    public class Repository<T>(
        LLM_PlatformDbContext tcoeusDbContext
        ) : IRepository<T> where T : class
    {
        private readonly LLM_PlatformDbContext _tcoeusDbContext = tcoeusDbContext;
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
            var now = TimeHelper.GetTimeByTimeZone();
            if (entity is ICreatedAt createdAtEntity)
            {
                createdAtEntity.CreatedAt = now; 
            }

            if (entity is IUpdatedAt updatedAtEntity)
            {
                updatedAtEntity.UpdatedAt = now;
            }

            await _tcoeusDbContext.Set<T>().AddAsync(entity);
            await _tcoeusDbContext.SaveChangesAsync();
        }

        public async Task UpdateAsync(T entity)
        {
            var now = TimeHelper.GetTimeByTimeZone();
            if (entity is IUpdatedAt updatedAtEntity)
            {
                updatedAtEntity.UpdatedAt = now;
            }

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
