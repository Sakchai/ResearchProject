using System.ComponentModel.DataAnnotations;
using Research.Web.Framework.Models;

namespace Research.Web.Models.Users
{
    //[Validator(typeof(PasswordRecoveryConfirmValidator))]
    public partial class PasswordRecoveryConfirmModel : BaseResearchModel
    {
        public string Email { get; set; }
        public string Token { get; set; }
        [DataType(DataType.Password)]
        //[NoTrim]
        //[ResearchResourceDisplayName("Account.PasswordRecovery.NewPassword")]
        [Display(Name = "รหัสผ่าน")]
        public string NewPassword { get; set; }
        
        //[NoTrim]
        [DataType(DataType.Password)]
        [Display(Name = "ยืนยันรหัสผ่าน")]
        //[ResearchResourceDisplayName("Account.PasswordRecovery.ConfirmNewPassword")]
        public string ConfirmNewPassword { get; set; }

        public bool DisablePasswordChanging { get; set; }
        public string Result { get; set; }
    }
}