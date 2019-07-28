using System;
using Research.Data;
using System.Linq;
using Research.Core;
using Research.Core.Data;
using Research.Core.Caching;

namespace Research.Services.Researchers
{
    public class ResearcherService : IResearcherService
    {
        private readonly IDataProvider _dataProvider;
        private readonly IDbContext _dbContext;
        private readonly IRepository<Researcher> _researcherRepository;
        private readonly IRepository<ResearcherEducation> _researcherEducationRepository;

        private readonly string _entityName;

        public ResearcherService(IDbContext dbContext,
                                IRepository<Researcher> researcherRepository,
                                IRepository<ResearcherEducation> researcherEducationRepository)
        {
            this._dbContext = dbContext;
            this._researcherRepository = researcherRepository;
            this._researcherEducationRepository = researcherEducationRepository;
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
           // DeleteResearcherEducation(researcherEducation);

        }

        public IPagedList<Researcher> GetAllResearchers(int agency = 0, int personalType = 0, string firstName = null, string lastName = null, int isCompleted = 0, int pageIndex = 0, int pageSize = int.MaxValue, bool getOnlyTotalCount = false)
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

            if (isCompleted != 0)
            {
                bool active = isCompleted == 1 ? true : false;
                query = query.Where(c => c.IsCompleted == active);
            }

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

        public void DeleteResearcherEducation(ResearcherEducation researcherEducation)
        {

            if (researcherEducation == null)
                throw new ArgumentNullException(nameof(researcherEducation));


            _researcherEducationRepository.Delete(researcherEducation);
        }

        /// <summary>
        /// Updates the researcherEducation
        /// </summary>
        /// <param name="researcherEducation">ResearcherEducation</param>
        public virtual void UpdateResearcherEducation(ResearcherEducation researcherEducation)
        {
            if (researcherEducation == null)
                throw new ArgumentNullException(nameof(researcherEducation));

            _researcherEducationRepository.Update(researcherEducation);
        }

        public ResearcherEducation GetResearcherEducationById(int researcherEducationId)
        {
            if (researcherEducationId == 0)
                return null;

            return _researcherEducationRepository.GetById(researcherEducationId);
        }



        public IPagedList<ResearcherEducation> GetAllResearcherEducations(int researcherId = 0, int pageIndex = 0, int pageSize = int.MaxValue, bool getOnlyTotalCount = false)
        {
            var query = _researcherEducationRepository.Table;

            query = query.Where(c => c.ResearcherId == researcherId).OrderByDescending(x => x.DegreeId).ThenByDescending(x=> x.GraduationYear);

            var researcherEducations = new PagedList<ResearcherEducation>(query, pageIndex, pageSize, getOnlyTotalCount);
            return researcherEducations;
        }

        public void InsertResearcherEducation(ResearcherEducation researcherEducation)
        {
            if (researcherEducation == null)
                throw new ArgumentNullException(nameof(researcherEducation));

            if (researcherEducation is IEntityForCaching)
                throw new ArgumentException("Cacheable entities are not supported by Entity Framework");

            _researcherEducationRepository.Insert(researcherEducation);

            //cache
            //_cacheManager.RemoveByPattern(ResearchResearcherEducationDefaults.ResearcherEducationsPatternCacheKey);
            //_staticCacheManager.RemoveByPattern(ResearchResearcherEducationDefaults.ResearcherEducationsPatternCacheKey);

            ////event notification
            //_eventPublisher.EntityInserted(researcherEducation);
        }

        public string GetNextNumber()
        {
            int maxNumber = 0;
            var query = _researcherRepository.Table;
            var researcher = query.OrderByDescending(x=>x.Id).LastOrDefault();
            if (researcher != null)
                maxNumber = int.Parse(researcher.ResearcherCode.Substring(5)) + 1;

            return $"Res-{maxNumber.ToString("D4")}";
        }
    }
}
