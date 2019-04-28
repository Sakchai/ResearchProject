using Research.Core;
using Research.Core.Caching;
using Research.Data;
using Research.Services.Events;
using System;
using System.Collections.Generic;
using System.Linq;


namespace Research.Services.ResearchIssues
{
    /// <summary>
    /// ResearchIssue service    
    /// </summary>
    public partial class ResearchIssueService : IResearchIssueService
    {
        #region Fields

        private readonly IRepository<ResearchIssue> _researchIssueRepository;
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
        /// <param name="researchIssueRepository">ResearchIssue repository</param>
        /// <param name="eventPublisher">Event publisher</param>
        public ResearchIssueService(ICacheManager cacheManager,
            IStaticCacheManager staticCacheManager,
            IRepository<ResearchIssue> researchIssueRepository,
            IEventPublisher eventPublisher)
        {
            this._cacheManager = cacheManager;
            this._researchIssueRepository = researchIssueRepository;
            this._eventPublisher = eventPublisher;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Delete researchIssue
        /// </summary>
        /// <param name="researchIssue">ResearchIssue</param>
        public virtual void DeleteResearchIssue(ResearchIssue researchIssue)
        {
            if (researchIssue == null)
                throw new ArgumentNullException(nameof(researchIssue));

            _researchIssueRepository.Delete(researchIssue);

            //event notification
            _eventPublisher.EntityDeleted(researchIssue);

        }

        
        /// <summary>
        /// Gets all categories
        /// </summary>
        /// <param name="researchIssueName">ResearchIssue name</param>
        /// <param name="pageIndex">Page index</param>
        /// <param name="pageSize">Page size</param>
        /// <param name="showHidden">A value indicating whether to show hidden records</param>
        /// <returns>ResearchIssues</returns>
        public virtual IPagedList<ResearchIssue> GetAllResearchIssues(string researchIssueName,
            int pageIndex = 0, int pageSize = int.MaxValue, bool showHidden = false)
        {
            var query = _researchIssueRepository.Table;
            if (!string.IsNullOrEmpty(researchIssueName))
                query = query.Where(a => a.Name == researchIssueName);

            //paging
            return new PagedList<ResearchIssue>(query, pageIndex, pageSize);
        }


        /// <summary>
        /// Gets a researchIssue
        /// </summary>
        /// <param name="researchIssueId">ResearchIssue identifier</param>
        /// <returns>ResearchIssue</returns>
        public virtual ResearchIssue GetResearchIssueById(int researchIssueId)
        {
            if (researchIssueId == 0)
                return null;
            
            var key = string.Format(ResearchResearchIssueDefaults.ResearchIssuesByIdCacheKey, researchIssueId);
            return _cacheManager.Get(key, () => _researchIssueRepository.GetById(researchIssueId));
        }

        /// <summary>
        /// Inserts researchIssue
        /// </summary>
        /// <param name="researchIssue">ResearchIssue</param>
        public virtual void InsertResearchIssue(ResearchIssue researchIssue)
        {
            if (researchIssue == null)
                throw new ArgumentNullException(nameof(researchIssue));

            if (researchIssue is IEntityForCaching)
                throw new ArgumentException("Cacheable entities are not supported by Entity Framework");

            _researchIssueRepository.Insert(researchIssue);

            //cache
            _cacheManager.RemoveByPattern(ResearchResearchIssueDefaults.ResearchIssuesPatternCacheKey);
            _staticCacheManager.RemoveByPattern(ResearchResearchIssueDefaults.ResearchIssuesPatternCacheKey);

            //event notification
            _eventPublisher.EntityInserted(researchIssue);
        }

        /// <summary>
        /// Updates the researchIssue
        /// </summary>
        /// <param name="researchIssue">ResearchIssue</param>
        public virtual void UpdateResearchIssue(ResearchIssue researchIssue)
        {
            if (researchIssue == null)
                throw new ArgumentNullException(nameof(researchIssue));

            if (researchIssue is IEntityForCaching)
                throw new ArgumentException("Cacheable entities are not supported by Entity Framework");


            _researchIssueRepository.Update(researchIssue);

            //cache
            _cacheManager.RemoveByPattern(ResearchResearchIssueDefaults.ResearchIssuesPatternCacheKey);
            _staticCacheManager.RemoveByPattern(ResearchResearchIssueDefaults.ResearchIssuesPatternCacheKey);

            //event notification
            _eventPublisher.EntityUpdated(researchIssue);
        }
        

        /// <summary>
        /// Returns a list of names of not existing categories
        /// </summary>
        /// <param name="researchIssueIdsNames">The names and/or IDs of the categories to check</param>
        /// <returns>List of names and/or IDs not existing categories</returns>
        public virtual string[] GetNotExistingResearchIssues(string[] researchIssueIdsNames)
        {
            if (researchIssueIdsNames == null)
                throw new ArgumentNullException(nameof(researchIssueIdsNames));

            var query = _researchIssueRepository.Table;
            var queryFilter = researchIssueIdsNames.Distinct().ToArray();
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
        /// <param name="researchIssueIds">ResearchIssue identifiers</param>
        /// <returns>ResearchIssues</returns>
        public virtual List<ResearchIssue> GetResearchIssuesByIds(int[] researchIssueIds)
        {
            if (researchIssueIds == null || researchIssueIds.Length == 0)
                return new List<ResearchIssue>();

            var query = from p in _researchIssueRepository.Table
                where researchIssueIds.Contains(p.Id) //&& !p.Deleted
                select p;
            
            return query.ToList();
        }

        public List<ResearchIssue> GetAllResearchIssues()
        {
            var query = _researchIssueRepository.Table;
            return query.ToList();
        }


        #endregion
    }
}
