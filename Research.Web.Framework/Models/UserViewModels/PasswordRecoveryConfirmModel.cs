using System.ComponentModel.DataAnnotations;
using Research.Web.Framework.Models;

namespace Research.Web.Models.Users
{
    //[Validator(typeof(PasswordRecoveryConfirmValidator))]
    public partial class PasswordRecoveryConfirmModel : BaseResearchModel
    {
        [DataType(DataType.Password)]
        //[NoTrim]
        //[ResearchResourceDisplayName("Account.PasswordRecovery.NewPassword")]
        public string NewPassword { get; set; }
        
        //[NoTrim]
        [DataType(DataType.Password)]
        //[ResearchResourceDisplayName("Account.PasswordRecovery.ConfirmNewPassword")]
        public string ConfirmNewPassword { get; set; }

        public bool DisablePasswordChanging { get; set; }
        public string Result { get; set; }
    }
}