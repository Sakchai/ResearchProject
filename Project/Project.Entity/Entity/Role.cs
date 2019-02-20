using System;
using System.Collections.Generic;

namespace Project.Entity
{
    public partial class Role : BaseEntity
    {
        public Role()
        {
            RoleProgram = new HashSet<RoleProgram>();
            UserRole = new HashSet<UserRole>();
        }

        public string RoleName { get; set; }
        public bool? IsActive { get; set; }
        public string LastUpdateBy { get; set; }

        public virtual ICollection<RoleProgram> RoleProgram { get; set; }
        public virtual ICollection<UserRole> UserRole { get; set; }
    }
}