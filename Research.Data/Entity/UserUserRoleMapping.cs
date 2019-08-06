using Research.Data;

namespace Research.Data
{
    /// <summary>
    /// Represents a customer-customer role mapping class
    /// </summary>
    public partial class UserUserRoleMapping : BaseEntity
    {
        /// <summary>
        /// Gets or sets the customer identifier
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// Gets or sets the customer role identifier
        /// </summary>
        public int UserRoleId { get; set; }

        /// <summary>
        /// Gets or sets the customer
        /// </summary>
        public virtual User User { get; set; }

        /// <summary>
        /// Gets or sets the customer role
        /// </summary>
        public virtual UserRole UserRole { get; set; }
    }
}