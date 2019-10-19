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
    /// Agency service    
    /// </summary>
    public partial class AgencyService : IAgencyService
    {
        #region Fields

        private readonly IRepository<Agency> _agencyRepository;
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
        /// <param name="agencyRepository">Agency repository</param>
        /// <param name="eventPublisher">Event publisher</param>
        public AgencyService(ICacheManager cacheManager,
        //    IStaticCacheManager staticCacheManager,
            IRepository<Agency> agencyRepository,
            IEventPublisher eventPublisher)
        {
            this._cacheManager = cacheManager;
            this._agencyRepository = agencyRepository;
            this._eventPublisher = eventPublisher;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Delete agency
        /// </summary>
        /// <param name="agency">Agency</param>
        public virtual void DeleteAgency(Agency agency)
        {
            if (agency == null)
                throw new ArgumentNullException(nameof(agency));

            _agencyRepository.Delete(agency);

            //event notification
            _eventPublisher.EntityDeleted(agency);

        }

        
        /// <summary>
        /// Gets all categories
        /// </summary>
        /// <param name="agencyName">Agency name</param>
        /// <param name="pageIndex">Page index</param>
        /// <param name="pageSize">Page size</param>
        /// <param name="showHidden">A value indicating whether to show hidden records</param>
        /// <returns>Agencys</returns>
        public virtual IPagedList<Agency> GetAllAgencys(string agencyName,
            int pageIndex = 0, int pageSize = int.MaxValue, bool showHidden = false)
        {
            var query = _agencyRepository.Table;
            if (!string.IsNullOrEmpty(agencyName))
                query = query.Where(a => a.Name == agencyName);

            //paging
            return new PagedList<Agency>(query, pageIndex, pageSize);
        }


        /// <summary>
        /// Gets a agency
        /// </summary>
        /// <param name="agencyId">Agency identifier</param>
        /// <returns>Agency</returns>
        public virtual Agency GetAgencyById(int agencyId)
        {
            if (agencyId == 0)
                return null;
            
            var key = string.Format(ResearchAgencyDefaults.AgenciesByIdCacheKey, agencyId);
            return _cacheManager.Get(key, () => _agencyRepository.GetById(agencyId));
        }

        /// <summary>
        /// Inserts agency
        /// </summary>
        /// <param name="agency">Agency</param>
        public virtual void InsertAgency(Agency agency)
        {
            if (agency == null)
                throw new ArgumentNullException(nameof(agency));

            if (agency is IEntityForCaching)
                throw new ArgumentException("Cacheable entities are not supported by Entity Framework");

            _agencyRepository.Insert(agency);

            //cache
            _cacheManager.RemoveByPattern(ResearchAgencyDefaults.AgenciesPatternCacheKey);
            //_staticCacheManager.RemoveByPattern(ResearchAgencyDefaults.AgenciesPatternCacheKey);

            //event notification
            _eventPublisher.EntityInserted(agency);
        }

        /// <summary>
        /// Updates the agency
        /// </summary>
        /// <param name="agency">Agency</param>
        public virtual void UpdateAgency(Agency agency)
        {
            if (agency == null)
                throw new ArgumentNullException(nameof(agency));

            if (agency is IEntityForCaching)
                throw new ArgumentException("Cacheable entities are not supported by Entity Framework");


            _agencyRepository.Update(agency);

            //cache
            _cacheManager.RemoveByPattern(ResearchAgencyDefaults.AgenciesPatternCacheKey);
            //_staticCacheManager.RemoveByPattern(ResearchAgencyDefaults.AgenciesPatternCacheKey);

            //event notification
            _eventPublisher.EntityUpdated(agency);
        }
        

        /// <summary>
        /// Returns a list of names of not existing categories
        /// </summary>
        /// <param name="agencyIdsNames">The names and/or IDs of the categories to check</param>
        /// <returns>List of names and/or IDs not existing categories</returns>
        public virtual string[] GetNotExistingAgencys(string[] agencyIdsNames)
        {
            if (agencyIdsNames == null)
                throw new ArgumentNullException(nameof(agencyIdsNames));

            var query = _agencyRepository.Table;
            var queryFilter = agencyIdsNames.Distinct().ToArray();
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
        /// <param name="agencyIds">Agency identifiers</param>
        /// <returns>Agencys</returns>
        public virtual List<Agency> GetAgencysByIds(int[] agencyIds)
        {
            if (agencyIds == null || agencyIds.Length == 0)
                return new List<Agency>();

            var query = from p in _agencyRepository.Table
                where agencyIds.Contains(p.Id) //&& !p.Deleted
                select p;
            
            return query.ToList();
        }

        public List<Agency> GetAllAgencies()
        {
            var query = _agencyRepository.Table;
            return query.ToList();
        }


        #endregion
    }
}
