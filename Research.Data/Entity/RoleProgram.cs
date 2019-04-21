using System;
using System.Collections.Generic;

namespace Research.Data
{
    public partial class RoleProgram : BaseEntity
    {
        public int RoleId { get; set; }
        public int ProgramId { get; set; }
        public bool? CanAdd { get; set; }
        public bool? CanView { get; set; }
        public bool? CanEdit { get; set; }
        public bool? CanDelete { get; set; }
        public bool? CanPrint { get; set; }
        public bool? CanEmail { get; set; }
        public string LastUpdateBy { get; set; }

        public virtual Program Program { get; set; }
        public virtual Role Role { get; set; }
    }
}