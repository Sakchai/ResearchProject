using System;
using System.Collections.Generic;

namespace Project.Entity
{
    public partial class Address : BaseEntity
    {
        public Address()
        {
            ProfessorAddress = new HashSet<ProfessorAddress>();
            ResearcherAddresses = new HashSet<ResearcherAddresses>();
        }

        public int? ProvinceId { get; set; }
        public int? DistrictId { get; set; }
        public int? SubdistrictId { get; set; }
        public AddressType AddressType { get; set; }
        public string Address1 { get; set; }
        public string ZipCode { get; set; }

        public virtual District District { get; set; }
        public virtual Province Province { get; set; }
        public virtual Subdistrict Subdistrict { get; set; }
        public virtual ICollection<ProfessorAddress> ProfessorAddress { get; set; }
        public virtual ICollection<ResearcherAddresses> ResearcherAddresses { get; set; }
    }

    public enum AddressType
    {
        CurrentResidence = 1,
        HouseAddress = 2,
    }
}