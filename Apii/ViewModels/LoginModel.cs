using System.ComponentModel.DataAnnotations;
namespace Apii.ViewModels
{
	public class LoginModel
	{
		[Required]
		public string Username { get; set; }
		[Required]
		public string Password { get; set; }
	}
}