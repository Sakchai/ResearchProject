
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


    }
}