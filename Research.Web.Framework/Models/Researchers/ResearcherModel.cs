using Microsoft.AspNetCore.Mvc.Rendering;
using Research.Web.Models.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Research.Web.Models.Researchers
{
    /// <summary>
    /// A ... attached to an Researcher
    /// </summary>
    public class ResearcherModel : BaseEntityModel
    {
        public ResearcherModel()
        {
            AvailableTitles = new List<SelectListItem>();
            AvailableAgencies = new List<SelectListItem>();
            AvailableAcademicRanks = new List<SelectListItem>();
            AvailablePersonalTypes = new List<SelectListItem>();
            AvailableAddEducationDegrees = new List<SelectListItem>();
            AvailableAddEducationEducationLevels = new List<SelectListItem>();
            AvailableAddEducationInstitutes = new List<SelectListItem>();
            AvailableAddEducationCountries = new List<SelectListItem>();
            Address = new AddressModel();
            ResearcherEducationSearchModel = new ResearcherEducationSearchModel();
        }
        [Display(Name = "คำนำหน้า(ไทย)")]
        public int TitleId { get; set; }
        public string TitleName { get; set; }
        public IList<SelectListItem> AvailableTitles { get; set; }
        [Display(Name = "หน่วยงาน")]
        public int? AgencyId { get; set; }
        public string AgencyName { get; set; }
        public IList<SelectListItem> AvailableAgencies { get; set; }
        [Display(Name = "ตำแหน่งทางวิชาการ")]
        public int? AcademicRankId { get; set; }
        public IList<SelectListItem> AvailableAcademicRanks { get; set; }
        [Display(Name = "ประเภทบุคลากร")]
        public int PersonalTypeId { get; set; }
        public string PersonalTypeName { get; set; }
        public IList<SelectListItem> AvailablePersonalTypes { get; set; }
        [Display(Name = "ชื่อ(ไทย)")]
        public string FirstName { get; set; }
        [Display(Name = "นามสกุล(ไทย)")]
        public string LastName { get; set; }
        [Display(Name = "ชื่อ(อังกฤษ)")]
        public string FirstNameEN { get; set; }
        [Display(Name = "นามสกุล(อังกฤษ)")]
        public string LastNameEN { get; set; }
        public int? DateOfBirthDay { get; set; }
        public int? DateOfBirthMonth { get; set; }
        public int? DateOfBirthYear { get; set; }

        public DateTime? ParseDateOfBirth()
        {
            if (!DateOfBirthYear.HasValue || !DateOfBirthMonth.HasValue || !DateOfBirthDay.HasValue)
                return null;

            DateTime? dateOfBirth = null;
            try
            {
                dateOfBirth = new DateTime(DateOfBirthYear.Value - 543 , DateOfBirthMonth.Value, DateOfBirthDay.Value);
            }
            catch { }
            return dateOfBirth;
        }
        [Display(Name = "เพศ")]
        public string Gender { get; set; }
        [Display(Name = "เลขประจำตัวประชาชน")]
        public string IDCard { get; set; }
        [Display(Name = "รหัสผู้วิจัย")]
        public string ResearcherCode { get; set; }

        [DataType(DataType.EmailAddress)]
        [Display(Name = "อีเมล")]
        public string Email { get; set; }
        public bool IsActive { get; set; }
        public bool IsAcceptedConditions { get; set; }
        [Display(Name = "โทรศัพท์")]
        public string Telephone { get; set; }
        public string LastUpdateBy { get; set; }
        [UIHint("Picture")]
        [Display(Name = "รูปภาพนักวิจัย")]
        public int? PictureId { get; set; }
        public AddressModel Address { get; set; }
        public string FullName { get => $"{TitleName}{FirstName} {LastName}"; }
    
        public bool IsCompleted { get => IsActive && IsAcceptedConditions; }

        #region Researcher educations

        [Display(Name = "ระดับปริญญา")]
        public int AddEducationDegreeId { get; set; }
        public IList<SelectListItem> AvailableAddEducationDegrees { get; set; }

        [Display(Name = "วุฒิการศึกษา")]
        public int AddEducationEducationLevelId { get; set; }
        public IList<SelectListItem> AvailableAddEducationEducationLevels { get; set; }

        [Display(Name = "สถาบันการศึกษา")]
        public int AddEducationInstituteId { get; set; }
        public IList<SelectListItem> AvailableAddEducationInstitutes { get; set; }

        [Display(Name = "ประเทศ")]
        public int AddEducationCountryId { get; set; }
        public IList<SelectListItem> AvailableAddEducationCountries { get; set; }

        [Display(Name = "ปีการศึกษาที่จบ")]
        public int AddEducationGraduationYear { get; set; }
        public ResearcherEducationSearchModel ResearcherEducationSearchModel { get; set; }
        #endregion
    }
}
