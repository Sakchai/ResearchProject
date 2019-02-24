using System.Collections.Generic;

namespace Project.Entity.Repository
{
    public interface IProjectResearcherRepository : IRepository<ProjectResearcher>
    {
        ProjectResearcher GetProjectResearcher(int projectId, int researcherId);
        IEnumerable<ProjectResearcher> GetProjectResearcherByProjectId(int projectid);
    }
}