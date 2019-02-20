using System;
using System.Collections.Generic;

namespace Project.Entity
{
    public partial class Download : BaseEntity
    {
        public Download()
        {
            ProjectCompletedUpload = new HashSet<Project>();
            ProjectProposalUpload = new HashSet<Project>();
        }

        public Guid DownloadGuid { get; set; }
        public bool UseDownloadUrl { get; set; }
        public string DownloadUrl { get; set; }
        public byte[] DownloadBinary { get; set; }
        public string ContentType { get; set; }
        public string Filename { get; set; }
        public string Extension { get; set; }
        public bool IsNew { get; set; }

        public virtual ICollection<Project> ProjectCompletedUpload { get; set; }
        public virtual ICollection<Project> ProjectProposalUpload { get; set; }
    }
}