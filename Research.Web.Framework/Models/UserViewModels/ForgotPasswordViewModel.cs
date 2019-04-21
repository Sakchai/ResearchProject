using System.ComponentModel.DataAnnotations;

namespace Research.Web.Models.Users
{
    public class ForgotPasswordViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}
