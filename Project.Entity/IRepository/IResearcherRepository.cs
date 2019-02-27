using System.Collections.Generic;

namespace Project.Entity.Repository
{
    public interface IResearcherRepository : IRepository<Researcher>
    {
        IEnumerable<Researcher> GetResearchersByProjectId(int projectid);

        Researcher GetResearcherUserByEmail(string email);
    }
}