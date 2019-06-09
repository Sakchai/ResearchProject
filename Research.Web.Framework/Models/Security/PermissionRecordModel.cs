

namespace Research.Web.Models.Security
{
    /// <summary>
    /// Represents a permission record model
    /// </summary>
    public partial class PermissionRecordModel : BaseResearchModel
    {
        #region Properties

        public string Name { get; set; }

        public string SystemName { get; set; }

        #endregion
    }
}