namespace Project.Web.Common
{
    public abstract partial class BaseSearchModel
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
    }
}