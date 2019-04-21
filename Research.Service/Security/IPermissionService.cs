using Research.Data;
using System.Collections.Generic;

namespace Research.Services.Security
{
    /// <summary>
    /// Permission service interface
    /// </summary>
    public partial interface IPermissionService
    {
        /// <summary>
        /// Delete a userRole
        /// </summary>
        /// <param name="userRole">Permission</param>
        void DeleteUserRole(UserRole userRole);

        /// <summary>
        /// Gets a userRole
        /// </summary>
        /// <param name="roleId">Permission identifier</param>
        /// <returns>Permission</returns>
        UserRole GetUserRoleById(int roleId);

        /// <summary>
        /// Gets a userRole
        /// </summary>
        /// <param name="roleName">Permission system name</param>
        /// <returns>Permission</returns>
        UserRole GetUserRoleByRoleName(string roleName);

        /// <summary>
        /// Gets all userRoles
        /// </summary>
        /// <returns>Permissions</returns>
        IList<UserRole> GetAllUserRoles();

        /// <summary>
        /// Inserts a userRole
        /// </summary>
        /// <param name="userRole">Permission</param>
        void InsertUserRole(UserRole userRole);

        /// <summary>
        /// Updates the userRole
        /// </summary>
        /// <param name="userRole">Permission</param>
        void UpdateUserRole(UserRole userRole);

        ///// <summary>
        ///// Install userRoles
        ///// </summary>
        ///// <param name="userRoleProvider">Permission provider</param>
        //void InstallPermissions(IPermissionProvider userRoleProvider);

        /// <summary>
        /// Uninstall userRoles
        /// </summary>
        /// <param name="userRoleProvider">Permission provider</param>
        void UninstallPermissions(IPermissionProvider userRoleProvider);

        /// <summary>
        /// Authorize userRole
        /// </summary>
        /// <param name="userRole">Permission record</param>
        /// <returns>true - authorized; otherwise, false</returns>
        bool Authorize(UserRole userRole);

        /// <summary>
        /// Authorize userRole
        /// </summary>
        /// <param name="userRole">Permission record</param>
        /// <param name="user">User</param>
        /// <returns>true - authorized; otherwise, false</returns>
        bool Authorize(UserRole userRole, User user);




        /// <summary>
        /// Authorize userRole
        /// </summary>
        /// <param name="userRoleRecordRoleName">Permission record system name</param>
        /// <returns>true - authorized; otherwise, false</returns>
        bool Authorize(string userRoleRecordRoleName);

        /// <summary>
        /// Authorize userRole
        /// </summary>
        /// <param name="userRoleRecordRoleName">Permission record system name</param>
        /// <param name="user">User</param>
        /// <returns>true - authorized; otherwise, false</returns>
        bool Authorize(string userRoleRecordRoleName, User user);

        /// <summary>
        /// GetRoleProgram by user Role
        /// </summary>
        /// <param name="program">Program</param>
        /// <param name="userRole">UserRole</param>
        /// <returns>RoleProgram</returns>
        RoleProgram GetRoleProgram(Program program, UserRole userRole);

        /// <summary>
        /// GetRolePrograms
        /// </summary>
        /// <param name="userRole">Permission record</param>
        /// <returns>List<RoleProgram> </returns>
        IList<RoleProgram> GetRolePrograms(UserRole userRole);
    }
}