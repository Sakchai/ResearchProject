using Research.Core;
using Research.Core.Caching;
using Research.Data;
using Research.Services.Events;
using System;
using System.Collections.Generic;
using System.Linq;


namespace Research.Services.Faculties
{
    /// <summary>
    /// Faculty service    
    /// </summary>
    public partial class FacultyService : IFacultyService
    {
        #region Fields

        private readonly IRepository<Faculty> _facultyRepository;
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
        /// <param name="facultyRepository">Faculty repository</param>
        /// <param name="eventPublisher">Event publisher</param>
        public FacultyService(ICacheManager cacheManager,
            IStaticCacheManager staticCacheManager,
            IRepository<Faculty> facultyRepository,
            IEventPublisher eventPublisher)
        {
            this._cacheManager = cacheManager;
            this._facultyRepository = facultyRepository;
            this._eventPublisher = eventPublisher;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Delete faculty
        /// </summary>
        /// <param name="faculty">Faculty</param>
        public virtual void DeleteFaculty(Faculty faculty)
        {
            if (faculty == null)
                throw new ArgumentNullException(nameof(faculty));

            _facultyRepository.Delete(faculty);

            //event notification
            _eventPublisher.EntityDeleted(faculty);

        }

        
        /// <summary>
        /// Gets all categories
        /// </summary>
        /// <param name="facultyName">Faculty name</param>
        /// <param name="pageIndex">Page index</param>
        /// <param name="pageSize">Page size</param>
        /// <param name="showHidden">A value indicating whether to show hidden records</param>
        /// <returns>Faculties</returns>
        public virtual IPagedList<Faculty> GetAllFaculties(string facultyName,
            int pageIndex = 0, int pageSize = int.MaxValue, bool showHidden = false)
        {
            var query = _facultyRepository.Table;
            if (!string.IsNullOrEmpty(facultyName))
                query = query.Where(a => a.Name == facultyName);

            //paging
            return new PagedList<Faculty>(query, pageIndex, pageSize);
        }


        /// <summary>
        /// Gets a faculty
        /// </summary>
        /// <param name="facultyId">Faculty identifier</param>
        /// <returns>Faculty</returns>
        public virtual Faculty GetFacultyById(int facultyId)
        {
            if (facultyId == 0)
                return null;
            
            var key = string.Format(ResearchFacultyDefaults.FacultiesByIdCacheKey, facultyId);
            return _cacheManager.Get(key, () => _facultyRepository.GetById(facultyId));
        }

        /// <summary>
        /// Inserts faculty
        /// </summary>
        /// <param name="faculty">Faculty</param>
        public virtual void InsertFaculty(Faculty faculty)
        {
            if (faculty == null)
                throw new ArgumentNullException(nameof(faculty));

            if (faculty is IEntityForCaching)
                throw new ArgumentException("Cacheable entities are not supported by Entity Framework");

            _facultyRepository.Insert(faculty);

            //cache
            _cacheManager.RemoveByPattern(ResearchFacultyDefaults.FacultiesPatternCacheKey);
            _staticCacheManager.RemoveByPattern(ResearchFacultyDefaults.FacultiesPatternCacheKey);

            //event notification
            _eventPublisher.EntityInserted(faculty);
        }

        /// <summary>
        /// Updates the faculty
        /// </summary>
        /// <param name="faculty">Faculty</param>
        public virtual void UpdateFaculty(Faculty faculty)
        {
            if (faculty == null)
                throw new ArgumentNullException(nameof(faculty));

            if (faculty is IEntityForCaching)
                throw new ArgumentException("Cacheable entities are not supported by Entity Framework");


            _facultyRepository.Update(faculty);

            //cache
            _cacheManager.RemoveByPattern(ResearchFacultyDefaults.FacultiesPatternCacheKey);
            _staticCacheManager.RemoveByPattern(ResearchFacultyDefaults.FacultiesPatternCacheKey);

            //event notification
            _eventPublisher.EntityUpdated(faculty);
        }
        

        /// <summary>
        /// Returns a list of names of not existing categories
        /// </summary>
        /// <param name="facultyIdsNames">The names and/or IDs of the categories to check</param>
        /// <returns>List of names and/or IDs not existing categories</returns>
        public virtual string[] GetNotExistingFaculties(string[] facultyIdsNames)
        {
            if (facultyIdsNames == null)
                throw new ArgumentNullException(nameof(facultyIdsNames));

            var query = _facultyRepository.Table;
            var queryFilter = facultyIdsNames.Distinct().ToArray();
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
        /// <param name="facultyIds">Faculty identifiers</param>
        /// <returns>Faculties</returns>
        public virtual List<Faculty> GetFacultiesByIds(int[] facultyIds)
        {
            if (facultyIds == null || facultyIds.Length == 0)
                return new List<Faculty>();

            var query = from p in _facultyRepository.Table
                where facultyIds.Contains(p.Id) //&& !p.Deleted
                select p;
            
            return query.ToList();
        }

        public List<Faculty> GetAllFaculties()
        {
            var query = _facultyRepository.Table;
            return query.ToList();
        }


        #endregion
    }
}
