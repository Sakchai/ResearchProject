using Research.Data;
using System.Collections.Generic;

namespace Research.Services.Roles
{
    /// <summary>
    /// Message template service
    /// </summary>
    public partial interface IRoleService
    {
        /// <summary>
        /// Delete a message template
        /// </summary>
        /// <param name="role">Message template</param>
        void DeleteRole(Role role);

        /// <summary>
        /// Inserts a message template
        /// </summary>
        /// <param name="role">Message template</param>
        void InsertRole(Role role);

        /// <summary>
        /// Updates a message template
        /// </summary>
        /// <param name="role">Message template</param>
        void UpdateRole(Role role);

        /// <summary>
        /// Gets a message template by identifier
        /// </summary>
        /// <param name="roleId">Message template identifier</param>
        /// <returns>Message template</returns>
        Role GetRoleById(int roleId);

        /// <summary>
        /// Gets message templates by the name
        /// </summary>
        /// <param name="roleName">Message template name</param>
        /// <param name="storeId">Store identifier; pass null to load all records</param>
        /// <returns>List of message templates</returns>
        IList<Role> GetRolesByName(string roleName, int? storeId = null);

        /// <summary>
        /// Gets all message templates
        /// </summary>
        /// <param name="storeId">Store identifier; pass 0 to load all records</param>
        /// <returns>Message template list</returns>
        IList<Role> GetAllRoles(int storeId);

        /// <summary>
        /// Create a copy of message template with all depended data
        /// </summary>
        /// <param name="role">Message template</param>
        /// <returns>Message template copy</returns>
        Role CopyRole(Role role);
    }
}
