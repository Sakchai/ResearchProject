using System;
using System.Collections.Generic;

namespace Project.Entity
{
    public partial class Province : BaseEntity
    {
        public Province()
        {
            Address = new HashSet<Address>();
            District = new HashSet<District>();
        }

        public string Name { get; set; }
        public virtual ICollection<Address> Address { get; set; }
        public virtual ICollection<District> District { get; set; }
    }
}