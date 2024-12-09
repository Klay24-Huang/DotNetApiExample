using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shared.Attributes;

namespace Business.Services.Api.Users
{
    [Singleton]
    public class JwtService : IJwtService
    {
        public string GenerateAccessToken(string username, DateTime expirationTime)
        {
            return GenerateToken(username, expirationTime);
        }

        public string GenerateRefreshToken(string username, DateTime expirationTime)
        {
            return GenerateToken(username, expirationTime);
        }

        // 生成 Token（通用方法）
        private string GenerateToken(string username, DateTime expirationTime) 
        {
            throw new NotImplementedException();
        }
    }
}
