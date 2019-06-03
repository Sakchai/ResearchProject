using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace Research.Web.Models.Messages
{
    /// <summary>
    /// Represents a message template search model
    /// </summary>
    public partial class MessageTemplateSearchModel : BaseSearchModel
    {
        #region Ctor

        public MessageTemplateSearchModel()
        {
            AvailableStores = new List<SelectListItem>();
        }

        #endregion

        #region Properties

        public int SearchStoreId { get; set; }
        public IList<SelectListItem> AvailableStores { get; set; }

        #endregion
    }
}