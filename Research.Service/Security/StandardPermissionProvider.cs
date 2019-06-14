using System.Collections.Generic;
using Research.Core.Domain.Users;
using Research.Core.Domain.Security;
using Research.Data;

namespace Research.Services.Security
{
    /// <summary>
    /// Standard permission provider
    /// </summary>
    public partial class StandardPermissionProvider : IPermissionProvider
    {
        //admin area permissions
        public static readonly PermissionRecord AccessAdminPanel = new PermissionRecord { Name = "Access admin area", SystemName = "AccessAdminPanel", Category = "Standard" };
        public static readonly PermissionRecord AllowUserImpersonation = new PermissionRecord { Name = "Allow User Impersonation", SystemName = "AllowUserImpersonation", Category = "Users" };
        public static readonly PermissionRecord ManageMessageTemplates = new PermissionRecord { Name = "Manage Message Templates", SystemName = "ManageMessageTemplates", Category = "Content Management" };
        public static readonly PermissionRecord ManageSettings = new PermissionRecord { Name = "Manage Settings", SystemName = "ManageSettings", Category = "Configuration" };
        public static readonly PermissionRecord ManageActivityLog = new PermissionRecord { Name = "Manage Activity Log", SystemName = "ManageActivityLog", Category = "Configuration" };
        public static readonly PermissionRecord ManageEmailAccounts = new PermissionRecord { Name = "Manage Email Accounts", SystemName = "ManageEmailAccounts", Category = "Configuration" };
        public static readonly PermissionRecord ManageSystemLog = new PermissionRecord { Name = "Manage System Log", SystemName = "ManageSystemLog", Category = "Configuration" };
        public static readonly PermissionRecord ManageMessageQueue = new PermissionRecord { Name = "Manage Message Queue", SystemName = "ManageMessageQueue", Category = "Configuration" };
        public static readonly PermissionRecord ManageMaintenance = new PermissionRecord { Name = "Manage Maintenance", SystemName = "ManageMaintenance", Category = "Configuration" };
        public static readonly PermissionRecord HtmlEditorManagePictures = new PermissionRecord { Name = "HTML Editor. Manage pictures", SystemName = "HtmlEditor.ManagePictures", Category = "Configuration" };
        public static readonly PermissionRecord ManageScheduleTasks = new PermissionRecord { Name = "Manage Schedule Tasks", SystemName = "ManageScheduleTasks", Category = "Configuration" };
        public static readonly PermissionRecord ManageUsers = new PermissionRecord { Name = "Manage Users", SystemName = "ManageUsers", Category = "Configuration" };

        //public store permissions
        public static readonly PermissionRecord PublicStoreAllowNavigation = new PermissionRecord { Name = "Public web application. Allow navigation", SystemName = "PublicStoreAllowNavigation", Category = "PublicStore" };
        public static readonly PermissionRecord AccessClosedStore = new PermissionRecord { Name = "Public web application. Access a closed application", SystemName = "AccessClosedStore", Category = "PublicStore" };
        public static readonly PermissionRecord ManageAcl;

        /// <summary>
        /// Get permissions
        /// </summary>
        /// <returns>Permissions</returns>
        public virtual IEnumerable<PermissionRecord> GetPermissions()
        {
            return new[]
            {
                AccessAdminPanel,
                AllowUserImpersonation,
                ManageMessageTemplates,
                ManageSettings,
                ManageActivityLog,
                ManageEmailAccounts,
                ManageSystemLog,
                ManageMessageQueue,
                ManageMaintenance,
                HtmlEditorManagePictures,
                ManageScheduleTasks,
                PublicStoreAllowNavigation,
                AccessClosedStore
            };
        }

        /// <summary>
        /// Get default permissions
        /// </summary>
        /// <returns>Permissions</returns>
        public virtual IEnumerable<DefaultPermissionRecord> GetDefaultPermissions()
        {
            return new[]
            {
                new DefaultPermissionRecord
                {
                    UserRoleSystemName = ResearchUserDefaults.AdministratorsRoleName,
                    PermissionRecords = new[]
                    {
                        AccessAdminPanel,
                        AllowUserImpersonation,
                        ManageMessageTemplates,
                        ManageSettings,
                        ManageActivityLog,
                        ManageEmailAccounts,
                        ManageSystemLog,
                        ManageMessageQueue,
                        ManageMaintenance,
                        HtmlEditorManagePictures,
                        ManageScheduleTasks,
                        PublicStoreAllowNavigation,
                        AccessClosedStore
                    }
                },
                new DefaultPermissionRecord
                {
                    UserRoleSystemName = ResearchUserDefaults.ResearchCoordinatorsRoleName,
                    PermissionRecords = new[]
                    {
                        PublicStoreAllowNavigation
                    }
                },
                new DefaultPermissionRecord
                {
                    UserRoleSystemName = ResearchUserDefaults.GuestsRoleName,
                    PermissionRecords = new[]
                    {
                        PublicStoreAllowNavigation
                    }
                },
                new DefaultPermissionRecord
                {
                    UserRoleSystemName = ResearchUserDefaults.ResearchDevelopmentInstituteStaffsRoleName,
                    PermissionRecords = new[]
                    {
                        PublicStoreAllowNavigation
                    }
                },
                new DefaultPermissionRecord
                {
                    UserRoleSystemName = ResearchUserDefaults.ResearchersRoleName,
                    PermissionRecords = new[]
                    {
                        AccessAdminPanel,
                    }
                }
            };
        }
    }
}