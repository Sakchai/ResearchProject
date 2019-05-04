using Research.Web.Framework.Models;
using System;

namespace Research.Web.Models.Logging
{
    /// <summary>
    /// Represents a log model
    /// </summary>
    public partial class LogModel : BaseEntityModel
    {
        #region Properties

        //[NopResourceDisplayName("Admin.System.Log.Fields.LogLevel")]
        public string LogLevel { get; set; }

        //[NopResourceDisplayName("Admin.System.Log.Fields.ShortMessage")]
        public string ShortMessage { get; set; }

        //[NopResourceDisplayName("Admin.System.Log.Fields.FullMessage")]
        public string FullMessage { get; set; }

        //[NopResourceDisplayName("Admin.System.Log.Fields.IPAddress")]
        public string IpAddress { get; set; }

        //[NopResourceDisplayName("Admin.System.Log.Fields.Customer")]
        public int? UserId { get; set; }

        //[NopResourceDisplayName("Admin.System.Log.Fields.Customer")]
        public string UserEmail { get; set; }

        //[NopResourceDisplayName("Admin.System.Log.Fields.PageURL")]
        public string PageUrl { get; set; }

        //[NopResourceDisplayName("Admin.System.Log.Fields.ReferrerURL")]
        public string ReferrerUrl { get; set; }

        //[NopResourceDisplayName("Admin.System.Log.Fields.CreatedOn")]
        public DateTime CreatedOn { get; set; }

        #endregion
    }
}