using System;
using System.Collections.Generic;

namespace Project.Entity
{
    public partial class Picture : BaseEntity
    {
        public Picture()
        {
            PictureBinary = new HashSet<PictureBinary>();
        }

        public string MimeType { get; set; }
        public string SeoFilename { get; set; }
        public string AltAttribute { get; set; }
        public string TitleAttribute { get; set; }
        public bool IsNew { get; set; }

        public virtual ICollection<PictureBinary> PictureBinary { get; set; }
    }
}