using System;
using System.Linq;
using System.Runtime.Serialization;
using Microsoft.AspNetCore.Mvc;
using Research.Core.Domain.Users;
using Research.Data;
using Research.Enum;
using Research.Services;
using Research.Web.Extensions;
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
        private readonly IUserService _userService;

        public UserModelFactory(UserSettings userSettings,
            IBaseAdminModelFactory baseAdminModelFactory,
            IUserService userService)
        {
            this._userSettings = userSettings;
            this._baseAdminModelFactory = baseAdminModelFactory;
            this._userService = userService;
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
            model.Gender = "M";
            _baseAdminModelFactory.PrepareTitles(model.AvailableTitles,true, "--ระบุคำนำ--");
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
                    resultText = "ระบบได้ลงทะเบียนผู้วิจัยเรียบร้อยแล้ว โปรดยืนยันอีเมลของท่าน ก่อนเข้าใช้ระบบงาน!";
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

        #region Methods

        /// <summary>
        /// Prepare user search model
        /// </summary>
        /// <param name="searchModel">User search model</param>
        /// <returns>User search model</returns>
        public virtual UserSearchModel PrepareUserSearchModel(UserSearchModel searchModel)
        {
            if (searchModel == null)
                throw new ArgumentNullException(nameof(searchModel));

            _baseAdminModelFactory.PrepareAgencies(searchModel.AvailableAgencyies, true, "--ระบุหน่วยงาน--");
            //prepare page parameters
            searchModel.SetGridPageSize();

            return searchModel;
        }

        /// <summary>
        /// Prepare paged user list model
        /// </summary>
        /// <param name="searchModel">User search model</param>
        /// <returns>User list model</returns>
        public virtual UserListModel PrepareUserListModel(UserSearchModel searchModel)
        {
            if (searchModel == null)
                throw new ArgumentNullException(nameof(searchModel));

            //get users
            var users = _userService.GetAllUsers(firstName: searchModel.FirstName,
                                                 lastName: searchModel.LastName,
                                                 agencyId: searchModel.AgencyId);


            //prepare grid model
            var model = new UserListModel
            {
                Data = users.PaginationByRequestModel(searchModel).Select(user =>
                {
                    //fill in model values from the entity
                    var userModel = user.ToModel<UserModel>();
                    string title = user.Title != null ? user.Title.TitleNameTH : string.Empty;
                    //little performance optimization: ensure that "Body" is not returned
                    userModel.Id = user.Id;
                    userModel.FullName = $"{title}{user.FirstName} {user.LastName}";
                    userModel.LastName = user.LastName;
                    userModel.MobileNumber = user.MobileNumber;
                    userModel.Email = user.Email;
                    userModel.AgencyName = user.Agency != null ? user.Agency.Name : string.Empty;
                    userModel.UserRoleName = _userService.GetUserRoleById(user.UserTypeId).Name;
                    return userModel;
                }),
                Total = users.Count
            };

            return model;
        }

        /// <summary>
        /// Prepare user model
        /// </summary>
        /// <param name="model">User model</param>
        /// <param name="user">User</param>
        /// <param name="excludeProperties">Whether to exclude populating of some properties of model</param>
        /// <returns>User model</returns>
        public virtual UserModel PrepareUserModel(UserModel model, User user, bool excludeProperties = false)
        {
            if (user != null)
            {
                //fill in model values from the entity
                model = model ?? user.ToModel<UserModel>();
                model.Id = user.Id;
                model.FirstName = user.FirstName;
                model.LastName = user.LastName;
                model.MobileNumber = user.MobileNumber;
                model.Email = user.Email;
                model.AgencyId = user.AgencyId;
            }
            else
            {
                model.UserName = _userService.GetNextNumber();
            }

            _baseAdminModelFactory.PrepareAgencies(model.AvailableAgencyies, true, "--ระบุหน่วยงาน--");
            _baseAdminModelFactory.PrepareTitles(model.AvailableTitles, true, "--ระบุคำนำหน้า--");
            _baseAdminModelFactory.PrepareUserRoles(model.AvailableUserRoles, true, "--ระบุระดับสิทธิ์--");

            return model;
        }


        #endregion
    }
}