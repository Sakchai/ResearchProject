using System;
using System.Collections.Generic;

namespace Research.Data
{
    public partial class Download : BaseEntity
    {
        //private ICollection<ProjectProgress> _projectProgresses;
        //private ICollection<Project> _projects;
        //public Download()
        //{
        //    DownloadGuid = new Guid();
        //}

        public Guid DownloadGuid { get; set; }
        public bool UseDownloadUrl { get; set; }
        public string DownloadUrl { get; set; }
        public byte[] DownloadBinary { get; set; }
        public string ContentType { get; set; }
        public string Filename { get; set; }
        public string Extension { get; set; }
        public bool IsNew { get; set; }

        //public virtual ICollection<Project> Projects {
        //    get => _projects ?? (_projects = new List<Project>());
        //    set => _projects = value;
        //}
        //public virtual ICollection<ProjectProgress> ProjectProgresses {
        //    get => _projectProgresses ?? (_projectProgresses = new List<ProjectProgress>());
        //    set => _projectProgresses = value;
        //}
    }
}