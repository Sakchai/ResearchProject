using System;
using System.Collections.Generic;

namespace Project.Entity
{
    public partial class Subdistrict : BaseEntity
    {
        public Subdistrict()
        {
            Address = new HashSet<Address>();
        }

        public string Name { get; set; }
        public string ZipPostalCode { get; set; }
        public int DistrictId { get; set; }

        public virtual District District { get; set; }
        public virtual ICollection<Address> Address { get; set; }
    }
}