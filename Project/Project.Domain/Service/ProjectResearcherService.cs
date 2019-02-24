using AutoMapper;
using Project;
using Project.Entity;
using Project.Entity.UnitofWork;
using System.Collections.Generic;

namespace Project.Domain.Service
{
    public class ProjectResearcherService : GenericService<ProjectResearcherViewModel, ProjectResearcher>, IProjectResearcherService
    {
        protected IUnitOfWork _unitOfWork;

        //DI must be implemented in specific service as well beside GenericService constructor
        public ProjectResearcherService(IUnitOfWork unitOfWork)
        {
            if (_unitOfWork == null)
                _unitOfWork = unitOfWork;
        }

        //add any custom service method or override generic service method
        //...test, it can be removed
        public bool DoNothing()
        {
            return true;
        }

        public override IEnumerable<ProjectResearcherViewModel> GetAll()
        {
            IEnumerable<Entity.ProjectResearcher> entities = _unitOfWork.GetRepository<Entity.ProjectResearcher>().GetAll();
            return Mapper.Map<IEnumerable<ProjectResearcherViewModel>>(source: entities);
        }


        public ProjectResearcherViewModel GetOne(int projectId, int researcherId)
        {
            var entity = _unitOfWork.GetProjectResearcherRepository().GetProjectResearcher(projectId, researcherId);
            return Mapper.Map<ProjectResearcherViewModel>(source: entity);
        }

        //public ProjectResearcherModel GetProjectResearcherByProjectId(int projectId, int researcherId)
        //{
        //    var entity = _unitOfWork.GetProjectResearcherRepository().GetProjectResearcher(projectId, researcherId);
        //    return Mapper.Map<ProjectResearcherModel>(source: entity);
        //}

        public IEnumerable<ProjectResearcherViewModel> GetProjectResearcherByProjectId(int projectId)
        {
            IEnumerable<Entity.ProjectResearcher> entities = _unitOfWork.GetProjectResearcherRepository().GetProjectResearcherByProjectId(projectId);
            return Mapper.Map<IEnumerable<ProjectResearcherViewModel>>(source: entities);
        }
    }
}