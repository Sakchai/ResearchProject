using System;
using System.Collections.Generic;

namespace Research.Data
{
    public partial class Address : BaseEntity
    {
        //private ICollection<Professor> _professors;
        //private ICollection<Researcher> _researchers;

        public int? ProvinceId { get; set; }

        public string Address1 { get; set; }
        public string Address2 { get; set; }

        public string ZipCode { get; set; }

        public virtual Province Province { get; set; }

        //public virtual ICollection<Professor> Professors {
        //    get => _professors ?? (_professors = new List<Professor>());
        //    set => _professors = value;
        //}
        //public virtual ICollection<Researcher> Researchers {
        //    get => _researchers ?? (_researchers = new List<Researcher>());
        //    set => _researchers = value;
        //}
    }

    public enum AddressType
    {
        CurrentResidence = 1,
        HouseAddress = 2,
    }
}