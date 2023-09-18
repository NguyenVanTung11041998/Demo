using System.ComponentModel.DataAnnotations;

namespace DemoWebApi.Dtos.Users
{
    public class LoginRequest
    {
        [Required]
        public string Username { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
