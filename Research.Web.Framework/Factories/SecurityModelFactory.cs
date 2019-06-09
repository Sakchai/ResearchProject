using Research.Core;
using Research.Services;
using Research.Services.Security;
using Research.Web.Extensions;
using Research.Web.Models.Security;
using Research.Web.Models.Users;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Research.Web.Factories
{
    /// <summary>
    /// Represents the security model factory implementation
    /// </summary>
    public partial class SecurityModelFactory : ISecurityModelFactory
    {
        #region Fields

        private readonly IUserService _userService;
        private readonly IPermissionService _permissionService;
        private readonly IWorkContext _workContext;

        #endregion

        #region Ctor

        public SecurityModelFactory(IUserService userService,
            IPermissionService permissionService,
            IWorkContext workContext)
        {
            this._userService = userService;
            this._permissionService = permissionService;
            this._workContext = workContext;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Prepare permission mapping model
        /// </summary>
        /// <param name="model">Permission mapping model</param>
        /// <returns>Permission mapping model</returns>
        public virtual PermissionMappingModel PreparePermissionMappingModel(PermissionMappingModel model)
        {
            if (model == null)
                throw new ArgumentNullException(nameof(model));

            var userRoles = _userService.GetAllUserRoles(true);
            model.AvailableUserRoles = userRoles.Select(role => role.ToModel<UserRoleModel>()).OrderBy(x=>x.Id).ToList();

            foreach (var permissionRecord in _permissionService.GetAllPermissionRecords())
            {
                model.AvailablePermissions.Add(new PermissionRecordModel
                {
                    Name = permissionRecord.Name,
                    SystemName = permissionRecord.SystemName
                });

                foreach (var role in userRoles)
                {
                    if (!model.Allowed.ContainsKey(permissionRecord.SystemName))
                        model.Allowed[permissionRecord.SystemName] = new Dictionary<int, bool>();
                    model.Allowed[permissionRecord.SystemName][role.Id] = permissionRecord.PermissionRecordUserRoleMappings
                        .Any(mapping => mapping.UserRoleId == role.Id);
                }
            }

            return model;
        }

        #endregion
    }
}