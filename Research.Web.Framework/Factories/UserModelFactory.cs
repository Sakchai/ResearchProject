using Research.Core.Domain.Users;
using Research.Web.Models.Users;

namespace Research.Web.Factories
{
    /// <summary>
    /// Represents the customer model factory implementation
    /// </summary>
    public partial class UserModelFactory : IUserModelFactory
    {
        private readonly UserSettings _userSettings;

        public UserModelFactory(UserSettings userSettings)
        {
            _userSettings = userSettings;
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
    }
}