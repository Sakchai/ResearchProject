using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Research.Web.Framework.Localization;
using Research.Web.Framework.Mvc.Routing;

namespace Research.Web.Infrastructure
{
    /// <summary>
    /// Represents provider that provided basic routes
    /// </summary>
    public partial class RouteProvider : IRouteProvider
    {
        #region Methods

        /// <summary>
        /// Register routes
        /// </summary>
        /// <param name="routeBuilder">Route builder</param>
        public void RegisterRoutes(IRouteBuilder routeBuilder)
        {
            //reorder routes so the most used ones are on top. It can improve performance
            //routeBuilder.MapRoute("Default", "{controller}/{action}/{id?}");

            //routeBuilder.MapRoute(name: "areaRoute", template: "/{controller=Home}/{action=Index}/{id?}");


            //        //home page
            //        routeBuilder.MapRoute("HomePage", "",
            //new { controller = "User", action = "Login" });

            //login
    //        routeBuilder.MapRoute("Login", "Login",
				//new { controller = "User", action = "Login" });

            //register
    //        routeBuilder.MapRoute("Register", "Register",
				//new { controller = "User", action = "Register" });

    //        //logout
    //        routeBuilder.MapRoute("Logout", "Logout",
				//new { controller = "User", action = "Logout" });

    //        //login page for checkout as guest
    //        routeBuilder.MapRoute("LoginCheckoutAsGuest", "login/checkoutasguest",
				//new { controller = "User", action = "Login", checkoutAsGuest = true });

            //register result page
    //        routeBuilder.MapRoute("RegisterResult", "registerresult/{resultId:min(0)}",
				//new { controller = "User", action = "RegisterResult" });

            //check username availability
    //        routeBuilder.MapRoute("CheckUsernameAvailability", "user/checkusernameavailability",
				//new { controller = "User", action = "CheckUsernameAvailability" });

            //passwordrecovery
            routeBuilder.MapRoute("PasswordRecovery", "PasswordRecovery",
				new { controller = "User", action = "PasswordRecovery" });

            routeBuilder.MapRoute("PasswordRecoverySend", "PasswordRecoverySend",
                new { controller = "User", action = "PasswordRecoverySend" });

            //password recovery confirmation
            routeBuilder.MapRoute("PasswordRecoveryConfirm", "PasswordRecoveryConfirm",
				new { controller = "User", action = "PasswordRecoveryConfirm" });


    //        routeBuilder.MapRoute("UserChangePassword", "user/changepassword",
				//new { controller = "User", action = "ChangePassword" });

    //        routeBuilder.MapRoute("UserAvatar", "user/avatar",
				//new { controller = "User", action = "Avatar" });

    //        routeBuilder.MapRoute("AccountActivation", "user/activation",
				//new { controller = "User", action = "AccountActivation" });

    //        routeBuilder.MapRoute("EmailRevalidation", "user/revalidateemail",
				//new { controller = "User", action = "EmailRevalidation" });

            //store closed
            routeBuilder.MapRoute("StoreClosed", "storeclosed",
				new { controller = "Common", action = "StoreClosed" });

            //error page
            routeBuilder.MapRoute("Error", "error",
                new { controller = "Common", action = "Error" });

            //page not found
            routeBuilder.MapRoute("PageNotFound", "page-not-found", 
                new { controller = "Common", action = "PageNotFound" });
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets a priority of route provider
        /// </summary>
        public int Priority
        {
            get { return 0; }
        }

        #endregion
    }
}
