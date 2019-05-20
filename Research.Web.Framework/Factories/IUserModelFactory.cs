
using System;
using Research.Data;
using Research.Web.Models.Users;

namespace Research.Web.Factories
{
    /// <summary>
    /// Represents the user model factory
    /// </summary>
    public partial interface IUserModelFactory
    {

        /// <summary>
        /// Prepare the login model
        /// </summary>
        /// <param name="checkoutAsGuest">Whether to checkout as guest is enabled</param>
        /// <returns>Login model</returns>
        LoginModel PrepareLoginModel(bool? checkoutAsGuest);

        /// <summary>
        /// Prepare the User register model
        /// </summary>
        /// <param name="model">User register model</param>
        /// <param name="excludeProperties">Whether to exclude populating of model properties from the entity</param>
        /// <param name="overrideCustomUserAttributesXml">Overridden customer attributes in XML format; pass null to use CustomCustomerAttributes of customer</param>
        /// <param name="setDefaultValues">Whether to populate model properties by default values</param>
        /// <returns>Customer register model</returns>
        RegisterModel PrepareRegisterModel(RegisterModel model);
        RegisterResultModel PrepareRegisterResultModel(int resultId);
        UserAgreementModel PrepareUserAgreementModel(Guid userGuid);

        /// <summary>
        /// Prepare user search model
        /// </summary>
        /// <param name="model">User search model</param>
        /// <returns>User search model</returns>
        UserSearchModel PrepareUserSearchModel(UserSearchModel searchModel);

        /// <summary>
        /// Prepare paged researcher list model
        /// </summary>
        /// <param name="searchModel">User search model</param>
        /// <returns>User list model</returns>
        UserListModel PrepareUserListModel(UserSearchModel searchModel);

        /// <summary>
        /// Prepare researcher model
        /// </summary>
        /// <param name="model">User model</param>
        /// <param name="user">User</param>
        /// <param name="excludeProperties">Whether to exclude populating of some properties of model</param>
        /// <returns>User model</returns>
        UserModel PrepareUserModel(UserModel model, User user, bool excludeProperties = false);

    }
}