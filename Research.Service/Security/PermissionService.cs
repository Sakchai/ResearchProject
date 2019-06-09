using System;
using System.Collections.Generic;
using System.Linq;
using Research.Core;
using Research.Core.Caching;
using Research.Data;

namespace Research.Services.Security
{
    /// <summary>
    /// Permission service
    /// </summary>
    public partial class PermissionService : IPermissionService
    {
        #region Fields

        private readonly IRepository<PermissionRecord> _permissionRecordRepository;
        private readonly IRepository<PermissionRecordUserRoleMapping> _permissionRecordUserRoleMappingRepository;
        private readonly IUserService _userService;
        private readonly IWorkContext _workContext;
        private readonly ICacheManager _cacheManager;
        private readonly IStaticCacheManager _staticCacheManager;

        #endregion

        #region Ctor

        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="permissionRecordRepository">Permission repository</param>
        /// <param name="permissionRecordUserRoleMappingRepository">Permission -user role mapping repository</param>
        /// <param name="userService">User service</param>
        /// <param name="workContext">Work context</param>
        /// <param name="localizationService">Localization service</param>
        /// <param name="languageService">Language service</param>
        /// <param name="cacheManager">Cache manager</param>
        /// <param name="staticCacheManager">Static cache manager</param>
        public PermissionService(IRepository<PermissionRecord> permissionRecordRepository,
            IRepository<PermissionRecordUserRoleMapping> permissionRecordUserRoleMappingRepository,
            IUserService userService,
            IWorkContext workContext,
            ICacheManager cacheManager,
            IStaticCacheManager staticCacheManager)
        {
            this._permissionRecordRepository = permissionRecordRepository;
            this._permissionRecordUserRoleMappingRepository = permissionRecordUserRoleMappingRepository;
            this._userService = userService;
            this._workContext = workContext;
            this._cacheManager = cacheManager;
            this._staticCacheManager = staticCacheManager;
        }

        #endregion

        #region Utilities

        /// <summary>
        /// Get permission records by user role identifier
        /// </summary>
        /// <param name="userRoleId">User role identifier</param>
        /// <returns>Permissions</returns>
        protected virtual IList<PermissionRecord> GetPermissionRecordsByUserRoleId(int userRoleId)
        {
            var key = string.Format(ResearchSecurityDefaults.PermissionsAllByUserRoleIdCacheKey, userRoleId);
            return _cacheManager.Get(key, () =>
            {
                var query = from pr in _permissionRecordRepository.Table
                            join prcrm in _permissionRecordUserRoleMappingRepository.Table on pr.Id equals prcrm.PermissionRecordId
                            where prcrm.UserRoleId == userRoleId
                            orderby pr.Id
                            select pr;

                return query.ToList();
            });
        }

        /// <summary>
        /// Authorize permission
        /// </summary>
        /// <param name="permissionRecordSystemName">Permission record system name</param>
        /// <param name="userRoleId">User role identifier</param>
        /// <returns>true - authorized; otherwise, false</returns>
        protected virtual bool Authorize(string permissionRecordSystemName, int userRoleId)
        {
            if (string.IsNullOrEmpty(permissionRecordSystemName))
                return false;

            var key = string.Format(ResearchSecurityDefaults.PermissionsAllowedCacheKey, userRoleId, permissionRecordSystemName);
            return _staticCacheManager.Get(key, () =>
            {
                var permissions = GetPermissionRecordsByUserRoleId(userRoleId);
                foreach (var permission1 in permissions)
                    if (permission1.SystemName.Equals(permissionRecordSystemName, StringComparison.InvariantCultureIgnoreCase))
                        return true;

                return false;
            });
        }

        #endregion

        #region Methods

        /// <summary>
        /// Delete a permission
        /// </summary>
        /// <param name="permission">Permission</param>
        public virtual void DeletePermissionRecord(PermissionRecord permission)
        {
            if (permission == null)
                throw new ArgumentNullException(nameof(permission));

            _permissionRecordRepository.Delete(permission);

            _cacheManager.RemoveByPattern(ResearchSecurityDefaults.PermissionsPatternCacheKey);
            _staticCacheManager.RemoveByPattern(ResearchSecurityDefaults.PermissionsPatternCacheKey);
        }

        /// <summary>
        /// Gets a permission
        /// </summary>
        /// <param name="permissionId">Permission identifier</param>
        /// <returns>Permission</returns>
        public virtual PermissionRecord GetPermissionRecordById(int permissionId)
        {
            if (permissionId == 0)
                return null;

            return _permissionRecordRepository.GetById(permissionId);
        }

        /// <summary>
        /// Gets a permission
        /// </summary>
        /// <param name="systemName">Permission system name</param>
        /// <returns>Permission</returns>
        public virtual PermissionRecord GetPermissionRecordBySystemName(string systemName)
        {
            if (string.IsNullOrWhiteSpace(systemName))
                return null;

            var query = from pr in _permissionRecordRepository.Table
                        where pr.SystemName == systemName
                        orderby pr.Id
                        select pr;

            var permissionRecord = query.FirstOrDefault();
            return permissionRecord;
        }

        /// <summary>
        /// Gets all permissions
        /// </summary>
        /// <returns>Permissions</returns>
        public virtual IList<PermissionRecord> GetAllPermissionRecords()
        {
            var query = from pr in _permissionRecordRepository.Table
                        orderby pr.Name
                        select pr;
            var permissions = query.ToList();
            return permissions;
        }

        /// <summary>
        /// Inserts a permission
        /// </summary>
        /// <param name="permission">Permission</param>
        public virtual void InsertPermissionRecord(PermissionRecord permission)
        {
            if (permission == null)
                throw new ArgumentNullException(nameof(permission));

            _permissionRecordRepository.Insert(permission);

            _cacheManager.RemoveByPattern(ResearchSecurityDefaults.PermissionsPatternCacheKey);
            _staticCacheManager.RemoveByPattern(ResearchSecurityDefaults.PermissionsPatternCacheKey);
        }

        /// <summary>
        /// Updates the permission
        /// </summary>
        /// <param name="permission">Permission</param>
        public virtual void UpdatePermissionRecord(PermissionRecord permission)
        {
            if (permission == null)
                throw new ArgumentNullException(nameof(permission));

            _permissionRecordRepository.Update(permission);

            _cacheManager.RemoveByPattern(ResearchSecurityDefaults.PermissionsPatternCacheKey);
            _staticCacheManager.RemoveByPattern(ResearchSecurityDefaults.PermissionsPatternCacheKey);
        }

        /// <summary>
        /// Install permissions
        /// </summary>
        /// <param name="permissionProvider">Permission provider</param>
        public virtual void InstallPermissions(IPermissionProvider permissionProvider)
        {
            //install new permissions
            var permissions = permissionProvider.GetPermissions();
            //default user role mappings
            var defaultPermissions = permissionProvider.GetDefaultPermissions().ToList();

            foreach (var permission in permissions)
            {
                var permission1 = GetPermissionRecordBySystemName(permission.SystemName);
                if (permission1 != null)
                    continue;

                //new permission (install it)
                permission1 = new PermissionRecord
                {
                    Name = permission.Name,
                    SystemName = permission.SystemName,
                    Category = permission.Category,
                };

                foreach (var defaultPermission in defaultPermissions)
                {
                    var userRole = _userService.GetUserRoleBySystemName(defaultPermission.UserRoleSystemName);
                    if (userRole == null)
                    {
                        //new role (save it)
                        userRole = new UserRole
                        {
                            Name = defaultPermission.UserRoleSystemName,
                            IsActive = true,
                            SystemName = defaultPermission.UserRoleSystemName
                        };
                        _userService.InsertUserRole(userRole);
                    }

                    var defaultMappingProvided = (from p in defaultPermission.PermissionRecords
                                                  where p.SystemName == permission1.SystemName
                                                  select p).Any();
                    var mappingExists = (from mapping in userRole.PermissionRecordUserRoleMappings
                                         where mapping.PermissionRecord.SystemName == permission1.SystemName
                                         select mapping.PermissionRecord).Any();
                    if (defaultMappingProvided && !mappingExists)
                    {
                        //permission1.UserRoles.Add(userRole);
                        permission1.PermissionRecordUserRoleMappings.Add(new PermissionRecordUserRoleMapping { UserRole = userRole });
                    }
                }

                //save new permission
                InsertPermissionRecord(permission1);

                //save localization
                //permission1.SaveLocalizedPermissionName(_localizationService, _languageService);
            }
        }

        /// <summary>
        /// Uninstall permissions
        /// </summary>
        /// <param name="permissionProvider">Permission provider</param>
        public virtual void UninstallPermissions(IPermissionProvider permissionProvider)
        {
            var permissions = permissionProvider.GetPermissions();
            foreach (var permission in permissions)
            {
                var permission1 = GetPermissionRecordBySystemName(permission.SystemName);
                if (permission1 != null)
                {
                    DeletePermissionRecord(permission1);

                    //delete permission locales
                   // permission1.DeleteLocalizedPermissionName(_localizationService, _languageService);
                }
            }
        }

        /// <summary>
        /// Authorize permission
        /// </summary>
        /// <param name="permission">Permission record</param>
        /// <returns>true - authorized; otherwise, false</returns>
        public virtual bool Authorize(PermissionRecord permission)
        {
            return Authorize(permission, _workContext.CurrentUser);
        }

        /// <summary>
        /// Authorize permission
        /// </summary>
        /// <param name="permission">Permission record</param>
        /// <param name="user">User</param>
        /// <returns>true - authorized; otherwise, false</returns>
        public virtual bool Authorize(PermissionRecord permission, User user)
        {
            if (permission == null)
                return false;

            if (user == null)
                return false;

            //old implementation of Authorize method
            //var userRoles = user.UserRoles.Where(cr => cr.Active);
            //foreach (var role in userRoles)
            //    foreach (var permission1 in role.PermissionRecords)
            //        if (permission1.SystemName.Equals(permission.SystemName, StringComparison.InvariantCultureIgnoreCase))
            //            return true;

            //return false;

            return Authorize(permission.SystemName, user);
        }

        /// <summary>
        /// Authorize permission
        /// </summary>
        /// <param name="permissionRecordSystemName">Permission record system name</param>
        /// <returns>true - authorized; otherwise, false</returns>
        public virtual bool Authorize(string permissionRecordSystemName)
        {
            return Authorize(permissionRecordSystemName, _workContext.CurrentUser);
        }

        /// <summary>
        /// Authorize permission
        /// </summary>
        /// <param name="permissionRecordSystemName">Permission record system name</param>
        /// <param name="user">User</param>
        /// <returns>true - authorized; otherwise, false</returns>
        public virtual bool Authorize(string permissionRecordSystemName, User user)
        {
            if (string.IsNullOrEmpty(permissionRecordSystemName))
                return false;

            var userRoles = user.UserRoles.Where(cr => cr.IsActive);
            foreach (var role in userRoles)
                if (Authorize(permissionRecordSystemName, role.Id))
                    //yes, we have such permission
                    return true;

            //no permission found
            return false;
        }

        #endregion
    }
}