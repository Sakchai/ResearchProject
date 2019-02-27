using System;
using System.Collections.Generic;
using System.Text;

namespace Project.Entity.Repository
{
    public interface IRoleRepository : IRepository<Role>
    {
        //Role GetOne(object id);
        IEnumerable<Role> GetUserRoles(object userId);
        //IEnumerable<Role> GetAll();
    }
}
