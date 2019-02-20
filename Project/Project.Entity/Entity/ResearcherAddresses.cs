using System;
using System.Collections.Generic;

namespace Project.Entity
{
    public partial class ResearcherAddresses : BaseEntity
    {
        public int ResearcherId { get; set; }
        public int AddressId { get; set; }

        public virtual Address Address { get; set; }
        public virtual Researcher Researcher { get; set; }
    }
}