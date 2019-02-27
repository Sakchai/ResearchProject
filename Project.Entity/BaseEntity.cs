using System;
using Dapper.Contrib;
using Dapper.Contrib.Extensions;

namespace Project.Entity
{
    public class BaseEntity
    {
        [Key]
        public int Id { get; set; }
        public DateTime Created { get; set; }
        public DateTime Modified { get; set; }
    }
}
