using Shared.Attributes;

namespace Business.Services.Api.Users
{
    public interface IJwtService
    {
        public string GenerateAccessToken(string username, DateTime expirationTime);
        public string GenerateRefreshToken(string username, DateTime expirationTime);
    }
}
