using Data.DataStore.LLM_Platform.Common;

namespace Data.DataStore.LLM_Platform.Users
{
    public interface IUserRepository : IRepository<User>
    {
        Task<User> GetUserByAccountAsync(string encryAccount);
    }
}
