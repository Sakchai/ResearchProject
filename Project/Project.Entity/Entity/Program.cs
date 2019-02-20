using System;
using System.Collections.Generic;

namespace Project.Entity
{
    public partial class Program : BaseEntity
    {
        public Program()
        {
            RoleProgram = new HashSet<RoleProgram>();
        }

        public string ProgramCode { get; set; }
        public string ProgramName { get; set; }
        public string ProgramUrl { get; set; }
        public string LastUpdateBy { get; set; }

        public virtual ICollection<RoleProgram> RoleProgram { get; set; }
    }
}