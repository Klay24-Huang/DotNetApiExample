using Shared.Attributes;

namespace Business.Services.Api.Users
{
    public interface IJwtService
    {
        public string GenerateAccessToken(string encryptUserId, DateTime expirationTime);
        public string GenerateRefreshToken(string encryptUserId, DateTime expirationTime);
    }
}
