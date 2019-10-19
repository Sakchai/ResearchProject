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
    /// Institute service    
    /// </summary>
    public partial class InstituteService : IInstituteService
    {
        #region Fields

        private readonly IRepository<Institute> _instituteRepository;
        private readonly IEventPublisher _eventPublisher;
        private readonly ICacheManager _cacheManager;
     //   private readonly IStaticCacheManager _staticCacheManager;

        #endregion

        #region Ctor

        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="cacheManager">Cache manager</param>
        /// <param name="staticCacheManager">Static cache manager</param>
        /// <param name="instituteRepository">Institute repository</param>
        /// <param name="eventPublisher">Event publisher</param>
        public InstituteService(ICacheManager cacheManager,
    //        IStaticCacheManager staticCacheManager,
            IRepository<Institute> instituteRepository,
            IEventPublisher eventPublisher)
        {
            this._cacheManager = cacheManager;
            this._instituteRepository = instituteRepository;
            this._eventPublisher = eventPublisher;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Delete institute
        /// </summary>
        /// <param name="institute">Institute</param>
        public virtual void DeleteInstitute(Institute institute)
        {
            if (institute == null)
                throw new ArgumentNullException(nameof(institute));

            _instituteRepository.Delete(institute);

            //event notification
            _eventPublisher.EntityDeleted(institute);

        }

        
        /// <summary>
        /// Gets all categories
        /// </summary>
        /// <param name="instituteName">Institute name</param>
        /// <param name="pageIndex">Page index</param>
        /// <param name="pageSize">Page size</param>
        /// <param name="showHidden">A value indicating whether to show hidden records</param>
        /// <returns>Institutes</returns>
        public virtual IPagedList<Institute> GetAllInstitutes(string instituteName,
            int pageIndex = 0, int pageSize = int.MaxValue, bool showHidden = false)
        {
            var query = _instituteRepository.Table;
            if (!string.IsNullOrEmpty(instituteName))
                query = query.Where(a => a.Name == instituteName);

            //paging
            return new PagedList<Institute>(query, pageIndex, pageSize);
        }


        /// <summary>
        /// Gets a institute
        /// </summary>
        /// <param name="instituteId">Institute identifier</param>
        /// <returns>Institute</returns>
        public virtual Institute GetInstituteById(int instituteId)
        {
            if (instituteId == 0)
                return null;
            
            var key = string.Format(ResearchInstituteDefaults.InstitutesByIdCacheKey, instituteId);
            return _cacheManager.Get(key, () => _instituteRepository.GetById(instituteId));
        }

        /// <summary>
        /// Inserts institute
        /// </summary>
        /// <param name="institute">Institute</param>
        public virtual void InsertInstitute(Institute institute)
        {
            if (institute == null)
                throw new ArgumentNullException(nameof(institute));

            if (institute is IEntityForCaching)
                throw new ArgumentException("Cacheable entities are not supported by Entity Framework");

            _instituteRepository.Insert(institute);

            //cache
            _cacheManager.RemoveByPattern(ResearchInstituteDefaults.InstitutesPatternCacheKey);
            //_staticCacheManager.RemoveByPattern(ResearchInstituteDefaults.InstitutesPatternCacheKey);

            //event notification
            _eventPublisher.EntityInserted(institute);
        }

        /// <summary>
        /// Updates the institute
        /// </summary>
        /// <param name="institute">Institute</param>
        public virtual void UpdateInstitute(Institute institute)
        {
            if (institute == null)
                throw new ArgumentNullException(nameof(institute));

            if (institute is IEntityForCaching)
                throw new ArgumentException("Cacheable entities are not supported by Entity Framework");


            _instituteRepository.Update(institute);

            //cache
            _cacheManager.RemoveByPattern(ResearchInstituteDefaults.InstitutesPatternCacheKey);
            //_staticCacheManager.RemoveByPattern(ResearchInstituteDefaults.InstitutesPatternCacheKey);

            //event notification
            _eventPublisher.EntityUpdated(institute);
        }
        

        /// <summary>
        /// Returns a list of names of not existing categories
        /// </summary>
        /// <param name="instituteIdsNames">The names and/or IDs of the categories to check</param>
        /// <returns>List of names and/or IDs not existing categories</returns>
        public virtual string[] GetNotExistingInstitutes(string[] instituteIdsNames)
        {
            if (instituteIdsNames == null)
                throw new ArgumentNullException(nameof(instituteIdsNames));

            var query = _instituteRepository.Table;
            var queryFilter = instituteIdsNames.Distinct().ToArray();
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
        /// <param name="instituteIds">Institute identifiers</param>
        /// <returns>Institutes</returns>
        public virtual List<Institute> GetInstitutesByIds(int[] instituteIds)
        {
            if (instituteIds == null || instituteIds.Length == 0)
                return new List<Institute>();

            var query = from p in _instituteRepository.Table
                where instituteIds.Contains(p.Id) //&& !p.Deleted
                select p;
            
            return query.ToList();
        }

        public List<Institute> GetAllInstitutes()
        {
            var query = _instituteRepository.Table;
            return query.ToList();
        }


        #endregion
    }
}
