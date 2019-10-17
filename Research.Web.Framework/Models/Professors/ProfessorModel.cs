using FluentValidation.Attributes;
using Microsoft.AspNetCore.Mvc.Rendering;
using Research.Web.Models.Common;
using Research.Web.Validators.Professors;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Research.Web.Models.Professors
{
    /// <summary>
    /// A ... attached to an Researcher
    /// </summary>
   // [Validator(typeof(ProfessorValidator))]
    public class ProfessorModel : BaseEntityModel
    {
        public ProfessorModel()
        {
            AvailableTitles = new List<SelectListItem>();
            AvailableProvinces = new List<SelectListItem>();
            AddressModel = new AddressModel();
        }
       
        public int TitleId { get; set; }
        [Display(Name = "คำนำหน้า")]
        [Required]
        public string TitleName { get; set; }
        public IList<SelectListItem> AvailableTitles { get; set; }
        public IList<SelectListItem> AvailableProvinces { get; set; }
        [Required]
        [Display(Name = "ชื่อ")]
        public string FirstName { get; set; }
        [Required]
        [Display(Name = "นามสกุล")]
        public string LastName { get; set; }
        [DataType(DataType.EmailAddress)]
        [Display(Name = "อีเมล")]
        public string Email { get; set; }
        [Display(Name = "การใช้งาน")]
        public bool IsActive { get; set; }
        [Required]
        [Display(Name = "โทรศัพท์")]
        public string Telephone { get; set; }
        [Display(Name = "รหัสผู้ทรงคุณวุฒิ")]
        public string ProfessorCode { get; set; }
        [Display(Name = "ประเภทผู้ทรงคุณวุฒิ")]
        public string ProfessorType { get; set; }
        [Display(Name = "ประเภทผู้ทรงคุณวุฒิ")]
        public string ProfessorTypeName { get; set; }
        public AddressModel AddressModel { get; set; }
        [Display(Name = "หมายเหตุ")]
        public string Comment { get; set; }

    }
}
