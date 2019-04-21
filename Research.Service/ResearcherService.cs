using System;
using Research.Data;
using System.Linq;
using Research.Core;
using Research.Core.Data;

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

        public IPagedList<Researcher> GetAllResearchers(string email, string username, string firstName, string lastName, int dayOfBirth, int monthOfBirth, string phone, string zipPostalCode, string ipAddress, int pageIndex, int pageSize, bool getOnlyTotalCount)
        {
            var query = _researcherRepository.Table;

            if (!string.IsNullOrWhiteSpace(email))
                query = query.Where(c => c.Email.Contains(email));

            if (!string.IsNullOrWhiteSpace(firstName))
                query = query.Where(c => c.FirstName.Contains(firstName));

            if (!string.IsNullOrWhiteSpace(lastName))
                query = query.Where(c => c.LastName.Contains(lastName));

            var customers = new PagedList<Researcher>(query, pageIndex, pageSize, getOnlyTotalCount);
            return customers;
        }

        public Researcher GetResearcherById(int? researcherId)
        {
            throw new NotImplementedException();
        }
    }
}
