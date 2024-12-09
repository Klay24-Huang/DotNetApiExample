using Swashbuckle.AspNetCore.Annotations;

namespace Data.Api.Users.Login
{
    public class LoginResponse
    {
        [SwaggerSchema("JWT Refresh Token 3個月有效")]
        public required string RefreshToken { get; init; }

        [SwaggerSchema("JWT Access Token, 30分鐘有效")]
        public required string AccessToken { get; init; }
    }

}
