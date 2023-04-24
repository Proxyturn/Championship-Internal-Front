using System.ComponentModel.DataAnnotations;

namespace Championship_Internal_Front.Auth
{
    public class RegisterModel
    {
        [Required]
        public string? Username { get; set; }

        [EmailAddress]
        [Required]
        public string? Email { get; set; }

        [Required]
        public string? Password { get; set; }
    }
}
