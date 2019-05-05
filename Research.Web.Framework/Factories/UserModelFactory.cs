using System;
using Microsoft.AspNetCore.Mvc;
using Research.Core.Domain.Users;
using Research.Enum;
using Research.Web.Models.Factories;
using Research.Web.Models.Users;

namespace Research.Web.Factories
{
    /// <summary>
    /// Represents the customer model factory implementation
    /// </summary>
    public partial class UserModelFactory : IUserModelFactory
    {
        private readonly UserSettings _userSettings;
        private readonly IBaseAdminModelFactory _baseAdminModelFactory;

        public UserModelFactory(UserSettings userSettings,
            IBaseAdminModelFactory baseAdminModelFactory)
        {
            this._userSettings = userSettings;
            this._baseAdminModelFactory = baseAdminModelFactory;
        }

        /// <summary>
        /// Prepare the login model
        /// </summary>
        /// <param name="checkoutAsGuest">Whether to checkout as guest is enabled</param>
        /// <returns>Login model</returns>
        public virtual LoginModel PrepareLoginModel(bool? checkoutAsGuest)
        {
            var model = new LoginModel
            {
                CheckoutAsGuest = checkoutAsGuest.GetValueOrDefault(),
            };
            return model;
        }


 
        public RegisterModel PrepareRegisterModel(RegisterModel model)
        {
            _baseAdminModelFactory.PrepareTitles(model.AvailableTitles,true, "--โปรดระบุคำนำ--");
            _baseAdminModelFactory.PrepareAgencies(model.AvailableAgencies, true, "--หน่วยงาน--");
            return model;
        }

        /// <summary>
        /// Prepare the register result model
        /// </summary>
        /// <param name="resultId">Value of UserRegistrationType enum</param>
        /// <returns>Register result model</returns>
        public RegisterResultModel PrepareRegisterResultModel(int resultId)
        {
            var resultText = "";
            switch ((UserRegistrationType)resultId)
            {
                case UserRegistrationType.Disabled:
                    resultText = "Account Disabled";
                    break;
                case UserRegistrationType.Standard:
                    resultText = "Account Standard";
                    break;
                case UserRegistrationType.AdminApproval:
                    resultText = "Account AdminApproval";
                    break;
                case UserRegistrationType.EmailValidation:
                    resultText = "Account EmailValidation";
                    break;
                default:
                    break;
            }
            var model = new RegisterResultModel
            {
                Result = resultText
            };
            return model;
        }

        public UserAgreementModel PrepareUserAgreementModel(Guid userGuid)
        {
            var model = new UserAgreementModel
            {
                UserGuid = userGuid
            };
            return model;
        }
    }
}