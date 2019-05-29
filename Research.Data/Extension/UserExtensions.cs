using Research.Core.Domain;
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
            int userRoleSystem, bool onlyActiveUserRoles = true)
        {
            if (user == null)
                throw new ArgumentNullException(nameof(user));

            if (userRoleSystem == 0)
                throw new ArgumentNullException(nameof(userRoleSystem));
            //chai

            //var result = user.UserRoles
            //    .FirstOrDefault(cr => (!onlyActiveUserRoles || cr.Role.IsActive) && cr.Role.RoleName == userRoleSystemName) != null;
            //var result = user.UserRoles.FirstOrDefault() != null;
            var result = user.UserRoles
                .FirstOrDefault(cr => (!onlyActiveUserRoles ||  cr.IsActive) && cr.RoleId == userRoleSystem) != null;

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
            return IsInUserRole(user, ResearchUserDefaults.AdministratorsRoleId, onlyActiveUserRoles);
        }

        /// <summary>
        /// Gets a value indicating whether user is a forum moderator
        /// </summary>
        /// <param name="user">User</param>
        /// <param name="onlyActiveUserRoles">A value indicating whether we should look only in active user roles</param>
        /// <returns>Result</returns>
        public static bool IsResearchCoordinator(this User user, bool onlyActiveUserRoles = true)
        {
            return IsInUserRole(user, ResearchUserDefaults.ResearchCoordinatorRoleId, onlyActiveUserRoles);
        }

        /// <summary>
        /// Gets a value indicating whether user is registered
        /// </summary>
        /// <param name="user">User</param>
        /// <param name="onlyActiveUserRoles">A value indicating whether we should look only in active user roles</param>
        /// <returns>Result</returns>
        public static bool IsRegistered(this User user, bool onlyActiveUserRoles = true)
        {
            return IsInUserRole(user, ResearchUserDefaults.ResearcherRoleId, onlyActiveUserRoles);
        }

        /// <summary>
        /// Gets a value indicating whether user is guest
        /// </summary>
        /// <param name="user">User</param>
        /// <param name="onlyActiveUserRoles">A value indicating whether we should look only in active user roles</param>
        /// <returns>Result</returns>
        public static bool IsGuest(this User user, bool onlyActiveUserRoles = true)
        {
            return IsInUserRole(user, ResearchUserDefaults.GuestsRoleId, onlyActiveUserRoles);
        }

        /// <summary>
        /// Gets a value indicating whether user is researcher
        /// </summary>
        /// <param name="user">User</param>
        /// <param name="onlyActiveUserRoles">A value indicating whether we should look only in active user roles</param>
        /// <returns>Result</returns>
        public static bool IsResearcher(this User user, bool onlyActiveUserRoles = true)
        {
            return IsInUserRole(user, ResearchUserDefaults.ResearcherRoleId, onlyActiveUserRoles);
        }

        #endregion
    }
}
