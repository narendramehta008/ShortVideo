using System.ComponentModel.DataAnnotations;

namespace ShortVideo.API.Models
{
    public class UserForRegisterDto
    {
        [Required]
        public string Username { get; set; }

        [Required]
        [StringLength(8, MinimumLength = 4, ErrorMessage = "Password must be between 4 and 8")]
        public string Password { get; set; }
    }

    public class UserForLoginDto
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }
}