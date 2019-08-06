using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Research.Web.Models.Common
{
   // [Validator(typeof(AddressValidator))]
    public partial class AddressModel : BaseEntityModel
    {
        public AddressModel()
        {
            AvailableProvinces = new List<SelectListItem>();
        }
        [Display(Name = "ที่อยู่ 1")]
        public string Address1 { get; set; }
        [Display(Name = "ที่อยู่ 2")]
        public string Address2 { get; set; }
        [Display(Name = "รหัสไปรษณีย์")]
        public string ZipCode { get; set; }
        [Display(Name = "จังหวัด")]
        public int? ProvinceId { get; set; }
        public string ProvinceName { get; set; }

        public IList<SelectListItem> AvailableProvinces { get; set; }

    }
}