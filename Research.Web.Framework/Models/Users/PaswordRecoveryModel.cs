using System.ComponentModel.DataAnnotations;
using FluentValidation.Attributes;
using Research.Web.Models;

namespace Research.Web.Models.Users
{
   // [Validator(typeof(PasswordRecoveryValidator))]
    public partial class PasswordRecoveryModel : BaseResearchModel
    {
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        public string Result { get; set; }
    }
}