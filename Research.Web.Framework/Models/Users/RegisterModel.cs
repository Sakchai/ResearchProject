﻿using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using FluentValidation.Attributes;
using System.ComponentModel.DataAnnotations;
using Research.Web.Validators.Users;
namespace Research.Web.Models.Users
{
    /// <summary>
    /// Represents a register model
    /// </summary>
    [Validator(typeof(RegisterValidator))]
    public class RegisterModel : BaseResearchModel
    {
        public RegisterModel()
        {
            this.AvailableTitles = new List<SelectListItem>();
            this.AvailableAgencies = new List<SelectListItem>();
        }
        [Required]
        [DataType(DataType.EmailAddress)]
        [Display(Name = "อีเมล")]
        public string Email { get; set; }

        [Required]
        [StringLength(20, ErrorMessage = "{0}ต้องมีความยาวอย่างน้อย {2} ตัวอักษร!", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "รหัสผ่าน")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "ยื่นยันรหัสผ่าน")]
        [Compare("Password", ErrorMessage = "ระบุรหัสผ่านไม่เหมือนกัน กรุณาระบุอีกครั้ง!")]
        public string ConfirmPassword { get; set; }

        [Required]
        [Display(Name = "ชื่อ")]
        public string FirstName { get; set; }

        [Required]
        [Display(Name = "นามสกุล")]
        public string LastName { get; set; }


        [Required]
        [Display(Name = "เลขประจำตัวประชาชน")]
        public string IDCard { get; set; }


     
        public IList<SelectListItem> AvailableGenders { get; set; }

        [Display(Name = "เพศ")]
        public string Gender { get; set; }

        [Required]
        
        public IList<SelectListItem> AvailableTitles { get; set; }
        [Display(Name = "คำนำหน้า")]
        public int TitleId { get; set; }
        [Display(Name = "หน่วยงาน")]
        public int AgencyId { get; set; }
        [Required]
        
        public IList<SelectListItem> AvailableAgencies { get; set; }
    }
}
