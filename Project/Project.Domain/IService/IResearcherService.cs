using System.Collections.Generic;
using Project.Entity;

namespace Project.Domain.Service
{
    public interface IResearcherService : IService<ResearcherViewModel, Researcher>
    {
        IEnumerable<ResearcherViewModel> GetResearchersByProjectId(int projectid);

        ResearcherViewModel GetResearcherUserByEmail(string email);
    }
}