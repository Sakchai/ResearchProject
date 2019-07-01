using Microsoft.AspNetCore.Mvc.Rendering;
using Research.Web.Models.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Research.Web.Models.FiscalSchedules
{
    /// <summary>
    /// A ... attached to an Researcher
    /// </summary>
    public class FiscalScheduleModel : BaseEntityModel
    {
        public FiscalScheduleModel()
        {
            AvailableFiscalYears = new List<SelectListItem>();
        }
        [Display(Name = "รหัสวันเปิดรับข้อเสนอโครงการวิจัย")]
        public string FiscalCode { get; set; }
        [Display(Name = "ปีงบประมาณ")]
        public int FiscalYear { get; set; }
        [Display(Name = "ครั้งที่")]
        public int FiscalTimes { get; set; }

        public IList<SelectListItem> AvailableFiscalYears { get; set; }
        [Display(Name = "ชื่อทุนวิจัย")]
        public string ScholarName { get; set; }
        [UIHint("Date")]
        [Display(Name = "วันเปิดรับข้อเสนอโครงการวิจัย")]
        public DateTime OpeningDate { get; set; }
        public string OpeningDateName { get; set; }
        [UIHint("Date")]
        [Display(Name = "วันปิดรับข้อเสนอโครงการวิจัย")]
        public DateTime ClosingDate { get; set; }
        public string ClosingDateName { get; set; }

    }
}
