using System;
using System.Collections.Generic;

namespace Research.Data
{
    public partial class Role : BaseEntity
    {
        ICollection<RoleProgram> _rolePrograms;
        ICollection<UserRole> _userRoles;

        public string RoleName { get; set; }
        public string RoleDesc { get; set; }
        public bool IsActive { get; set; }
        public string LastUpdateBy { get; set; }
        public DateTime Created { get; set; }

        public virtual ICollection<RoleProgram> RolePrograms {
            get => _rolePrograms ?? (_rolePrograms = new List<RoleProgram>());
            set => _rolePrograms = value;
        }
        public virtual ICollection<UserRole> UserRoles {
            get => _userRoles ?? (_userRoles = new List<UserRole>());
            set => _userRoles = value;
        }
    }
}