using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Research.Web.Models.Logging
{
    /// <summary>
    /// Represents an activity log search model
    /// </summary>
    public partial class ActivityLogSearchModel : BaseSearchModel
    {
        #region Ctor

        public ActivityLogSearchModel()
        {
            ActivityLogType = new List<SelectListItem>();
        }

        #endregion

        #region Properties

       // [NopResourceDisplayName("Admin.Configuration.ActivityLog.ActivityLog.Fields.CreatedOnFrom")]
        [UIHint("DateNullable")]
        [Display(Name = "เริ่มวันที่")]
        public DateTime? CreatedOnFrom { get; set; }

        //[NopResourceDisplayName("Admin.Configuration.ActivityLog.ActivityLog.Fields.CreatedOnTo")]
        [UIHint("DateNullable")]
        [Display(Name = "สิ้นสุดวันที่")]
        public DateTime? CreatedOnTo { get; set; }

        //[NopResourceDisplayName("Admin.Configuration.ActivityLog.ActivityLog.Fields.ActivityLogType")]
        [Display(Name = "ประเภท Log")]
        public int ActivityLogTypeId { get; set; }

        //[NopResourceDisplayName("Admin.Configuration.ActivityLog.ActivityLog.Fields.ActivityLogType")]
       
        public IList<SelectListItem> ActivityLogType { get; set; }

        //[NopResourceDisplayName("Admin.Customers.Customers.ActivityLog.IpAddress")]
        [Display(Name = "ไอพีเอดเดรส")]
        public string IpAddress { get; set; }

        #endregion
    }
}