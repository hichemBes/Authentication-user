using System.ComponentModel.DataAnnotations;

namespace Apii.ViewModels
{
    public class RolesUser
    {
        [Required]
        public string Role{ get; set; }

        [Required]
        public string Username { get; set; }

    }
}
