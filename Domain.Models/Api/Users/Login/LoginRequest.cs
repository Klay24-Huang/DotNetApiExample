using System.ComponentModel.DataAnnotations;

namespace Data.Api.Users.Login
{
    public class LoginRequest
    {
        [Required(ErrorMessage = "帳號不可為空")]
        [StringLength(15, ErrorMessage = "帳號超過15字")]
        public required string Account {  get; set; }
        [Required(ErrorMessage = "密碼不可為空")]
        [StringLength(15, ErrorMessage = "密碼超過15字")]
        public required string Password { get; set; }
    }
}
