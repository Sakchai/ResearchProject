using System.Collections.Generic;
using Project.Entity;

namespace Project.Domain.Service
{
    public interface IRoleService : IService<RoleViewModel,Role>
    {
        IEnumerable<RoleViewModel> GetUserRoles(int id);
    }
}