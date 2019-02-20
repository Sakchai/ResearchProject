using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using AutoMapper;
using Project.Entity;
using Project.Entity.UnitofWork;

namespace Project.Domain.Service
{

    public class RoleServiceAsync<Tv, Te> : GenericServiceAsync<Tv, Te>
                                                where Tv : RoleViewModel
                                                where Te : Role
    {
        //DI must be implemented specific service as well beside GenericAsyncService constructor
        public RoleServiceAsync(IUnitOfWork unitOfWork)
        {
            if (_unitOfWork == null)
                _unitOfWork = unitOfWork;
        }

        //add any custom service method or override genericasync service method
        //...
    }

}
