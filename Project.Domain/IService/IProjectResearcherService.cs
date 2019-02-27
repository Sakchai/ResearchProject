using System.Collections.Generic;
using Project.Entity;

namespace Project.Domain.Service
{
    public interface IProjectResearcherService : IService<ProjectResearcherViewModel, ProjectResearcher>
    {
        ProjectResearcherViewModel GetOne(int projectId, int researcherId);
        IEnumerable<ProjectResearcherViewModel> GetProjectResearcherByProjectId(int projectId);
    }
}