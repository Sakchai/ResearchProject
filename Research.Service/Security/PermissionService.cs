using Research.Core;
using Research.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using Research.Services;
using Research.Core.Caching;

namespace Research.Services.Security
{
    /// <summary>
    /// Permission service
    /// </summary>
    public partial class PermissionService : IPermissionService
    {
        #region Fields

        private readonly ICacheManager _cacheManager;
        private readonly IUserService _userService;
        private readonly IRepository<UserRole> _userRoleRepository;
        private readonly IRepository<RoleProgram> _roleProgramRepository;
        private readonly IStaticCacheManager _staticCacheManager;
        private readonly IWorkContext _workContext;

        #endregion

        #region Ctor

        public PermissionService(ICacheManager cacheManager,
            IUserService userService,
            IRepository<UserRole> userRoleRepository,
            IRepository<RoleProgram> roleProgramRepository,
            IStaticCacheManager staticCacheManager,
            IWorkContext workContext)
        {
            this._cacheManager = cacheManager;
            this._userService = userService;
            this._userRoleRepository = userRoleRepository;
            this._roleProgramRepository = roleProgramRepository;
            this._staticCacheManager = staticCacheManager;
            this._workContext = workContext;
        }

        #endregion

        #region Utilities

        /// <summary>
        /// Get userRole records by user role identifier
        /// </summary>
        /// <param name="userRoleId">User role identifier</param>
        /// <returns>Permissions</returns>
        protected virtual IList<UserRole> GetUserRolesByUserRoleId(int userRoleId)
        {
            var key = string.Format(ResearchSecurityDefaults.PermissionsAllByUserRoleIdCacheKey, userRoleId);
            return _cacheManager.Get(key, () =>
            {
                var query = from pr in _userRoleRepository.Table
                            join prcrm in _roleProgramRepository.Table on pr.Id equals prcrm.RoleId
                            where prcrm.RoleId == userRoleId
                            orderby pr.Id
                            select pr;

                return query.ToList();
            });
        }

        /// <summary>
        /// Authorize userRole
        /// </summary>
        /// <param name="userRoleRecordRoleName">Permission record system name</param>
        /// <param name="userRoleId">User role identifier</param>
        /// <returns>true - authorized; otherwise, false</returns>
        protected virtual bool Authorize(string userRoleRecordRoleName, int userRoleId)
        {
            if (string.IsNullOrEmpty(userRoleRecordRoleName))
                return false;

            var key = string.Format(ResearchSecurityDefaults.PermissionsAllowedCacheKey, userRoleId, userRoleRecordRoleName);
            return _staticCacheManager.Get(key, () =>
            {
                var userRoles = GetUserRolesByUserRoleId(userRoleId);
                foreach (var userRole1 in userRoles)
                    if (userRole1.Role.RoleName.Equals(userRoleRecordRoleName, StringComparison.InvariantCultureIgnoreCase))
                        return true;

                return false;
            });
        }

        #endregion

        #region Methods

        /// <summary>
        /// Delete a userRole
        /// </summary>
        /// <param name="userRole">Permission</param>
        public virtual void DeleteUserRole(UserRole userRole)
        {
            if (userRole == null)
                throw new ArgumentNullException(nameof(userRole));

            _userRoleRepository.Delete(userRole);

            _cacheManager.RemoveByPattern(ResearchSecurityDefaults.PermissionsPatternCacheKey);
            _staticCacheManager.RemoveByPattern(ResearchSecurityDefaults.PermissionsPatternCacheKey);
        }

        /// <summary>
        /// Gets a userRole
        /// </summary>
        /// <param name="userRoleId">Permission identifier</param>
        /// <returns>Permission</returns>
        public virtual UserRole GetUserRoleById(int userRoleId)
        {
            if (userRoleId == 0)
                return null;

            return _userRoleRepository.GetById(userRoleId);
        }

        /// <summary>
        /// Gets a userRole
        /// </summary>
        /// <param name="systemName">Permission system name</param>
        /// <returns>Permission</returns>
        public virtual UserRole GetUserRoleByRoleName(string systemName)
        {
            if (string.IsNullOrWhiteSpace(systemName))
                return null;

            var query = from pr in _userRoleRepository.Table
                      //  where pr.RoleName == systemName
                        orderby pr.Id
                        select pr;

            var userRoleRecord = query.FirstOrDefault();
            return userRoleRecord;
        }

        /// <summary>
        /// Gets all userRoles
        /// </summary>
        /// <returns>Permissions</returns>
        public virtual IList<UserRole> GetAllUserRoles()
        {
            var query = from pr in _userRoleRepository.Table
                       // orderby pr.Name
                        select pr;
            var userRoles = query.ToList();
            return userRoles;
        }

        /// <summary>
        /// Inserts a userRole
        /// </summary>
        /// <param name="userRole">Permission</param>
        public virtual void InsertUserRole(UserRole userRole)
        {
            if (userRole == null)
                throw new ArgumentNullException(nameof(userRole));

            _userRoleRepository.Insert(userRole);

            _cacheManager.RemoveByPattern(ResearchSecurityDefaults.PermissionsPatternCacheKey);
            _staticCacheManager.RemoveByPattern(ResearchSecurityDefaults.PermissionsPatternCacheKey);
        }

        /// <summary>
        /// Updates the userRole
        /// </summary>
        /// <param name="userRole">Permission</param>
        public virtual void UpdateUserRole(UserRole userRole)
        {
            if (userRole == null)
                throw new ArgumentNullException(nameof(userRole));

            _userRoleRepository.Update(userRole);

            _cacheManager.RemoveByPattern(ResearchSecurityDefaults.PermissionsPatternCacheKey);
            _staticCacheManager.RemoveByPattern(ResearchSecurityDefaults.PermissionsPatternCacheKey);
        }

        ///// <summary>
        ///// Install userRoles
        ///// </summary>
        ///// <param name="userRoleProvider">Permission provider</param>
        //public virtual void InstallPermissions(IPermissionProvider userRoleProvider)
        //{
        //    //install new userRoles
        //    var userRoles = userRoleProvider.GetPermissions();
        //    //default user role mappings
        //    var defaultPermissions = userRoleProvider.GetDefaultPermissions().ToList();

        //    foreach (var userRole in userRoles)
        //    {
        //        var userRole1 = GetUserRoleByRoleName(userRole.Role.RoleName);
        //        if (userRole1 != null)
        //            continue;

        //        //new userRole (install it)
        //        userRole1 = new UserRole
        //        {

        //            RoleId = userRole.RoleId,
        //            UserId = userRole.UserId
        //           // RoleName = userRole.RoleName,
        //           // Category = userRole.Category
        //        };

        //        foreach (var defaultPermission in defaultPermissions)
        //        {
        //            var userRole2 = _userService.GetUserRoleByRoleName(defaultPermission.UserRoleRoleName);
        //            if (userRole == null)
        //            {
        //                //new role (save it)
        //                userRole2 = new UserRole
        //                {
        //                    RoleId = defaultPermission.UserRoleRoleName,
        //                   // Active = true,
        //                   // RoleName = defaultPermission.UserRoleRoleName
        //                };
        //                _userService.InsertUserRole(userRole2);
        //            }

        //            var defaultMappingProvided = (from p in defaultPermission.UserRoles
        //                                          where p.RoleName == userRole1.Role.RoleName
        //                                          select p).Any();
        //            var mappingExists = (from mapping in userRole.RolePrograms
        //                                 where mapping.Role.RoleName == userRole1.Role.RoleName
        //                                 select mapping.Role).Any();
        //            if (defaultMappingProvided && !mappingExists)
        //            {
        //                //userRole1.UserRoles.Add(userRole);
        //                userRole1.RolePrograms.Add(new RoleProgram { UserRole = userRole });
        //            }
        //        }

        //        //save new userRole
        //        InsertUserRole(userRole1);

        //        //save localization
        //       // _localizationService.SaveLocalizedPermissionName(userRole1);
        //    }
        //}

        /// <summary>
        /// Uninstall userRoles
        /// </summary>
        /// <param name="userRoleProvider">Permission provider</param>
        public virtual void UninstallPermissions(IPermissionProvider userRoleProvider)
        {
            var userRoles = userRoleProvider.GetPermissions();
            foreach (var userRole in userRoles)
            {
                var userRole1 = GetUserRoleByRoleName(userRole.Role.RoleName);
                if (userRole1 == null) 
                    continue;

                DeleteUserRole(userRole1);

                //delete userRole locales
                //_localizationService.DeleteLocalizedPermissionName(userRole1);
            }
        }

        /// <summary>
        /// Authorize userRole
        /// </summary>
        /// <param name="userRole">Permission record</param>
        /// <returns>true - authorized; otherwise, false</returns>
        public virtual bool Authorize(UserRole userRole)
        {
            return Authorize(userRole, _workContext.CurrentUser);
        }

        /// <summary>
        /// Authorize userRole
        /// </summary>
        /// <param name="userRole">Permission record</param>
        /// <param name="user">User</param>
        /// <returns>true - authorized; otherwise, false</returns>
        public virtual bool Authorize(UserRole userRole, User user)
        {
            if (userRole == null)
                return false;

            if (user == null)
                return false;


            return Authorize(userRole.Role.RoleName, user);
        }

        /// <summary>
        /// Authorize userRole
        /// </summary>
        /// <param name="userRoleRecordRoleName">Permission record system name</param>
        /// <returns>true - authorized; otherwise, false</returns>
        public virtual bool Authorize(string userRoleRecordRoleName)
        {
            return Authorize(userRoleRecordRoleName, _workContext.CurrentUser);
        }

        /// <summary>
        /// Authorize userRole
        /// </summary>
        /// <param name="userRoleRecordRoleName">Permission record system name</param>
        /// <param name="user">User</param>
        /// <returns>true - authorized; otherwise, false</returns>
        public virtual bool Authorize(string userRoleRecordRoleName, User user)
        {
            if (string.IsNullOrEmpty(userRoleRecordRoleName))
                return false;

            var userRoles = user.UserRoles.Where(cr => cr.Role.IsActive);
            foreach (var role in userRoles)
                if (Authorize(userRoleRecordRoleName, role.Id))
                    //yes, we have such userRole
                    return true;

            //no userRole found
            return false;
        }
        #endregion
        public RoleProgram GetRoleProgram(Program program, UserRole userRole)
        {
            return userRole.RolePrograms.Where(x => x.ProgramId == program.Id).FirstOrDefault();
        }

        public IList<RoleProgram> GetRolePrograms(UserRole userRole)
        {
            return userRole.RolePrograms.Where(x => x.RoleId == userRole.RoleId).ToList();
        }


       
    }
}