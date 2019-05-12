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
    /// Title service    
    /// </summary>
    public partial class TitleService : ITitleService
    {
        #region Fields

        private readonly IRepository<Title> _titleRepository;
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
        /// <param name="titleRepository">Title repository</param>
        /// <param name="eventPublisher">Event publisher</param>
        public TitleService(ICacheManager cacheManager,
            IStaticCacheManager staticCacheManager,
            IRepository<Title> titleRepository,
            IEventPublisher eventPublisher)
        {
            this._cacheManager = cacheManager;
            this._titleRepository = titleRepository;
            this._eventPublisher = eventPublisher;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Delete title
        /// </summary>
        /// <param name="title">Title</param>
        public virtual void DeleteTitle(Title title)
        {
            if (title == null)
                throw new ArgumentNullException(nameof(title));

            _titleRepository.Delete(title);

            //event notification
            _eventPublisher.EntityDeleted(title);

        }

        
        /// <summary>
        /// Gets all categories
        /// </summary>
        /// <param name="titleName">Title name</param>
        /// <param name="pageIndex">Page index</param>
        /// <param name="pageSize">Page size</param>
        /// <param name="showHidden">A value indicating whether to show hidden records</param>
        /// <returns>Titles</returns>
        public virtual IPagedList<Title> GetAllTitles(string titleName,
            int pageIndex = 0, int pageSize = int.MaxValue, bool showHidden = false)
        {
            var query = _titleRepository.Table;
            if (!string.IsNullOrEmpty(titleName))
                query = query.Where(a => a.TitleNameTH == titleName);

            //paging
            return new PagedList<Title>(query, pageIndex, pageSize);
        }


        /// <summary>
        /// Gets a title
        /// </summary>
        /// <param name="titleId">Title identifier</param>
        /// <returns>Title</returns>
        public virtual Title GetTitleById(int titleId)
        {
            if (titleId == 0)
                return null;
            
            var key = string.Format(ResearchTitleDefaults.TitlesByIdCacheKey, titleId);
            return _cacheManager.Get(key, () => _titleRepository.GetById(titleId));
        }

        /// <summary>
        /// Inserts title
        /// </summary>
        /// <param name="title">Title</param>
        public virtual void InsertTitle(Title title)
        {
            if (title == null)
                throw new ArgumentNullException(nameof(title));

            if (title is IEntityForCaching)
                throw new ArgumentException("Cacheable entities are not supported by Entity Framework");

            _titleRepository.Insert(title);

            //cache
            _cacheManager.RemoveByPattern(ResearchTitleDefaults.TitlesPatternCacheKey);
            _staticCacheManager.RemoveByPattern(ResearchTitleDefaults.TitlesPatternCacheKey);

            //event notification
            _eventPublisher.EntityInserted(title);
        }

        /// <summary>
        /// Updates the title
        /// </summary>
        /// <param name="title">Title</param>
        public virtual void UpdateTitle(Title title)
        {
            if (title == null)
                throw new ArgumentNullException(nameof(title));

            if (title is IEntityForCaching)
                throw new ArgumentException("Cacheable entities are not supported by Entity Framework");


            _titleRepository.Update(title);

            //cache
            _cacheManager.RemoveByPattern(ResearchTitleDefaults.TitlesPatternCacheKey);
            _staticCacheManager.RemoveByPattern(ResearchTitleDefaults.TitlesPatternCacheKey);

            //event notification
            _eventPublisher.EntityUpdated(title);
        }
        

        /// <summary>
        /// Returns a list of names of not existing categories
        /// </summary>
        /// <param name="titleIdsNames">The names and/or IDs of the categories to check</param>
        /// <returns>List of names and/or IDs not existing categories</returns>
        public virtual string[] GetNotExistingTitles(string[] titleIdsNames)
        {
            if (titleIdsNames == null)
                throw new ArgumentNullException(nameof(titleIdsNames));

            var query = _titleRepository.Table;
            var queryFilter = titleIdsNames.Distinct().ToArray();
            //filtering by name
            var filter = query.Select(c => c.TitleNameTH).Where(c => queryFilter.Contains(c)).ToList();
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
        /// <param name="titleIds">Title identifiers</param>
        /// <returns>Titles</returns>
        public virtual List<Title> GetTitlesByIds(int[] titleIds)
        {
            if (titleIds == null || titleIds.Length == 0)
                return new List<Title>();

            var query = from p in _titleRepository.Table
                where titleIds.Contains(p.Id) //&& !p.Deleted
                select p;
            
            return query.ToList();
        }

        public List<Title> GetAllTitles()
        {
            var query = _titleRepository.Table;
            return query.ToList();
        }

        public string GetGender(int titleId)
        {
            var query = _titleRepository.Table;
            var title = query.Where(x => x.Id == titleId).FirstOrDefault();
            return title.Gender;
        }


        #endregion
    }
}
