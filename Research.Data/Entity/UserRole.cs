using System;
using System.Collections.Generic;

namespace Research.Data
{
    public partial class UserRole : BaseEntity
    {
        private ICollection<PermissionRecordUserRoleMapping> _permissionRecordUserRoleMappings;

        /// <summary>
        /// Gets or sets the customer role name
        /// </summary>
        public string Name { get; set; }

        public bool IsActive { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the customer role is system
        /// </summary>
        public bool IsSystemRole { get; set; }

        /// <summary>
        /// Gets or sets the customer role system name
        /// </summary>
        public string SystemName { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the customers must change passwords after a specified time
        /// </summary>
        public bool EnablePasswordLifetime { get; set; }

        public virtual ICollection<PermissionRecordUserRoleMapping> PermissionRecordUserRoleMappings
        {
            get => _permissionRecordUserRoleMappings ?? (_permissionRecordUserRoleMappings = new List<PermissionRecordUserRoleMapping>());
            protected set => _permissionRecordUserRoleMappings = value;
        }
    }
}