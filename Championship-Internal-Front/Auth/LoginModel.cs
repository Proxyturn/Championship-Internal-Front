using System.ComponentModel.DataAnnotations;

namespace Championship_Internal_Front.Auth
{
    public class LoginModel
    {
        [Required]
        public string? Email { get; set; }

        [Required]
        public string? Password { get; set; }
    }
}
