using System.ComponentModel.DataAnnotations;

namespace Apii.ViewModels
{
	public class ConfirmEmailViewModel
	{
		[Required]
		public string Token { get; set; }
		[Required]
		public string UserId { get; set; }
	}
}