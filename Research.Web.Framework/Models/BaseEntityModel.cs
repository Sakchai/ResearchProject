

namespace Research.Web.Models
{
    /// <summary>
    /// Represents base nopCommerce entity model
    /// </summary>
    public partial class BaseEntityModel : BaseResearchModel
    {
        /// <summary>
        /// Gets or sets model identifier
        /// </summary>
        public virtual int Id { get; set; }
    }
}