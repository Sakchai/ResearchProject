using System;
using System.Collections.Generic;

namespace Project.Entity
{
    public partial class PictureBinary : BaseEntity
    {
        public PictureBinary()
        {
            Researcher = new HashSet<Researcher>();
        }

        public byte[] BinaryData { get; set; }
        public int PictureId { get; set; }

        public virtual Picture Picture { get; set; }
        public virtual ICollection<Researcher> Researcher { get; set; }
    }
}