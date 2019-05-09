﻿using Microsoft.AspNetCore.Mvc;

namespace Research.Web.Framework.Extensions
{
    /// <summary>
    /// IUrlHelper extensions 
    /// </summary>
    public static class UrlHelperExtensions
    {
        /// <summary>
        /// Get login page URL
        /// </summary>
        /// <param name="urlHelper">IUrlHelper</param>
        /// <param name="returnUrl">Return URL</param>
        /// <returns>Login page URL</returns>
        public static string LogOn(this IUrlHelper urlHelper, string returnUrl)
        {
            if (!string.IsNullOrEmpty(returnUrl))
                return urlHelper.Action("Login", "User", new { ReturnUrl = returnUrl });
            return urlHelper.Action("Login", "User");
        }

        /// <summary>
        /// Get logout page URL
        /// </summary>
        /// <param name="urlHelper">IUrlHelper</param>
        /// <param name="returnUrl">Return URL</param>
        /// <returns>Logout page URL</returns>
        public static string LogOff(this IUrlHelper urlHelper, string returnUrl)
        {
            if (!string.IsNullOrEmpty(returnUrl))
                return urlHelper.Action("Logout", "User", new { ReturnUrl = returnUrl });
            return urlHelper.Action("Logout", "User");
        }
    }
}