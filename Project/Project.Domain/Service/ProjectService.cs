﻿using AutoMapper;
using Project;
using Project.Entity.UnitofWork;
using System.Collections.Generic;

namespace Project.Domain.Service
{
    public class ProjectService : GenericService<ProjectModel, Entity.Project>
    {
        protected IUnitOfWork _unitOfWork;

        //DI must be implemented in specific service as well beside GenericService constructor
        public ProjectService(IUnitOfWork unitOfWork)
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

        public override IEnumerable<ProjectModel> GetAll()
        {
            IEnumerable<Entity.Project> entities = _unitOfWork.GetRepository<Entity.Project>().GetAll();
            return Mapper.Map<IEnumerable<ProjectModel>>(source: entities);
        }

        public IEnumerable<ProjectGridModel> GetGridAll()
        {
            IEnumerable<Entity.Project> entities = _unitOfWork.GetRepository<Entity.Project>().GetAll();
            return Mapper.Map<IEnumerable<ProjectGridModel>>(source: entities);
        }

        public override ProjectModel GetOne(int id)
        {
            var entity = _unitOfWork.GetRepository<Entity.Project>().GetOne(id);
            return Mapper.Map<ProjectModel>(source: entity);
        }
    }
}