using Business.Services.Api.Users;
using Data.Api.Users.Login;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController(
        IUserService userService
        ) : ControllerBase
    {
        private readonly IUserService _userService = userService;

        /// <summary>
        /// 用戶登入 API
        /// </summary>
        /// <param name="loginRequest">用戶登入請求資料</param>
        /// <returns>登入回應資料</returns>
        /// <response code="200">登入成功</response>
        /// <response code="400">錯誤的請求參數</response>
        /// <response code="500">伺服器錯誤</response>
        [HttpPost("v1")]
        [ProducesResponseType(typeof(LoginResponse), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<LoginResponse>> Login([FromBody] LoginRequest loginRequest)
        {
            return await _userService.Login(loginRequest);
        }
    }
}
