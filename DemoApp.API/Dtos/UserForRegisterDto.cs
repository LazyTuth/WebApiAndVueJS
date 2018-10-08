using System.ComponentModel.DataAnnotations;

namespace DemoApp.API.Dtos
{
    public class UserForRegisterDto
    {
        [Required]
        [StringLength(20)]
        public string Firstname { get; set; }
        [Required]
        [StringLength(50)]
        public string Lastname { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        [StringLength(10, MinimumLength=4, ErrorMessage="Length must be between {1} and {2}")]
        public string Username { get; set; }
        [Required]
        [MinLength(6)]
        public string Password { get;set; }
    }
}