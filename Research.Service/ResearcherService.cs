using System;
using Research.Data;
using System.Linq;
using Research.Core;
using Research.Core.Data;
using Research.Core.Caching;

namespace Research.Services
{
    public class ResearcherService : IResearcherService
    {
        private readonly IDataProvider _dataProvider;
        private readonly IDbContext _dbContext;
        private readonly IRepository<Researcher> _researcherRepository;
        private readonly string _entityName;

        public ResearcherService(IDbContext dbContext,
                                IRepository<Researcher> researcherRepository)
        {
            this._dbContext = dbContext;
            this._researcherRepository = researcherRepository;
            this._entityName = typeof(Researcher).Name;
        }
        public void DeleteResearcher(Researcher researcher)
        {

            if (researcher == null)
                throw new ArgumentNullException(nameof(researcher));

            researcher.Deleted = true;

            UpdateResearcher(researcher);
        }

        /// <summary>
        /// Updates the researcher
        /// </summary>
        /// <param name="researcher">Researcher</param>
        public virtual void UpdateResearcher(Researcher researcher)
        {
            if (researcher == null)
                throw new ArgumentNullException(nameof(researcher));

            _researcherRepository.Update(researcher);
        }

        public Researcher GetResearcherById(int researcherId)
        {
            if (researcherId == 0)
                return null;

            return _researcherRepository.GetById(researcherId);
        }

        public Researcher GetResearcherByEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return null;

            var query = from c in _researcherRepository.Table
                        orderby c.Id
                        where c.Email == email
                        select c;
            var customer = query.FirstOrDefault();
            return customer;
        }

        public void RemoveResearcherEducation(Researcher researcher, ResearcherEducation researcherEducation)
        {
            if (researcher == null)
                throw new ArgumentNullException(nameof(researcher));

            if (researcherEducation == null)
                throw new ArgumentNullException(nameof(researcherEducation));

            researcher.ResearcherEducations.Remove(researcherEducation);

        }

        public IPagedList<Researcher> GetAllResearchers(int agency = 0, int personalType = 0, string firstName = null, string lastName = null, bool isActive = true, int pageIndex = 0, int pageSize = int.MaxValue, bool getOnlyTotalCount = false)
        {
            var query = _researcherRepository.Table;
            query = query.Where(c => c.Deleted != true);

            if (agency != 0)
                query = query.Where(c => c.Agency.Id == agency);

            if (personalType != 0)
                query = query.Where(c => c.PersonalTypeId == personalType);

            if (!string.IsNullOrWhiteSpace(firstName))
                query = query.Where(c => c.FirstName.Contains(firstName));

            if (!string.IsNullOrWhiteSpace(lastName))
                query = query.Where(c => c.LastName.Contains(lastName));

            //query = query.Where(c => c.IsActive == isActive);
            var customers = new PagedList<Researcher>(query, pageIndex, pageSize, getOnlyTotalCount);
            return customers;
        }

        public void InsertResearcher(Researcher researcher)
        {
            if (researcher == null)
                throw new ArgumentNullException(nameof(researcher));

            if (researcher is IEntityForCaching)
                throw new ArgumentException("Cacheable entities are not supported by Entity Framework");

            _researcherRepository.Insert(researcher);

            //cache
            //_cacheManager.RemoveByPattern(ResearchResearcherDefaults.ResearchersPatternCacheKey);
            //_staticCacheManager.RemoveByPattern(ResearchResearcherDefaults.ResearchersPatternCacheKey);

            ////event notification
            //_eventPublisher.EntityInserted(researcher);
        }


    }
}
