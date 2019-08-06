using System;
using System.Collections.Generic;

namespace Research.Data
{
    public partial class ProjectHistory : BaseEntity
    {
        public int ProjectId { get; set; }
        public string ProjectNameTh { get; set; }
        public string LastUpdateBy { get; set; }

        public virtual Project Project { get; set; }
    }
}