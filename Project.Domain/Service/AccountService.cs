using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using AutoMapper;
using Project.Entity;
using Project.Entity.UnitofWork;

namespace Project.Domain.Service
{

    public class AccountService<Tv, Te> : GenericService<Tv, Te>
                                        where Tv : AccountViewModel
                                        where Te : Account
    {
        //DI must be implemented in specific service as well beside GenericService constructor
        public AccountService(IUnitOfWork unitOfWork)
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
    }


}
