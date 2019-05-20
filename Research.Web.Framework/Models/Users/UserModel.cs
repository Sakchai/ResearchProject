using Microsoft.AspNetCore.Mvc.Rendering;
using Research.Web.Models.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Research.Web.Models.Users
{
    /// <summary>
    /// A ... attached to an Researcher
    /// </summary>
    public class UserModel : BaseEntityModel
    {
        public UserModel()
        {
            AvailableTitles = new List<SelectListItem>();
            AvailableAgencyies = new List<SelectListItem>();
            AvailableUserRoles = new List<SelectListItem>();
        }
        [Display(Name = "รหัสผู้ดูแลระบบ")]
        public string UserName { get; set; }

        [Display(Name = "ชื่อ")]
        public string FirstName { get; set; }

        [Display(Name = "นามสกุล")]
        public string LastName { get; set; }
        [Display(Name = "คำนำหน้าชื่อ")]
        public int TitleId { get; set; }
        public IList<SelectListItem> AvailableTitles { get; set; }

        [Display(Name = "อีเมล")]
        public string Email { get; set; }
        [Display(Name = "หน่วยงาน")]
        public int AgencyId { get; set; }
        public IList<SelectListItem> AvailableAgencyies { get; set; }

        public string AgencyName { get; set; }
        [Display(Name = "เบอร์โทรศัพท์")]
        public string MobileNumber { get; set; }
        [Display(Name = "สถานะเข้าใช้งานระบบ")]
        public bool IsActive { get; set; }
        [Display(Name = "หมายเหตุ")]
        public string Description { get; set; }
        [Display(Name = "ระดับสิทธิ์")]
        public int UserTypeId { get; set; }
        public IList<SelectListItem> AvailableUserRoles { get; set; }
        public string FullName { get; set; }
        public string UserRoleName { get; set; }


    }

}
