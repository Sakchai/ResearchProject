using System;
using System.Collections.Generic;

namespace Research.Data
{
    public partial class Program : BaseEntity
    {
        ICollection<RoleProgram> _rolePrograms;
        //public Program()
        //{
        //}

        public string ProgramCode { get; set; }
        public string ProgramName { get; set; }
        public string ProgramUrl { get; set; }
        public string LastUpdateBy { get; set; }

        public virtual ICollection<RoleProgram> RolePrograms {
            get => _rolePrograms ?? (_rolePrograms = new List<RoleProgram>());
            set => _rolePrograms = value;
        }
    }
}