using System.ComponentModel.DataAnnotations;
using System.Security.Policy;

namespace Championship_Internal_Front.Models
{
	public class Login
	{
		public int Id { get; set; }
        [Display(Name = "Email")]
        [Required(ErrorMessage = "Please fill the email", AllowEmptyStrings = false)]
        public string? Email { get; set; }

        [Required(ErrorMessage = "Please fill the password", AllowEmptyStrings = false)]
        [DataType(System.ComponentModel.DataAnnotations.DataType.Password)]
        public string? Password { get; set; }
	}
}
