using Data.DataStore.LLM_Platform.Common;
using Shared.Attributes;

namespace Data.DataStore.LLM_Platform.Users
{
    [Scoped]
    public class UserRepository(LLM_PlatformDbContext lLM_PlatformDbContext) : Repository<User>(lLM_PlatformDbContext), IUserRepository
    {
        public Task<User> GetUserByAccountAsync(string encryAccount)
        {
            throw new NotImplementedException();
        }
    }
}
