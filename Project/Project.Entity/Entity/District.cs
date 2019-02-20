using System;
using System.Collections.Generic;

namespace Project.Entity
{
    public partial class District : BaseEntity
    {
        public District()
        {
            Address = new HashSet<Address>();
            Subdistrict = new HashSet<Subdistrict>();
        }

        public string Name { get; set; }
        public int ProvinceId { get; set; }
        public virtual Province Province { get; set; }
        public virtual ICollection<Address> Address { get; set; }
        public virtual ICollection<Subdistrict> Subdistrict { get; set; }
    }
}