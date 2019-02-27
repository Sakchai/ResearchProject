using System;
using System.Collections.Generic;

namespace Project.Entity
{
    public partial class ProjectKeyword : BaseEntity
    {
        public int KeywordId { get; set; }
        public int ProjectId { get; set; }

        public virtual Keyword Keyword { get; set; }
        public virtual Project Project { get; set; }
    }
}