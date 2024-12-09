using Microsoft.EntityFrameworkCore;

namespace Data.DataStore.LLM_Platform
{
    public class LLM_PlatformDbContext(DbContextOptions<LLM_PlatformDbContext> options) : DbContext(options)
    {

        // 定義資料表對應
        public required DbSet<User> Users { get; set; }
    }

    public class User
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public required string Email { get; set; }
    }
}
