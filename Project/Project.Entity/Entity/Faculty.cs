using System;
using System.Collections.Generic;

namespace Project.Entity
{
    public partial class Faculty : BaseEntity
    {
        public Faculty()
        {
            Researcher = new HashSet<Researcher>();
            User = new HashSet<User>();
        }

        public string Name { get; set; }
        public virtual ICollection<Researcher> Researcher { get; set; }
        public virtual ICollection<User> User { get; set; }
    }
}