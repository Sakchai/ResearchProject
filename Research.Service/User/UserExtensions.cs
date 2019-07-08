using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Xml;
using Research.Core;
using Research.Core.Caching;
using Research.Core.Domain.Users;
using Research.Services.Common;
using Research.Data;
using Research.Infrastructure;

namespace Research.Services.Users
{
    /// <summary>
    /// User extensions
    /// </summary>
    public static class UserExtensions
    {
        /// <summary>
        /// Get full name
        /// </summary>
        /// <param name="user">User</param>
        /// <returns>User full name</returns>
        public static string GetFullName(this User user)
        {
            if (user == null)
                throw new ArgumentNullException(nameof(user));
            var firstName = user.GetAttribute<string>(ResearchUserDefaults.FirstNameAttribute);
            var lastName = user.GetAttribute<string>(ResearchUserDefaults.LastNameAttribute);

            var fullName = "";
            if (!string.IsNullOrWhiteSpace(firstName) && !string.IsNullOrWhiteSpace(lastName))
                fullName = $"{firstName} {lastName}";
            else
            {
                if (!string.IsNullOrWhiteSpace(firstName))
                    fullName = firstName;

                if (!string.IsNullOrWhiteSpace(lastName))
                    fullName = lastName;
            }
            return fullName;
        }

        ///// <summary>
        ///// Formats the user name
        ///// </summary>
        ///// <param name="user">Source</param>
        ///// <param name="stripTooLong">Strip too long user name</param>
        ///// <param name="maxLength">Maximum user name length</param>
        ///// <returns>Formatted text</returns>
        //public static string FormatUserName(this User user, bool stripTooLong = false, int maxLength = 0)
        //{
        //    if (user == null)
        //        return string.Empty;

        //    if (user.IsGuest())
        //    {
        //        return EngineContext.Current.Resolve<ILocalizationService>().GetResource("User.Guest");
        //    }

        //    var result = string.Empty;
        //    switch (EngineContext.Current.Resolve<UserSettings>().UserNameFormat)
        //    {
        //        case UserNameFormat.ShowEmails:
        //            result = user.Email;
        //            break;
        //        case UserNameFormat.ShowUsernames:
        //            result = user.Username;
        //            break;
        //        case UserNameFormat.ShowFullNames:
        //            result = user.GetFullName();
        //            break;
        //        case UserNameFormat.ShowFirstName:
        //            result = user.GetAttribute<string>(ResearchUserDefaults.FirstNameAttribute);
        //            break;
        //        default:
        //            break;
        //    }

        //    if (stripTooLong && maxLength > 0)
        //    {
        //        result = CommonHelper.EnsureMaximumLength(result, maxLength);
        //    }

        //    return result;
        //}

        /// <summary>
        /// Gets coupon codes
        /// </summary>
        /// <param name="user">User</param>
        /// <returns>Coupon codes</returns>
        public static string[] ParseAppliedDiscountCouponCodes(this User user)
        {
            if (user == null)
                throw new ArgumentNullException(nameof(user));

            var genericAttributeService = EngineContext.Current.Resolve<IGenericAttributeService>();
            var existingCouponCodes = user.GetAttribute<string>(ResearchUserDefaults.DiscountCouponCodeAttribute,
                genericAttributeService);

            var couponCodes = new List<string>();
            if (string.IsNullOrEmpty(existingCouponCodes))
                return couponCodes.ToArray();

            try
            {
                var xmlDoc = new XmlDocument();
                xmlDoc.LoadXml(existingCouponCodes);

                var nodeList1 = xmlDoc.SelectNodes(@"//DiscountCouponCodes/CouponCode");
                foreach (XmlNode node1 in nodeList1)
                {
                    if (node1.Attributes != null && node1.Attributes["Code"] != null)
                    {
                        var code = node1.Attributes["Code"].InnerText.Trim();
                        couponCodes.Add(code);
                    }
                }
            }
            catch (Exception exc)
            {
                Debug.Write(exc.ToString());
            }
            return couponCodes.ToArray();
        }


        /// <summary>
        /// Gets coupon codes
        /// </summary>
        /// <param name="user">User</param>
        /// <returns>Coupon codes</returns>
        public static string[] ParseAppliedGiftCardCouponCodes(this User user)
        {
            if (user == null)
                throw new ArgumentNullException(nameof(user));

            var genericAttributeService = EngineContext.Current.Resolve<IGenericAttributeService>();
            var existingCouponCodes = user.GetAttribute<string>(ResearchUserDefaults.GiftCardCouponCodesAttribute,
                genericAttributeService);

            var couponCodes = new List<string>();
            if (string.IsNullOrEmpty(existingCouponCodes))
                return couponCodes.ToArray();

            try
            {
                var xmlDoc = new XmlDocument();
                xmlDoc.LoadXml(existingCouponCodes);

                var nodeList1 = xmlDoc.SelectNodes(@"//GiftCardCouponCodes/CouponCode");
                foreach (XmlNode node1 in nodeList1)
                {
                    if (node1.Attributes != null && node1.Attributes["Code"] != null)
                    {
                        var code = node1.Attributes["Code"].InnerText.Trim();
                        couponCodes.Add(code);
                    }
                }
            }
            catch (Exception exc)
            {
                Debug.Write(exc.ToString());
            }
            return couponCodes.ToArray();
        }

        /// <summary>
        /// Adds a coupon code
        /// </summary>
        /// <param name="user">User</param>
        /// <param name="couponCode">Coupon code</param>
        /// <returns>New coupon codes document</returns>
        public static void ApplyGiftCardCouponCode(this User user, string couponCode)
        {
            if (user == null)
                throw new ArgumentNullException(nameof(user));

            var genericAttributeService = EngineContext.Current.Resolve<IGenericAttributeService>();
            var result = string.Empty;
            try
            {
                var existingCouponCodes = user.GetAttribute<string>(ResearchUserDefaults.GiftCardCouponCodesAttribute,
                    genericAttributeService);

                couponCode = couponCode.Trim().ToLower();

                var xmlDoc = new XmlDocument();
                if (string.IsNullOrEmpty(existingCouponCodes))
                {
                    var element1 = xmlDoc.CreateElement("GiftCardCouponCodes");
                    xmlDoc.AppendChild(element1);
                }
                else
                {
                    xmlDoc.LoadXml(existingCouponCodes);
                }
                var rootElement = (XmlElement)xmlDoc.SelectSingleNode(@"//GiftCardCouponCodes");

                XmlElement gcElement = null;
                //find existing
                var nodeList1 = xmlDoc.SelectNodes(@"//GiftCardCouponCodes/CouponCode");
                foreach (XmlNode node1 in nodeList1)
                {
                    if (node1.Attributes != null && node1.Attributes["Code"] != null)
                    {
                        var couponCodeAttribute = node1.Attributes["Code"].InnerText.Trim();
                        if (couponCodeAttribute.ToLower() == couponCode.ToLower())
                        {
                            gcElement = (XmlElement)node1;
                            break;
                        }
                    }
                }

                //create new one if not found
                if (gcElement == null)
                {
                    gcElement = xmlDoc.CreateElement("CouponCode");
                    gcElement.SetAttribute("Code", couponCode);
                    rootElement.AppendChild(gcElement);
                }

                result = xmlDoc.OuterXml;
            }
            catch (Exception exc)
            {
                Debug.Write(exc.ToString());
            }

            //apply new value
            genericAttributeService.SaveAttribute(user, ResearchUserDefaults.GiftCardCouponCodesAttribute, result);
        }

        /// <summary>
        /// Removes a coupon code
        /// </summary>
        /// <param name="user">User</param>
        /// <param name="couponCode">Coupon code to remove</param>
        /// <returns>New coupon codes document</returns>
        public static void RemoveGiftCardCouponCode(this User user, string couponCode)
        {
            if (user == null)
                throw new ArgumentNullException(nameof(user));

            //get applied coupon codes
            var existingCouponCodes = user.ParseAppliedGiftCardCouponCodes();

            //clear them
            var genericAttributeService = EngineContext.Current.Resolve<IGenericAttributeService>();
            genericAttributeService.SaveAttribute<string>(user, ResearchUserDefaults.GiftCardCouponCodesAttribute, null);

            //save again except removed one
            foreach (string existingCouponCode in existingCouponCodes)
                if (!existingCouponCode.Equals(couponCode, StringComparison.InvariantCultureIgnoreCase))
                    user.ApplyGiftCardCouponCode(existingCouponCode);
        }

        /// <summary>
        /// Check whether password recovery token is valid
        /// </summary>
        /// <param name="user">User</param>
        /// <param name="token">Token to validate</param>
        /// <returns>Result</returns>
        public static bool IsPasswordRecoveryTokenValid(this User user, string token)
        {
            if (user == null)
                throw new ArgumentNullException(nameof(user));

            var cPrt = user.GetAttribute<string>(ResearchUserDefaults.PasswordRecoveryTokenAttribute);
            if (string.IsNullOrEmpty(cPrt))
                return false;

            if (!cPrt.Equals(token, StringComparison.InvariantCultureIgnoreCase))
                return false;

            return true;
        }

        /// <summary>
        /// Check whether password recovery link is expired
        /// </summary>
        /// <param name="user">User</param>
        /// <param name="userSettings">User settings</param>
        /// <returns>Result</returns>
        public static bool IsPasswordRecoveryLinkExpired(this User user, UserSettings userSettings)
        {
            if (user == null)
                throw new ArgumentNullException(nameof(user));

            if (userSettings == null)
                throw new ArgumentNullException(nameof(userSettings));

            if (userSettings.PasswordRecoveryLinkDaysValid == 0)
                return false;
            
            var geneatedDate = user.GetAttribute<DateTime?>(ResearchUserDefaults.PasswordRecoveryTokenDateGeneratedAttribute);
            if (!geneatedDate.HasValue)
                return false;

            var daysPassed = (DateTime.UtcNow - geneatedDate.Value).TotalDays;
            if (daysPassed > userSettings.PasswordRecoveryLinkDaysValid)
                return true;

            return false;
        }

        /// <summary>
        /// Get user role identifiers
        /// </summary>
        /// <param name="user">User</param>
        /// <param name="showHidden">A value indicating whether to load hidden records</param>
        /// <returns>User role identifiers</returns>
        public static int[] GetUserRoleIds(this User user, bool showHidden = false)
        {
            if (user == null)
                throw new ArgumentNullException(nameof(user));

            var userRolesIds = user.UserRoles
               .Where(cr => showHidden || cr.IsActive)
               .Select(cr => cr.Id)
               .ToArray();

            return userRolesIds;
        }

        /// <summary>
        /// Check whether user password is expired 
        /// </summary>
        /// <param name="user">User</param>
        /// <returns>True if password is expired; otherwise false</returns>
        public static bool PasswordIsExpired(this User user)
        {
            if (user == null)
                throw new ArgumentNullException(nameof(user));

            //the guests don't have a password
            if (user.IsGuest())
                return false;

            //password lifetime is disabled for user
            if (!user.UserRoles.Any(role => role.IsActive && role.EnablePasswordLifetime))
                return false;

            //setting disabled for all
            var userSettings = EngineContext.Current.Resolve<UserSettings>();
            if (userSettings.PasswordLifetime == 0)
                return false;
            
            //cache result between HTTP requests 
            var cacheManager = EngineContext.Current.Resolve<IStaticCacheManager>();
            var cacheKey = string.Format(ResearchUserServiceDefaults.UserPasswordLifetimeCacheKey, user.Id);

            //get current password usage time
            var currentLifetime = cacheManager.Get(cacheKey, () =>
            {
                var userPassword = EngineContext.Current.Resolve<IUserService>().GetCurrentPassword(user.Id);
                //password is not found, so return max value to force user to change password
                if (userPassword == null)
                    return int.MaxValue;

                return (DateTime.UtcNow - userPassword.CreatedOnUtc).Days;
            });

            return currentLifetime >= userSettings.PasswordLifetime;
        }
    }
}
