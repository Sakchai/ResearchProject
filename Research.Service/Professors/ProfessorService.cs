using Research.Core;
using Research.Core.Caching;
using Research.Data;
using Research.Services.Events;
using System;
using System.Collections.Generic;
using System.Linq;


namespace Research.Services.Professors
{
    /// <summary>
    /// Professor service    
    /// </summary>
    public partial class ProfessorService : IProfessorService
    {
        #region Fields

        private readonly IRepository<Professor> _professorRepository;
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
        /// <param name="professorRepository">Professor repository</param>
        /// <param name="eventPublisher">Event publisher</param>
        public ProfessorService(ICacheManager cacheManager,
            IStaticCacheManager staticCacheManager,
            IRepository<Professor> professorRepository,
            IEventPublisher eventPublisher)
        {
            this._cacheManager = cacheManager;
            this._professorRepository = professorRepository;
            this._eventPublisher = eventPublisher;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Delete professor
        /// </summary>
        /// <param name="professor">Professor</param>
        public virtual void DeleteProfessor(Professor professor)
        {
            if (professor == null)
                throw new ArgumentNullException(nameof(professor));

            _professorRepository.Delete(professor);

            //event notification
            _eventPublisher.EntityDeleted(professor);

        }

        
        /// <summary>
        /// Gets all categories
        /// </summary>
        /// <param name="professorName">Professor name</param>
        /// <param name="pageIndex">Page index</param>
        /// <param name="pageSize">Page size</param>
        /// <param name="showHidden">A value indicating whether to show hidden records</param>
        /// <returns>Professors</returns>
        public virtual IPagedList<Professor> GetAllProfessors(string professorName,
            int pageIndex = 0, int pageSize = int.MaxValue, bool showHidden = false)
        {
            var query = _professorRepository.Table;
            if (!string.IsNullOrEmpty(professorName))
                query = query.Where(a => a.FirstName == professorName);

            //paging
            return new PagedList<Professor>(query, pageIndex, pageSize);
        }


        /// <summary>
        /// Gets a professor
        /// </summary>
        /// <param name="professorId">Professor identifier</param>
        /// <returns>Professor</returns>
        public virtual Professor GetProfessorById(int professorId)
        {
            if (professorId == 0)
                return null;
            
            var key = string.Format(ResearchProfessorDefaults.ProfessorsByIdCacheKey, professorId);
            return _cacheManager.Get(key, () => _professorRepository.GetById(professorId));
        }

        /// <summary>
        /// Inserts professor
        /// </summary>
        /// <param name="professor">Professor</param>
        public virtual void InsertProfessor(Professor professor)
        {
            if (professor == null)
                throw new ArgumentNullException(nameof(professor));

            if (professor is IEntityForCaching)
                throw new ArgumentException("Cacheable entities are not supported by Entity Framework");

            _professorRepository.Insert(professor);

            //cache
            _cacheManager.RemoveByPattern(ResearchProfessorDefaults.ProfessorsPatternCacheKey);
            _staticCacheManager.RemoveByPattern(ResearchProfessorDefaults.ProfessorsPatternCacheKey);

            //event notification
            _eventPublisher.EntityInserted(professor);
        }

        /// <summary>
        /// Updates the professor
        /// </summary>
        /// <param name="professor">Professor</param>
        public virtual void UpdateProfessor(Professor professor)
        {
            if (professor == null)
                throw new ArgumentNullException(nameof(professor));

            if (professor is IEntityForCaching)
                throw new ArgumentException("Cacheable entities are not supported by Entity Framework");


            _professorRepository.Update(professor);

            //cache
            _cacheManager.RemoveByPattern(ResearchProfessorDefaults.ProfessorsPatternCacheKey);
            _staticCacheManager.RemoveByPattern(ResearchProfessorDefaults.ProfessorsPatternCacheKey);

            //event notification
            _eventPublisher.EntityUpdated(professor);
        }
        

        /// <summary>
        /// Returns a list of names of not existing categories
        /// </summary>
        /// <param name="professorIdsNames">The names and/or IDs of the categories to check</param>
        /// <returns>List of names and/or IDs not existing categories</returns>
        public virtual string[] GetNotExistingProfessors(string[] professorIdsNames)
        {
            if (professorIdsNames == null)
                throw new ArgumentNullException(nameof(professorIdsNames));

            var query = _professorRepository.Table;
            var queryFilter = professorIdsNames.Distinct().ToArray();
            //filtering by name
            var filter = query.Select(c => c.FirstName).Where(c => queryFilter.Contains(c)).ToList();
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
        /// <param name="professorIds">Professor identifiers</param>
        /// <returns>Professors</returns>
        public virtual List<Professor> GetProfessorsByIds(int[] professorIds)
        {
            if (professorIds == null || professorIds.Length == 0)
                return new List<Professor>();

            var query = from p in _professorRepository.Table
                where professorIds.Contains(p.Id) //&& !p.Deleted
                select p;
            
            return query.ToList();
        }

        public List<Professor> GetAllProfessors()
        {
            var query = _professorRepository.Table;
            return query.ToList();
        }


        #endregion
    }
}
