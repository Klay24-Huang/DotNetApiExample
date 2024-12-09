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

        [HttpPost("v1")]
        public async Task<LoginResponse> Login([FromBody]LoginRequest loginRequest)
        {
            return await _userService.Login(loginRequest);
        }
    }
}
