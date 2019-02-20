using System;
using System.Collections.Generic;

namespace Project.Entity
{
    public partial class ProfessorAddress : BaseEntity
    {
        public int ProfessorId { get; set; }
        public int AddressId { get; set; }

        public virtual Address Address { get; set; }
        public virtual Professor Professor { get; set; }
    }
}