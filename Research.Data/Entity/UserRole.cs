using System;
using System.Collections.Generic;

namespace Research.Data
{
    public partial class UserRole : BaseEntity
    {
        ICollection<RoleProgram> _rolePrograms;
        public int UserId { get; set; }
        public int RoleId { get; set; }

        public virtual Role Role { get; set; }
        public virtual User User { get; set; }
        public virtual ICollection<RoleProgram> RolePrograms {
            get => _rolePrograms ?? (_rolePrograms = new List<RoleProgram>());
            set => _rolePrograms = value;
        }
    }
}