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
    /// AcademicRank service    
    /// </summary>
    public partial class AcademicRankService : IAcademicRankService
    {
        #region Fields

        private readonly IRepository<AcademicRank> _academicRankRepository;
        private readonly IEventPublisher _eventPublisher;
        private readonly ICacheManager _cacheManager;
      //  private readonly IStaticCacheManager _staticCacheManager;

        #endregion

        #region Ctor

        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="cacheManager">Cache manager</param>
        /// <param name="staticCacheManager">Static cache manager</param>
        /// <param name="academicRankRepository">AcademicRank repository</param>
        /// <param name="eventPublisher">Event publisher</param>
        public AcademicRankService(ICacheManager cacheManager,
          //  IStaticCacheManager staticCacheManager,
            IRepository<AcademicRank> academicRankRepository,
            IEventPublisher eventPublisher)
        {
            this._cacheManager = cacheManager;
            this._academicRankRepository = academicRankRepository;
            this._eventPublisher = eventPublisher;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Delete academicRank
        /// </summary>
        /// <param name="academicRank">AcademicRank</param>
        public virtual void DeleteAcademicRank(AcademicRank academicRank)
        {
            if (academicRank == null)
                throw new ArgumentNullException(nameof(academicRank));

            _academicRankRepository.Delete(academicRank);

            //event notification
            _eventPublisher.EntityDeleted(academicRank);

        }

        
        /// <summary>
        /// Gets all categories
        /// </summary>
        /// <param name="academicRankName">AcademicRank name</param>
        /// <param name="pageIndex">Page index</param>
        /// <param name="pageSize">Page size</param>
        /// <param name="showHidden">A value indicating whether to show hidden records</param>
        /// <returns>AcademicRanks</returns>
        public virtual IPagedList<AcademicRank> GetAllAcademicRanks(string academicRankName,
            int pageIndex = 0, int pageSize = int.MaxValue, bool showHidden = false)
        {
            var query = _academicRankRepository.Table;
            if (!string.IsNullOrEmpty(academicRankName))
                query = query.Where(a => a.NameTh == academicRankName);

            //paging
            return new PagedList<AcademicRank>(query, pageIndex, pageSize);
        }


        /// <summary>
        /// Gets a academicRank
        /// </summary>
        /// <param name="academicRankId">AcademicRank identifier</param>
        /// <returns>AcademicRank</returns>
        public virtual AcademicRank GetAcademicRankById(int academicRankId)
        {
            if (academicRankId == 0)
                return null;
            
            var key = string.Format(ResearchAcademicRankDefaults.AcademicRanksByIdCacheKey, academicRankId);
            return _cacheManager.Get(key, () => _academicRankRepository.GetById(academicRankId));
        }

        /// <summary>
        /// Inserts academicRank
        /// </summary>
        /// <param name="academicRank">AcademicRank</param>
        public virtual void InsertAcademicRank(AcademicRank academicRank)
        {
            if (academicRank == null)
                throw new ArgumentNullException(nameof(academicRank));

            if (academicRank is IEntityForCaching)
                throw new ArgumentException("Cacheable entities are not supported by Entity Framework");

            _academicRankRepository.Insert(academicRank);

            //cache
            _cacheManager.RemoveByPattern(ResearchAcademicRankDefaults.AcademicRanksPatternCacheKey);
            //_staticCacheManager.RemoveByPattern(ResearchAcademicRankDefaults.AcademicRanksPatternCacheKey);

            //event notification
            _eventPublisher.EntityInserted(academicRank);
        }

        /// <summary>
        /// Updates the academicRank
        /// </summary>
        /// <param name="academicRank">AcademicRank</param>
        public virtual void UpdateAcademicRank(AcademicRank academicRank)
        {
            if (academicRank == null)
                throw new ArgumentNullException(nameof(academicRank));

            if (academicRank is IEntityForCaching)
                throw new ArgumentException("Cacheable entities are not supported by Entity Framework");


            _academicRankRepository.Update(academicRank);

            //cache
            _cacheManager.RemoveByPattern(ResearchAcademicRankDefaults.AcademicRanksPatternCacheKey);
            //_staticCacheManager.RemoveByPattern(ResearchAcademicRankDefaults.AcademicRanksPatternCacheKey);

            //event notification
            _eventPublisher.EntityUpdated(academicRank);
        }
        

        /// <summary>
        /// Returns a list of names of not existing categories
        /// </summary>
        /// <param name="academicRankIdsNames">The names and/or IDs of the categories to check</param>
        /// <returns>List of names and/or IDs not existing categories</returns>
        public virtual string[] GetNotExistingAcademicRanks(string[] academicRankIdsNames)
        {
            if (academicRankIdsNames == null)
                throw new ArgumentNullException(nameof(academicRankIdsNames));

            var query = _academicRankRepository.Table;
            var queryFilter = academicRankIdsNames.Distinct().ToArray();
            //filtering by name
            var filter = query.Select(c => c.NameTh).Where(c => queryFilter.Contains(c)).ToList();
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
        /// <param name="academicRankIds">AcademicRank identifiers</param>
        /// <returns>AcademicRanks</returns>
        public virtual List<AcademicRank> GetAcademicRanksByIds(int[] academicRankIds)
        {
            if (academicRankIds == null || academicRankIds.Length == 0)
                return new List<AcademicRank>();

            var query = from p in _academicRankRepository.Table
                where academicRankIds.Contains(p.Id) //&& !p.Deleted
                select p;
            
            return query.ToList();
        }

        public List<AcademicRank> GetAllAcademicRanks()
        {
            var query = _academicRankRepository.Table;
            return query.ToList();
        }

        public IList<AcademicRank> GetAcademicRanksByPersonalTypeId(int personalTypeId)
        {
            var query = from sp in _academicRankRepository.Table
                        orderby sp.NameTh
                        where sp.PersonTypeId == personalTypeId 
                        select sp;
            return query.ToList();
        }


        #endregion
    }
}
