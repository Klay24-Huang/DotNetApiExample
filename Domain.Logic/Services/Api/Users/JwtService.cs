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
        public string GenerateAccessToken(string encryptUserId, DateTime expirationTime)
        {
            return GenerateToken(encryptUserId, expirationTime);
        }

        public string GenerateRefreshToken(string encryptUserId, DateTime expirationTime)
        {
            return GenerateToken(encryptUserId, expirationTime);
        }

        // 生成 Token（通用方法）
        private string GenerateToken(string encryptUserId, DateTime expirationTime) 
        {
            throw new NotImplementedException();
        }
    }
}
