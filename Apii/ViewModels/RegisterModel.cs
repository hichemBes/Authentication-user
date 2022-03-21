using System.ComponentModel.DataAnnotations;

namespace Apii.ViewModels
{
	public class RegisterModel
	{
		[Required]
		public string Username { get; set; }
		[Required]
		public string Email { get; set; }
		[Required]
		public string Password { get; set; }
        //[Required]
        //public string Role { get; set; }
        [Required]
        public string JobTitle { get; set; }
		[Required]
		public string Filliale { get; set; }
	}
}