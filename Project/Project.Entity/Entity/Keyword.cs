using System;
using System.Collections.Generic;

namespace Project.Entity
{
    public partial class Keyword
    {
        public Keyword()
        {
            ProjectKeyword = new HashSet<ProjectKeyword>();
        }

        public string Name { get; set; }

        public virtual ICollection<ProjectKeyword> ProjectKeyword { get; set; }
    }
}