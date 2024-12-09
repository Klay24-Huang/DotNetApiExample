using System.Data;
using Data.Api.Users.Login;
using Data.DataStore.LLM_Platform.Users;
using Shared.Attributes;
using Shared.Helpers;

namespace Business.Services.Api.Users
{
    [Scoped]
    public class UserService(
        JwtService jwtService,
        UserRepository userRepository
        ) : IUserService
    {
        private readonly JwtService _jwtService = jwtService;
        private readonly UserRepository _userRepository = userRepository;
        public async Task<LoginResponse> Login(LoginRequest loginRequest)
        {
            var encryAccount = EncryptHelper.HasEncrypt(loginRequest.Account);
            // 查詢帳號是否存在
            var user = await _userRepository.GetUserByAccountAsync(encryAccount) ?? 
                throw new UnauthorizedAccessException("Invalid account or password");

            // 比對密碼
            var isPasswordCorrect = EncryptHelper.HasEncrypt(loginRequest.Password) == user.Password;
            if (isPasswordCorrect)
            {
                throw new UnauthorizedAccessException("Invalid account or password");
            }

            // 密碼驗證通過，生成 Token 並回應
            var refreshToken = _jwtService.GenerateRefreshToken();
            var accessToken = _jwtService.GenerateAccessToken();

            return new LoginResponse
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken
            };
        }
    }
}
