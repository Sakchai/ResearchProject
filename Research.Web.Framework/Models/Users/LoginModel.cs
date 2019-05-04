using Research.Web.Framework.Models;
using System.ComponentModel.DataAnnotations;

namespace Research.Web.Models.Users
{
    public class LoginModel : BaseResearchModel
    {
        public bool CheckoutAsGuest;

        [Required]
        [Display(Name = "อีเมลล์")]
        [EmailAddress]
        public string Email { get; set; }

        public string UserName { get; set; }

        [Required]
        [Display(Name = "รหัสผ่าน")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Display(Name = "จำฉันในคราวต่อไป")]
        public bool RememberMe { get; set; }
    }
}
