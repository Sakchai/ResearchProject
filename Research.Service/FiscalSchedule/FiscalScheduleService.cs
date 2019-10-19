using Research.Core;
using Research.Core.Caching;
using Research.Data;
using Research.Services.Events;
using System;
using System.Collections.Generic;
using System.Linq;


namespace Research.Services.FiscalSchedules
{
    /// <summary>
    /// FiscalSchedule service    
    /// </summary>
    public partial class FiscalScheduleService : IFiscalScheduleService
    {
        #region Fields

        private readonly IRepository<FiscalSchedule> _fiscalScheduleRepository;
        private readonly IEventPublisher _eventPublisher;
        private readonly ICacheManager _cacheManager;
       // private readonly IStaticCacheManager _staticCacheManager;

        #endregion

        #region Ctor

        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="cacheManager">Cache manager</param>
        /// <param name="staticCacheManager">Static cache manager</param>
        /// <param name="fiscalScheduleRepository">FiscalSchedule repository</param>
        /// <param name="eventPublisher">Event publisher</param>
        public FiscalScheduleService(ICacheManager cacheManager,
      //      IStaticCacheManager staticCacheManager,
            IRepository<FiscalSchedule> fiscalScheduleRepository,
            IEventPublisher eventPublisher)
        {
            this._cacheManager = cacheManager;
            this._fiscalScheduleRepository = fiscalScheduleRepository;
            this._eventPublisher = eventPublisher;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Delete fiscalSchedule
        /// </summary>
        /// <param name="fiscalSchedule">FiscalSchedule</param>
        public virtual void DeleteFiscalSchedule(FiscalSchedule fiscalSchedule)
        {
            if (fiscalSchedule == null)
                throw new ArgumentNullException(nameof(fiscalSchedule));

            _fiscalScheduleRepository.Delete(fiscalSchedule);

            //event notification
            _eventPublisher.EntityDeleted(fiscalSchedule);

        }

        
        /// <summary>
        /// Gets all categories
        /// </summary>
        /// <param name="fiscalScheduleName">FiscalSchedule name</param>
        /// <param name="pageIndex">Page index</param>
        /// <param name="pageSize">Page size</param>
        /// <param name="showHidden">A value indicating whether to show hidden records</param>
        /// <returns>FiscalSchedules</returns>
        public virtual IPagedList<FiscalSchedule> GetAllFiscalSchedules(string fiscalScheduleName, DateTime? openingDate = null, DateTime? closingDate = null, int fiscalYear = 0,
            int pageIndex = 0, int pageSize = int.MaxValue, bool showHidden = false)
        {
            var query = _fiscalScheduleRepository.Table;
            query = query.Where(a => a.Deleted != true);

            //filter by creation date
            if (openingDate.HasValue)
                query = query.Where(x => openingDate.Value <= x.OpeningDate);
            if (closingDate.HasValue)
                query = query.Where(x => closingDate.Value >= x.ClosingDate);

            if (fiscalYear != 0)
                query = query.Where(a => a.FiscalYear == fiscalYear);
            if (!string.IsNullOrEmpty(fiscalScheduleName))
                query = query.Where(a => a.ScholarName.Contains(fiscalScheduleName));

            //paging
            return new PagedList<FiscalSchedule>(query, pageIndex, pageSize);
        }


        /// <summary>
        /// Gets a fiscalSchedule
        /// </summary>
        /// <param name="fiscalScheduleId">FiscalSchedule identifier</param>
        /// <returns>FiscalSchedule</returns>
        public virtual FiscalSchedule GetFiscalScheduleById(int fiscalScheduleId)
        {
            if (fiscalScheduleId == 0)
                return null;
            
            var key = string.Format(ResearchFiscalScheduleDefaults.FiscalSchedulesByIdCacheKey, fiscalScheduleId);
            return _cacheManager.Get(key, () => _fiscalScheduleRepository.GetById(fiscalScheduleId));
        }

        /// <summary>
        /// Inserts fiscalSchedule
        /// </summary>
        /// <param name="fiscalSchedule">FiscalSchedule</param>
        public virtual void InsertFiscalSchedule(FiscalSchedule fiscalSchedule)
        {
            if (fiscalSchedule == null)
                throw new ArgumentNullException(nameof(fiscalSchedule));

            if (fiscalSchedule is IEntityForCaching)
                throw new ArgumentException("Cacheable entities are not supported by Entity Framework");

            _fiscalScheduleRepository.Insert(fiscalSchedule);

            //cache
            _cacheManager.RemoveByPattern(ResearchFiscalScheduleDefaults.FiscalSchedulesPatternCacheKey);
            //_staticCacheManager.RemoveByPattern(ResearchFiscalScheduleDefaults.FiscalSchedulesPatternCacheKey);

            //event notification
            _eventPublisher.EntityInserted(fiscalSchedule);
        }

        /// <summary>
        /// Updates the fiscalSchedule
        /// </summary>
        /// <param name="fiscalSchedule">FiscalSchedule</param>
        public virtual void UpdateFiscalSchedule(FiscalSchedule fiscalSchedule)
        {
            if (fiscalSchedule == null)
                throw new ArgumentNullException(nameof(fiscalSchedule));

            if (fiscalSchedule is IEntityForCaching)
                throw new ArgumentException("Cacheable entities are not supported by Entity Framework");


            _fiscalScheduleRepository.Update(fiscalSchedule);

            //cache
            _cacheManager.RemoveByPattern(ResearchFiscalScheduleDefaults.FiscalSchedulesPatternCacheKey);
           // _staticCacheManager.RemoveByPattern(ResearchFiscalScheduleDefaults.FiscalSchedulesPatternCacheKey);

            //event notification
            _eventPublisher.EntityUpdated(fiscalSchedule);
        }
        

        /// <summary>
        /// Returns a list of names of not existing categories
        /// </summary>
        /// <param name="fiscalScheduleIdsNames">The names and/or IDs of the categories to check</param>
        /// <returns>List of names and/or IDs not existing categories</returns>
        public virtual string[] GetNotExistingFiscalSchedules(string[] fiscalScheduleIdsNames)
        {
            if (fiscalScheduleIdsNames == null)
                throw new ArgumentNullException(nameof(fiscalScheduleIdsNames));

            var query = _fiscalScheduleRepository.Table;
            var queryFilter = fiscalScheduleIdsNames.Distinct().ToArray();
            //filtering by name
            var filter = query.Select(c => c.ScholarName).Where(c => queryFilter.Contains(c)).ToList();
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
        /// <param name="fiscalScheduleIds">FiscalSchedule identifiers</param>
        /// <returns>FiscalSchedules</returns>
        public virtual List<FiscalSchedule> GetFiscalSchedulesByIds(int[] fiscalScheduleIds)
        {
            if (fiscalScheduleIds == null || fiscalScheduleIds.Length == 0)
                return new List<FiscalSchedule>();

            var query = from p in _fiscalScheduleRepository.Table
                where fiscalScheduleIds.Contains(p.Id) //&& !p.Deleted
                select p;
            
            return query.ToList();
        }

        public List<FiscalSchedule> GetAllFiscalSchedules()
        {
            var query = _fiscalScheduleRepository.Table;
            return query.ToList();
        }

        public string GetNextNumber()
        {
            var query = _fiscalScheduleRepository.Table;
            query = query.Where(a => a.Deleted != true);
            int maxNumber = query.LastOrDefault() != null ? query.LastOrDefault().Id : 0;
            maxNumber += 1;
            return $"t-{maxNumber.ToString("D4")}";
        }


        #endregion
    }
}
