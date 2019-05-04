using System.ComponentModel.DataAnnotations;

namespace Research.Web.Models.Users
{
    public class ExternalLoginConfirmationViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}
