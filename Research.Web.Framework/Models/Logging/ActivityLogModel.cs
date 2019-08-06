﻿using Research.Web.Framework.Models;
using System;

namespace Research.Web.Models.Logging
{
    /// <summary>
    /// Represents an activity log model
    /// </summary>
    public partial class ActivityLogModel : BaseEntityModel
    {
        #region Properties

     //   [NopResourceDisplayName("Admin.Configuration.ActivityLog.ActivityLog.Fields.ActivityLogType")]
        public string ActivityLogTypeName { get; set; }

        //[NopResourceDisplayName("Admin.Configuration.ActivityLog.ActivityLog.Fields.Customer")]
        public int UserId { get; set; }

        //[NopResourceDisplayName("Admin.Configuration.ActivityLog.ActivityLog.Fields.Customer")]
       // public string UserEmail { get; set; }

        //[NopResourceDisplayName("Admin.Configuration.ActivityLog.ActivityLog.Fields.Comment")]
        public string Comment { get; set; }

        //[NopResourceDisplayName("Admin.Configuration.ActivityLog.ActivityLog.Fields.CreatedOn")]
        public DateTime CreatedOn { get; set; }

        //[NopResourceDisplayName("Admin.Customers.Customers.ActivityLog.IpAddress")]
        public string IpAddress { get; set; }
        public string UserEmail { get; set; }
        #endregion
    }
}