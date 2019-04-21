using System;
using System.Collections.Generic;

namespace Research.Data
{
    public partial class ProfessorHistory : BaseEntity
    {
        public int ProfessorId { get; set; }
        public int TitleId { get; set; }
        public string TitleName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime Created {get; set;}
        public virtual Professor Professor { get; set; }
        public virtual Title Title { get; set; }
    }
}