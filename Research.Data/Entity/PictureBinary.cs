using System;
using System.Collections.Generic;

namespace Research.Data
{
    public partial class PictureBinary : BaseEntity
    {

        public byte[] BinaryData { get; set; }
        public int PictureId { get; set; }

        public virtual Picture Picture { get; set; }

    }
}