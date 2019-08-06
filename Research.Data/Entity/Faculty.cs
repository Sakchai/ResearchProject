using System;
using System.Collections.Generic;

namespace Research.Data
{
    public partial class Faculty : BaseEntity
    {
        //private ICollection<Researcher> _researchers;
        //private ICollection<User> _users;

        public string Name { get; set; }
        //public virtual ICollection<Researcher> Researchers {
        //    get => _researchers ?? (_researchers = new List<Researcher>());
        //    set => _researchers = value;
        //}
        //public virtual ICollection<User> Users {
        //    get => _users ?? (_users = new List<User>());
        //    set => _users = value;
        //}
    }
}