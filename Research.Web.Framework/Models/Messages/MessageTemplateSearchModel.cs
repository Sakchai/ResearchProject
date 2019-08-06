using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Research.Web.Models.Messages
{
    /// <summary>
    /// Represents a message template search model
    /// </summary>
    public partial class MessageTemplateSearchModel : BaseSearchModel
    {

        #region Properties

        [Display(Name = "หัวข้อเอกสาร")]
        public string Subject { get; set; }

        #endregion
    }
}