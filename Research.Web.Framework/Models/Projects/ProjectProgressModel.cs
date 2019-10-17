
using Microsoft.AspNetCore.Mvc.Rendering;
using Research.Data;
using Research.Enum;
using Research.Web.Framework.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Research.Web.Models.Projects
{
    public class ProjectProgressModel : BaseEntityModel
    {
        public ProjectProgressModel()
        {
            AvailableProgressStatuses = new List<SelectListItem>();
        }
        public int ProjectId { get; set; }
        [UIHint("Date")]
        public DateTime ProgressStartDate { get; set; }
        public string ProgressStartDateName { get; set; }
        [UIHint("Date")]
        public DateTime ProgressEndDate { get; set; }
        public string ProgressEndDateName { get; set; }
        public int ProgressStatusId { get; set; }
        public string ProgressStatusName { get; set; }
        public string Comment { get; set; }
        public DateTime Modified { get; set; }
        public string ModifiedName { get; set; }
        public string LastUpdateBy { get; set; }
        public string DownloadGuid { get; set; }
        public IList<SelectListItem> AvailableProgressStatuses { get; set; }
    }
}
