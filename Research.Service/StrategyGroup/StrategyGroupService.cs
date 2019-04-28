using Research.Core;
using Research.Core.Caching;
using Research.Data;
using Research.Services.Events;
using System;
using System.Collections.Generic;
using System.Linq;


namespace Research.Services.StrategyGroups
{
    /// <summary>
    /// StrategyGroup service    
    /// </summary>
    public partial class StrategyGroupService : IStrategyGroupService
    {
        #region Fields

        private readonly IRepository<StrategyGroup> _strategyGroupRepository;
        private readonly IEventPublisher _eventPublisher;
        private readonly ICacheManager _cacheManager;
        private readonly IStaticCacheManager _staticCacheManager;

        #endregion

        #region Ctor

        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="cacheManager">Cache manager</param>
        /// <param name="staticCacheManager">Static cache manager</param>
        /// <param name="strategyGroupRepository">StrategyGroup repository</param>
        /// <param name="eventPublisher">Event publisher</param>
        public StrategyGroupService(ICacheManager cacheManager,
            IStaticCacheManager staticCacheManager,
            IRepository<StrategyGroup> strategyGroupRepository,
            IEventPublisher eventPublisher)
        {
            this._cacheManager = cacheManager;
            this._strategyGroupRepository = strategyGroupRepository;
            this._eventPublisher = eventPublisher;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Delete strategyGroup
        /// </summary>
        /// <param name="strategyGroup">StrategyGroup</param>
        public virtual void DeleteStrategyGroup(StrategyGroup strategyGroup)
        {
            if (strategyGroup == null)
                throw new ArgumentNullException(nameof(strategyGroup));

            _strategyGroupRepository.Delete(strategyGroup);

            //event notification
            _eventPublisher.EntityDeleted(strategyGroup);

        }

        
        /// <summary>
        /// Gets all categories
        /// </summary>
        /// <param name="strategyGroupName">StrategyGroup name</param>
        /// <param name="pageIndex">Page index</param>
        /// <param name="pageSize">Page size</param>
        /// <param name="showHidden">A value indicating whether to show hidden records</param>
        /// <returns>StrategyGroups</returns>
        public virtual IPagedList<StrategyGroup> GetAllStrategyGroups(string strategyGroupName,
            int pageIndex = 0, int pageSize = int.MaxValue, bool showHidden = false)
        {
            var query = _strategyGroupRepository.Table;
            if (!string.IsNullOrEmpty(strategyGroupName))
                query = query.Where(a => a.Name == strategyGroupName);

            //paging
            return new PagedList<StrategyGroup>(query, pageIndex, pageSize);
        }


        /// <summary>
        /// Gets a strategyGroup
        /// </summary>
        /// <param name="strategyGroupId">StrategyGroup identifier</param>
        /// <returns>StrategyGroup</returns>
        public virtual StrategyGroup GetStrategyGroupById(int strategyGroupId)
        {
            if (strategyGroupId == 0)
                return null;
            
            var key = string.Format(ResearchStrategyGroupDefaults.StrategyGroupsByIdCacheKey, strategyGroupId);
            return _cacheManager.Get(key, () => _strategyGroupRepository.GetById(strategyGroupId));
        }

        /// <summary>
        /// Inserts strategyGroup
        /// </summary>
        /// <param name="strategyGroup">StrategyGroup</param>
        public virtual void InsertStrategyGroup(StrategyGroup strategyGroup)
        {
            if (strategyGroup == null)
                throw new ArgumentNullException(nameof(strategyGroup));

            if (strategyGroup is IEntityForCaching)
                throw new ArgumentException("Cacheable entities are not supported by Entity Framework");

            _strategyGroupRepository.Insert(strategyGroup);

            //cache
            _cacheManager.RemoveByPattern(ResearchStrategyGroupDefaults.StrategyGroupsPatternCacheKey);
            _staticCacheManager.RemoveByPattern(ResearchStrategyGroupDefaults.StrategyGroupsPatternCacheKey);

            //event notification
            _eventPublisher.EntityInserted(strategyGroup);
        }

        /// <summary>
        /// Updates the strategyGroup
        /// </summary>
        /// <param name="strategyGroup">StrategyGroup</param>
        public virtual void UpdateStrategyGroup(StrategyGroup strategyGroup)
        {
            if (strategyGroup == null)
                throw new ArgumentNullException(nameof(strategyGroup));

            if (strategyGroup is IEntityForCaching)
                throw new ArgumentException("Cacheable entities are not supported by Entity Framework");


            _strategyGroupRepository.Update(strategyGroup);

            //cache
            _cacheManager.RemoveByPattern(ResearchStrategyGroupDefaults.StrategyGroupsPatternCacheKey);
            _staticCacheManager.RemoveByPattern(ResearchStrategyGroupDefaults.StrategyGroupsPatternCacheKey);

            //event notification
            _eventPublisher.EntityUpdated(strategyGroup);
        }
        

        /// <summary>
        /// Returns a list of names of not existing categories
        /// </summary>
        /// <param name="strategyGroupIdsNames">The names and/or IDs of the categories to check</param>
        /// <returns>List of names and/or IDs not existing categories</returns>
        public virtual string[] GetNotExistingStrategyGroups(string[] strategyGroupIdsNames)
        {
            if (strategyGroupIdsNames == null)
                throw new ArgumentNullException(nameof(strategyGroupIdsNames));

            var query = _strategyGroupRepository.Table;
            var queryFilter = strategyGroupIdsNames.Distinct().ToArray();
            //filtering by name
            var filter = query.Select(c => c.Name).Where(c => queryFilter.Contains(c)).ToList();
            queryFilter = queryFilter.Except(filter).ToArray();

            //if some names not found
            if (queryFilter.Any())
            {
                //filtering by IDs
                filter = query.Select(c => c.Id.ToString()).Where(c => queryFilter.Contains(c)).ToList();
                queryFilter = queryFilter.Except(filter).ToArray();
            }

            return queryFilter.ToArray();
        }

        /// <summary>
        /// Gets categories by identifier
        /// </summary>
        /// <param name="strategyGroupIds">StrategyGroup identifiers</param>
        /// <returns>StrategyGroups</returns>
        public virtual List<StrategyGroup> GetStrategyGroupsByIds(int[] strategyGroupIds)
        {
            if (strategyGroupIds == null || strategyGroupIds.Length == 0)
                return new List<StrategyGroup>();

            var query = from p in _strategyGroupRepository.Table
                where strategyGroupIds.Contains(p.Id) //&& !p.Deleted
                select p;
            
            return query.ToList();
        }

        public List<StrategyGroup> GetAllStrategyGroups()
        {
            var query = _strategyGroupRepository.Table;
            return query.ToList();
        }


        #endregion
    }
}
