using System.ComponentModel.DataAnnotations;

namespace Dat_api.DTOs
{
    public class RegisterDto
    {
        [Required]
        public string Username { get; set; }
        [Required]
        public string Password { get; set; }

    }
}
