using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;
using Research.Web.Framework.Models;

namespace Research.Web.Models.Messages
{
    /// <summary>
    /// Represents a message template model
    /// </summary>
   // [Validator(typeof(MessageTemplateValidator))]
    public partial class MessageTemplateModel : BaseEntityModel
    {
        #region Ctor

        public MessageTemplateModel()
        {
            AvailableEmailAccounts = new List<SelectListItem>();
        }

        #endregion

        #region Properties

        [Display(Name = "แสดง Tokens")]
        public string AllowedTokens { get; set; }

        public string Name { get; set; }

        [Display(Name = "Bcc อีเมล์")]
        public string BccEmailAddresses { get; set; }

        public string Subject { get; set; }

        //[NopResourceDisplayName("Admin.ContentManagement.MessageTemplates.Fields.Body")]
        public string Body { get; set; }

        [Display(Name = "สถานะ")]
        public bool IsActive { get; set; }

        [Display(Name = "ส่งทันที")]
        public bool SendImmediately { get; set; }

        //[NopResourceDisplayName("Admin.ContentManagement.MessageTemplates.Fields.DelayBeforeSend")]
        [UIHint("Int32Nullable")]
        public int? DelayBeforeSend { get; set; }

        public int DelayPeriodId { get; set; }

        public bool HasAttachedDownload { get; set; }
        [Display(Name = "ไฟล์แนบ")]
        [UIHint("Download")]
        public int AttachedDownloadId { get; set; }

        [Display(Name = "อีเมล์ที่่ใช้ส่ง")]
        public int EmailAccountId { get; set; }

        public IList<SelectListItem> AvailableEmailAccounts { get; set; }

        //store mapping
        //[NopResourceDisplayName("Admin.ContentManagement.MessageTemplates.Fields.LimitedToStores")]
        //public IList<int> SelectedStoreIds { get; set; }

        //public IList<SelectListItem> AvailableStores { get; set; }

        //comma-separated list of stores used on the list page
        //[NopResourceDisplayName("Admin.ContentManagement.MessageTemplates.Fields.LimitedToStores")]
        //public string ListOfStores { get; set; }

        
        #endregion
    }

}