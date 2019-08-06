using Research.Core.Domain.Common;
using Research.Infrastructure;
using Research.Web.Framework.Models;

namespace Research.Web.Models
{
    public abstract partial class BaseSearchModel : BaseResearchModel, IPagingRequestModel
    {
        public BaseSearchModel()
        {
            //set the default values
            this.Page = 1;
            this.PageSize = 10;
        }


        /// <summary>
        /// Gets or sets a page number
        /// </summary>
        public int Page { get; set; }

        /// <summary>
        /// Gets or sets a page size
        /// </summary>
        public int PageSize { get; set; }

        /// <summary>
        /// Gets or sets a comma-separated list of available page sizes
        /// </summary>
        public string AvailablePageSizes { get; set; }

        #region Methods

        /// <summary>
        /// Set grid page parameters
        /// </summary>
        public void SetGridPageSize()
        {
            var adminAreaSettings = EngineContext.Current.Resolve<AdminAreaSettings>();

            this.Page = 1;
            this.PageSize = adminAreaSettings.DefaultGridPageSize;
            this.AvailablePageSizes = adminAreaSettings.GridPageSizes;
        }

        /// <summary>
        /// Set popup grid page parameters
        /// </summary>
        public void SetPopupGridPageSize()
        {
            var adminAreaSettings = EngineContext.Current.Resolve<AdminAreaSettings>();

            this.Page = 1;
            this.PageSize = adminAreaSettings.PopupGridPageSize;
            this.AvailablePageSizes = adminAreaSettings.GridPageSizes;
        }

        #endregion
    }
}