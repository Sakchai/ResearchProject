using Research.Web.Framework.Models;
using System;
using System.ComponentModel.DataAnnotations;

namespace Research.Web.Models.Logging
{
    /// <summary>
    /// Represents a log model
    /// </summary>
    public partial class LogModel : BaseEntityModel
    {
        #region Properties

        [Display(Name = "ประเภท Log")]
        public string LogLevel { get; set; }

        [Display(Name = "รายละเอียดสั้น")]
        public string ShortMessage { get; set; }

        [Display(Name = "รายละเอียดยาว")]
        public string FullMessage { get; set; }

        [Display(Name = "ไอพีเอดเดรส")]
        public string IpAddress { get; set; }

        [Display(Name = "หมายเลข User")]
        public int? UserId { get; set; }

        [Display(Name = "อีเมล")]
        public string UserEmail { get; set; }

        [Display(Name = "Page Url")]
        public string PageUrl { get; set; }

        [Display(Name = "Referrer Url")]
        public string ReferrerUrl { get; set; }

        [Display(Name = "สร้างรายการเมื่อ")]
        public DateTime CreatedOn { get; set; }

        #endregion
    }
}