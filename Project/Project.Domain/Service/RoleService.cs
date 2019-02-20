using AutoMapper;
using System;
using System.Collections.Generic;
using Project.Entity;
using Project.Entity.UnitofWork;

namespace Project.Domain.Service
{
    public class RoleService : GenericService<RoleViewModel,Role>, IRoleService
    {
        protected IUnitOfWork _unitOfWork;

        //DI must be implemented in specific service as well beside GenericService constructor
        public RoleService(IUnitOfWork unitOfWork)
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

        public override IEnumerable<RoleViewModel> GetAll()
        {
            IEnumerable<Role> entities = _unitOfWork.GetRepository<Role>().GetAll();
            return Mapper.Map<IEnumerable<RoleViewModel>>(source: entities);
        }

        public override RoleViewModel GetOne(int id)
        {
            var entity = _unitOfWork.GetRepository<Role>().GetOne(id);
            return Mapper.Map<RoleViewModel>(source: entity);
        }

        public IEnumerable<RoleViewModel> GetUserRoles(int id)
        {
            IEnumerable<Role> entities = _unitOfWork.GetRoleRepository().GetUserRoles(id);
            return Mapper.Map<IEnumerable<RoleViewModel>>(source: entities);
        }


    }
}