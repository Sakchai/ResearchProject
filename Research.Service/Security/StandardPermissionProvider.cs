using Research.Data;
using System.Collections.Generic;

namespace Research.Services.Security
{
    /// <summary>
    /// Standard permission provider
    /// </summary>
    public partial class StandardPermissionProvider : IPermissionProvider
    {
        public static readonly UserRole AccessAdminPanel = new UserRole { RoleId = 1, UserId = 1 };
        IEnumerable<UserRole> IPermissionProvider.GetPermissions()
        {
            throw new System.NotImplementedException();
        }

        IEnumerable<UserRole> IPermissionProvider.GetDefaultPermissions()
        {
            throw new System.NotImplementedException();
        }
    }
}