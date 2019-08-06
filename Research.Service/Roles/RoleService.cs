using System;
using System.Collections.Generic;
using System.Linq;
using Research.Core.Caching;
using Research.Data;
using Research.Services.Events;

namespace Research.Services.Roles
{
    /// <summary>
    /// Message template service
    /// </summary>
    public partial class RoleService : IRoleService
    {
        #region Fields

        private readonly ICacheManager _cacheManager;
        private readonly IEventPublisher _eventPublisher;
        private readonly IRepository<Role> _roleRepository;
        private readonly string _entityName;

        #endregion

        #region Ctor

        public RoleService(ICacheManager cacheManager,
            IEventPublisher eventPublisher,
            IRepository<Role> roleRepository)
        {
            this._cacheManager = cacheManager;
            this._eventPublisher = eventPublisher;
            this._roleRepository = roleRepository;
            this._entityName = typeof(Role).Name;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Delete a message template
        /// </summary>
        /// <param name="role">Message template</param>
        public virtual void DeleteRole(Role role)
        {
            if (role == null)
                throw new ArgumentNullException(nameof(role));

            _roleRepository.Delete(role);

           // _cacheManager.RemoveByPattern(ResearchRoleDefaults.RolesPatternCacheKey);

            //event notification
            _eventPublisher.EntityDeleted(role);
        }

        /// <summary>
        /// Inserts a message template
        /// </summary>
        /// <param name="role">Message template</param>
        public virtual void InsertRole(Role role)
        {
            if (role == null)
                throw new ArgumentNullException(nameof(role));

            _roleRepository.Insert(role);

            //_cacheManager.RemoveByPattern(ResearchRoleDefaults.RolesPatternCacheKey);

            //event notification
            _eventPublisher.EntityInserted(role);
        }

        /// <summary>
        /// Updates a message template
        /// </summary>
        /// <param name="role">Message template</param>
        public virtual void UpdateRole(Role role)
        {
            if (role == null)
                throw new ArgumentNullException(nameof(role));

            _roleRepository.Update(role);

            //_cacheManager.RemoveByPattern(ResearchRoleDefaults.RolesPatternCacheKey);

            //event notification
            _eventPublisher.EntityUpdated(role);
        }

        /// <summary>
        /// Gets a message template
        /// </summary>
        /// <param name="roleId">Message template identifier</param>
        /// <returns>Message template</returns>
        public virtual Role GetRoleById(int roleId)
        {
            if (roleId == 0)
                return null;

            return _roleRepository.GetById(roleId);
        }

        /// <summary>
        /// Gets message roles by the name
        /// </summary>
        /// <param name="roleName">Message template name</param>
        /// <param name="storeId">Store identifier; pass null to load all records</param>
        /// <returns>List of message roles</returns>
        public virtual IList<Role> GetRolesByName(string roleName, int? storeId = null)
        {
            if (string.IsNullOrWhiteSpace(roleName))
                throw new ArgumentException(nameof(roleName));

           // var key = string.Format(ResearchRoleDefaults.RolesByNameCacheKey, roleName, storeId ?? 0);
           // return _cacheManager.Get(key, () =>
           // {
                //get message roles with the passed name
                var roles = _roleRepository.Table
                    .Where(role => role.RoleName.Equals(roleName))
                    .OrderBy(role => role.Id).ToList();

                //filter by the store
                //if (storeId.HasValue && storeId.Value > 0)
                //    roles = roles.Where(role => _storeMappingService.Authorize(role, storeId.Value)).ToList();

                return roles;
            //});
        }

        /// <summary>
        /// Gets all message roles
        /// </summary>
        /// <param name="storeId">Store identifier; pass 0 to load all records</param>
        /// <returns>Message template list</returns>
        public virtual IList<Role> GetAllRoles(int storeId)
        {
           // var key = string.Format(ResearchRoleDefaults.RolesAllCacheKey, storeId);
           // return _cacheManager.Get(key, () =>
           // {
                var query = _roleRepository.Table;
                query = query.OrderBy(t => t.RoleName);
                
                //if (storeId <= 0 || _catalogSettings.IgnoreStoreLimitations) 
                //    return query.ToList();
                
                ////store mapping
                //query = from t in query
                //    join sm in _storeMappingRepository.Table
                //        on new { c1 = t.Id, c2 = _entityName } equals new { c1 = sm.EntityId, c2 = sm.EntityName } into tSm
                //    from sm in tSm.DefaultIfEmpty()
                //    where !t.LimitedToStores || storeId == sm.StoreId
                //    select t;

                //query = query.Distinct().OrderBy(t => t.Name);

                return query.ToList();
            //});
        }

        /// <summary>
        /// Create a copy of message template with all depended data
        /// </summary>
        /// <param name="role">Message template</param>
        /// <returns>Message template copy</returns>
        public virtual Role CopyRole(Role role)
        {
            if (role == null)
                throw new ArgumentNullException(nameof(role));

            var mtCopy = new Role
            {
                IsActive = role.IsActive,
                RoleName = role.RoleName,
            };

            InsertRole(mtCopy);

            //var languages = _languageService.GetAllLanguages(true);

            ////localization
            //foreach (var lang in languages)
            //{
            //    var bccEmailAddresses = _localizationService.GetLocalized(role, x => x.BccEmailAddresses, lang.Id, false, false);
            //    if (!string.IsNullOrEmpty(bccEmailAddresses))
            //        _localizedEntityService.SaveLocalizedValue(mtCopy, x => x.BccEmailAddresses, bccEmailAddresses, lang.Id);

            //    var subject = _localizationService.GetLocalized(role, x => x.Subject, lang.Id, false, false);
            //    if (!string.IsNullOrEmpty(subject))
            //        _localizedEntityService.SaveLocalizedValue(mtCopy, x => x.Subject, subject, lang.Id);

            //    var body = _localizationService.GetLocalized(role, x => x.Body, lang.Id, false, false);
            //    if (!string.IsNullOrEmpty(body))
            //        _localizedEntityService.SaveLocalizedValue(mtCopy, x => x.Body, body, lang.Id);

            //    var emailAccountId = _localizationService.GetLocalized(role, x => x.EmailAccountId, lang.Id, false, false);
            //    if (emailAccountId > 0)
            //        _localizedEntityService.SaveLocalizedValue(mtCopy, x => x.EmailAccountId, emailAccountId, lang.Id);
            //}

            ////store mapping
            //var selectedStoreIds = _storeMappingService.GetStoresIdsWithAccess(role);
            //foreach (var id in selectedStoreIds)
            //{
            //    _storeMappingService.InsertStoreMapping(mtCopy, id);
            //}

            return mtCopy;
        }

        #endregion
    }
}