using AutoMapper;
using System;
using System.Collections.Generic;
using Project.Entity;
using Project.Entity.UnitofWork;

namespace Project.Domain.Service
{
    public class ResearcherService : GenericService<ResearcherViewModel,Researcher>, IResearcherService
    {
        protected IUnitOfWork _unitOfWork;

        //DI must be implemented in specific service as well beside GenericService constructor
        public ResearcherService(IUnitOfWork unitOfWork)
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

        public override IEnumerable<ResearcherViewModel> GetAll()
        {
            IEnumerable<Researcher> entities = _unitOfWork.GetRepository<Researcher>().GetAll();
            return Mapper.Map<IEnumerable<ResearcherViewModel>>(source: entities);
        }

        public override ResearcherViewModel GetOne(int id)
        {
            var entity = _unitOfWork.GetRepository<Researcher>().GetOne(id);
            return Mapper.Map<ResearcherViewModel>(source: entity);
        }

        public IEnumerable<ResearcherViewModel> GetResearchersByProjectId(int projectid)
        {
            var entities = _unitOfWork.GetResearcherRepository().GetResearchersByProjectId(projectid);
            return Mapper.Map<IEnumerable<ResearcherViewModel>>(source: entities);
        }

        public ResearcherViewModel GetResearcherUserByEmail(string email)
        {
            var entity = _unitOfWork.GetResearcherRepository().GetResearcherUserByEmail(email);
            return Mapper.Map<ResearcherViewModel>(source: entity);
        }


    }
}