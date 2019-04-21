using System;
using System.Collections.Generic;
using System.Text;

namespace Research.Data
{
    public class Agency : BaseEntity
    {
        //private ICollection<User> _users;
        public string Name { get; set; }

        //public virtual ICollection<User> Users {
        //    get => _users ?? (_users = new List<User>());
        //    set => _users = value;
        //}
        public virtual Researcher Researcher  { get;  set; }
    }
}
