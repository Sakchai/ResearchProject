using Research.Enum;
using System;
using System.Collections.Generic;

namespace Research.Data
{
    public partial class ProjectProgress : BaseEntity
    {
        public int ProjectId { get; set; }
        public DateTime ProgressStartDate { get; set; }
        public DateTime ProgressEndDate { get; set; }
        public int? InternalProfessorId { get; set; }
        public int? InternalProfessor2Id { get; set; }
        public int? ExternalProfessorId { get; set; }
        public string Comment { get; set; }

        public int? ProjectUploadId { get; set; }
        public int? ProgressStatusId { get; set; }
        public string LastUpdateBy { get; set; }
        public virtual Project Project { get; set; }
        public ProgressStatus ProgressStatus {
            get => (ProgressStatus)ProgressStatusId;
            set => ProgressStatusId = (int)value;
        }

        public virtual Professor InternalProfessor { get; set; }

        public virtual Professor InternalProfessor2 { get; set; }

        public virtual Professor ExternalProfessor { get; set; }
    }


}