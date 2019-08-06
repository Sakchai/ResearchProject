using Research.Core;
using Research.Core.Caching;
using Research.Data;
using Research.Services.Events;
using System;
using System.Collections.Generic;
using System.Linq;


namespace Research.Services.Common
{
    /// <summary>
    /// EducationLevel service    
    /// </summary>
    public partial class EducationLevelService : IEducationLevelService
    {
        #region Fields

        private readonly IRepository<EducationLevel> _educationLevelRepository;
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
        /// <param name="educationLevelRepository">EducationLevel repository</param>
        /// <param name="eventPublisher">Event publisher</param>
        public EducationLevelService(ICacheManager cacheManager,
            IStaticCacheManager staticCacheManager,
            IRepository<EducationLevel> educationLevelRepository,
            IEventPublisher eventPublisher)
        {
            this._cacheManager = cacheManager;
            this._educationLevelRepository = educationLevelRepository;
            this._eventPublisher = eventPublisher;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Delete educationLevel
        /// </summary>
        /// <param name="educationLevel">EducationLevel</param>
        public virtual void DeleteEducationLevel(EducationLevel educationLevel)
        {
            if (educationLevel == null)
                throw new ArgumentNullException(nameof(educationLevel));

            _educationLevelRepository.Delete(educationLevel);

            //event notification
            _eventPublisher.EntityDeleted(educationLevel);

        }

        
        /// <summary>
        /// Gets all categories
        /// </summary>
        /// <param name="educationLevelName">EducationLevel name</param>
        /// <param name="pageIndex">Page index</param>
        /// <param name="pageSize">Page size</param>
        /// <param name="showHidden">A value indicating whether to show hidden records</param>
        /// <returns>EducationLevels</returns>
        public virtual IPagedList<EducationLevel> GetAllEducationLevels(string educationLevelName,
            int pageIndex = 0, int pageSize = int.MaxValue, bool showHidden = false)
        {
            var query = _educationLevelRepository.Table;
            if (!string.IsNullOrEmpty(educationLevelName))
                query = query.Where(a => a.Name == educationLevelName);

            //paging
            return new PagedList<EducationLevel>(query, pageIndex, pageSize);
        }


        /// <summary>
        /// Gets a educationLevel
        /// </summary>
        /// <param name="educationLevelId">EducationLevel identifier</param>
        /// <returns>EducationLevel</returns>
        public virtual EducationLevel GetEducationLevelById(int educationLevelId)
        {
            if (educationLevelId == 0)
                return null;
            
            var key = string.Format(ResearchEducationLevelDefaults.EducationLevelsByIdCacheKey, educationLevelId);
            return _cacheManager.Get(key, () => _educationLevelRepository.GetById(educationLevelId));
        }

        /// <summary>
        /// Inserts educationLevel
        /// </summary>
        /// <param name="educationLevel">EducationLevel</param>
        public virtual void InsertEducationLevel(EducationLevel educationLevel)
        {
            if (educationLevel == null)
                throw new ArgumentNullException(nameof(educationLevel));

            if (educationLevel is IEntityForCaching)
                throw new ArgumentException("Cacheable entities are not supported by Entity Framework");

            _educationLevelRepository.Insert(educationLevel);

            //cache
            _cacheManager.RemoveByPattern(ResearchEducationLevelDefaults.EducationLevelsPatternCacheKey);
            _staticCacheManager.RemoveByPattern(ResearchEducationLevelDefaults.EducationLevelsPatternCacheKey);

            //event notification
            _eventPublisher.EntityInserted(educationLevel);
        }

        /// <summary>
        /// Updates the educationLevel
        /// </summary>
        /// <param name="educationLevel">EducationLevel</param>
        public virtual void UpdateEducationLevel(EducationLevel educationLevel)
        {
            if (educationLevel == null)
                throw new ArgumentNullException(nameof(educationLevel));

            if (educationLevel is IEntityForCaching)
                throw new ArgumentException("Cacheable entities are not supported by Entity Framework");


            _educationLevelRepository.Update(educationLevel);

            //cache
            _cacheManager.RemoveByPattern(ResearchEducationLevelDefaults.EducationLevelsPatternCacheKey);
            _staticCacheManager.RemoveByPattern(ResearchEducationLevelDefaults.EducationLevelsPatternCacheKey);

            //event notification
            _eventPublisher.EntityUpdated(educationLevel);
        }
        

        /// <summary>
        /// Returns a list of names of not existing categories
        /// </summary>
        /// <param name="educationLevelIdsNames">The names and/or IDs of the categories to check</param>
        /// <returns>List of names and/or IDs not existing categories</returns>
        public virtual string[] GetNotExistingEducationLevels(string[] educationLevelIdsNames)
        {
            if (educationLevelIdsNames == null)
                throw new ArgumentNullException(nameof(educationLevelIdsNames));

            var query = _educationLevelRepository.Table;
            var queryFilter = educationLevelIdsNames.Distinct().ToArray();
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
        /// <param name="educationLevelIds">EducationLevel identifiers</param>
        /// <returns>EducationLevels</returns>
        public virtual List<EducationLevel> GetEducationLevelsByIds(int[] educationLevelIds)
        {
            if (educationLevelIds == null || educationLevelIds.Length == 0)
                return new List<EducationLevel>();

            var query = from p in _educationLevelRepository.Table
                where educationLevelIds.Contains(p.Id) //&& !p.Deleted
                select p;
            
            return query.ToList();
        }

        public List<EducationLevel> GetAllEducationLevels()
        {
            var query = _educationLevelRepository.Table;
            query.OrderBy(x => x.DisplayOrder);
            return query.ToList();
        }


        #endregion
    }
}
