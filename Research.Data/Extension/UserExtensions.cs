using Research.Core.Domain;
using Research.Core.Domain.Users;
using Research.Enum;
using System;
using System.Linq;

namespace Research.Data
{
    /// <summary>
    /// User extensions
    /// </summary>
    public static class UserExtensions
    {
        #region User role

        /// <summary>
        /// Gets a value indicating whether user is in a certain user role
        /// </summary>
        /// <param name="user">User</param>
        /// <param name="userRoleSystem">User role system name</param>
        /// <param name="onlyActiveUserRoles">A value indicating whether we should look only in active user roles</param>
        /// <returns>Result</returns>
        public static bool IsInUserRole(this User user,
            string userRoleSystemName, bool onlyActiveUserRoles = true)
        {
            if (user == null)
                throw new ArgumentNullException(nameof(user));

            if (string.IsNullOrEmpty(userRoleSystemName))
                throw new ArgumentNullException(nameof(userRoleSystemName));

            var result = user.UserRoles
                .FirstOrDefault(cr => (!onlyActiveUserRoles || cr.IsActive) && cr.SystemName == userRoleSystemName) != null;
            return result;
        }

        /// <summary>
        /// Gets a value indicating whether user a search engine
        /// </summary>
        /// <param name="user">User</param>
        /// <returns>Result</returns>
        public static bool IsSearchEngineAccount(this User user)
        {
            if (user == null)
                throw new ArgumentNullException(nameof(user));

            if (!user.IsSystemAccount || string.IsNullOrEmpty(user.SystemName))
                return false;

            var result = user.SystemName.Equals(ResearchUserDefaults.SearchEngineUserName, StringComparison.InvariantCultureIgnoreCase);
            return result;
        }

        /// <summary>
        /// Gets a value indicating whether the user is a built-in record for background tasks
        /// </summary>
        /// <param name="user">User</param>
        /// <returns>Result</returns>
        public static bool IsBackgroundTaskAccount(this User user)
        {
            if (user == null)
                throw new ArgumentNullException(nameof(user));

            if (!user.IsSystemAccount || string.IsNullOrEmpty(user.SystemName))
                return false;

            var result = user.SystemName.Equals(ResearchUserDefaults.BackgroundTaskUserName, StringComparison.InvariantCultureIgnoreCase);
            return result;
        }

        /// <summary>
        /// Gets a value indicating whether user is administrator
        /// </summary>
        /// <param name="user">User</param>
        /// <param name="onlyActiveUserRoles">A value indicating whether we should look only in active user roles</param>
        /// <returns>Result</returns>
        public static bool IsAdmin(this User user, bool onlyActiveUserRoles = true)
        {
            return IsInUserRole(user, ResearchUserDefaults.AdministratorsRoleName, onlyActiveUserRoles);
        }

        /// <summary>
        /// Gets a value indicating whether user is a forum moderator
        /// </summary>
        /// <param name="user">User</param>
        /// <param name="onlyActiveUserRoles">A value indicating whether we should look only in active user roles</param>
        /// <returns>Result</returns>
        public static bool IsResearchCoordinator(this User user, bool onlyActiveUserRoles = true)
        {
            return IsInUserRole(user, ResearchUserDefaults.ResearchCoordinatorsRoleName, onlyActiveUserRoles);
        }

        /// <summary>
        /// Gets a value indicating whether user is registered
        /// </summary>
        /// <param name="user">User</param>
        /// <param name="onlyActiveUserRoles">A value indicating whether we should look only in active user roles</param>
        /// <returns>Result</returns>
        public static bool IsRegistered(this User user, bool onlyActiveUserRoles = true)
        {
            return IsInUserRole(user, ResearchUserDefaults.ResearchersRoleName, onlyActiveUserRoles);
        }

        /// <summary>
        /// Gets a value indicating whether user is guest
        /// </summary>
        /// <param name="user">User</param>
        /// <param name="onlyActiveUserRoles">A value indicating whether we should look only in active user roles</param>
        /// <returns>Result</returns>
        public static bool IsGuest(this User user, bool onlyActiveUserRoles = true)
        {
            return IsInUserRole(user, ResearchUserDefaults.GuestsRoleName, onlyActiveUserRoles);
        }

        /// <summary>
        /// Gets a value indicating whether user is researcher
        /// </summary>
        /// <param name="user">User</param>
        /// <param name="onlyActiveUserRoles">A value indicating whether we should look only in active user roles</param>
        /// <returns>Result</returns>
        public static bool IsResearcher(this User user, bool onlyActiveUserRoles = true)
        {
            return IsInUserRole(user, ResearchUserDefaults.ResearchersRoleName, onlyActiveUserRoles);
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
        #endregion
    }
}
