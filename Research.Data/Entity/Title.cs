using Research.Enum;
using System;
using System.Collections.Generic;

namespace Research.Data
{
    public partial class Title : BaseEntity
    {

        public string TitleNameTH { get; set; }
        public string TitleNameEN { get; set; }
        public string Gender { get; set; }
        public DateTime Created { get; set; }

    }
}