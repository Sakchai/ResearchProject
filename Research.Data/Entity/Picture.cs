using System;
using System.Collections.Generic;

namespace Research.Data
{
    public partial class Picture : BaseEntity
    {
        //public Picture()
        //{
        //}

        public string MimeType { get; set; }
        public string SeoFilename { get; set; }
        public string AltAttribute { get; set; }
        public string TitleAttribute { get; set; }
        public bool IsNew { get; set; }

        /// <summary>
        /// Gets or sets the picture binary
        /// </summary>
        public virtual PictureBinary PictureBinary { get; set; }
    }
}