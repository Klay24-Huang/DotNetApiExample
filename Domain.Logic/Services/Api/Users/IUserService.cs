using Data.Api.Users.Login;

namespace Business.Services.Api.Users
{
    public interface IUserService
    {
        public Task<LoginResponse> Login(LoginRequest loginRequest);
    }
}
